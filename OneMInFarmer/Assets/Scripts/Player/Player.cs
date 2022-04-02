using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    private Vector2 moveInput;
    private bool canMove = true;

    [SerializeField] private GameObject characterObject;

    public Wallet wallet { get; private set; }

    [SerializeField] private Transform interactableDetector;
    [SerializeField] private float interactableDetectRange;
    [SerializeField] private LayerMask interactableLayerMask;

    public PickableObject holdingObject { get; private set; }
    public Interactable targetInteractable { get; private set; }
    private bool isDetectInteractable = false;

    [SerializeField] private Transform itemHolderTransform;
    [SerializeField] private Transform itemDropTransform;

    public float facingDirection { get; private set; }

    private Rigidbody2D rb;
    private Animator anim;

    private Vector2 velocityWorkspace;

    private void Awake()
    {
        facingDirection = transform.localScale.x / Mathf.Abs(transform.localScale.x);
        wallet = new Wallet(10);

        Instance = this;
    }

    private void OnValidate()
    {
        if (!rb)
        {
            rb = GetComponent<Rigidbody2D>();
            rb.isKinematic = false;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        if (!anim)
        {
            anim = characterObject.GetComponent<Animator>();
        }
    }

    private void Update()
    {
        isDetectInteractable = CheckInteractableInRange();
        CheckMoveInput();

        if (isDetectInteractable && targetInteractable)
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                if (holdingObject)
                {
                    if (holdingObject is IValuable && targetInteractable is ShopForSell)
                    {
                        IValuable valuable = holdingObject as IValuable;
                        ShopForSell shop = targetInteractable as ShopForSell;

                        if (shop.PutItemInContainer(valuable))
                        {
                            holdingObject = null;
                        }
                    }
                    else if (holdingObject is AnimalFood && targetInteractable is Animal)
                    {
                        UseItem();
                    }
                    else if (targetInteractable is PickableObject)
                    {
                        PickUpItem((PickableObject)targetInteractable);
                    }
                    else
                    {
                        Interact();
                    }
                }
                else if (targetInteractable is PickableObject)
                {
                    PickUpItem((PickableObject)targetInteractable);
                }
                else
                {
                    Interact();
                }
            }
            else if (Input.GetKey(KeyCode.J))
            {
                if (holdingObject is WateringPot)
                {
                    if (holdingObject is WateringPot && targetInteractable is Pool)
                    {
                        holdingObject.GetComponent<WateringPot>().Refill();
                        return;
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            DropItem();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            wallet.EarnCoin(5);
        }
    }

    public void FixedUpdate()
    {
        Move();
    }
    private void CheckMoveInput()
    {
        if (!canMove)
        {
            return;
        }

        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");

        if (moveInput.magnitude != 0)
        {
            if (facingDirection > 0 && inputX < 0)
            {
                Flip();
            }
            else if (facingDirection < 0 && inputX > 0)
            {
                Flip();
            }
        }

        moveInput.Set(inputX, inputY);
    }

    public void UseItem()
    {
        if (holdingObject is Item)
        {
            Item useableItem = holdingObject as Item;
            if (useableItem.Use(targetInteractable))
            {
                holdingObject = null;
            }
        }
    }

    private void Move()
    {
        int moveSpeed = StatusUpgradeManager.Instance.moveSpeedStatus.GetValue;

        float velocityX = moveInput.x * moveSpeed * Time.fixedDeltaTime;
        float velocityY = moveInput.y * moveSpeed * Time.fixedDeltaTime;

        velocityWorkspace.Set(velocityX, velocityY);

        rb.velocity = velocityWorkspace;
    }

    private void Flip()
    {
        facingDirection *= -1;
        characterObject.transform.localScale = new Vector2(-characterObject.transform.localScale.x, characterObject.transform.localScale.y);
    }

    private void Interact()
    {
        if (isDetectInteractable && targetInteractable)
        {
            targetInteractable.Interact(this);
        }
    }

    public void PickUpItem(PickableObject itemToPick)
    {
        if (holdingObject)
        {
            DropItem();
        }

        Transform itemTransform = itemToPick.Pick(this);

        itemToPick.SetParent(itemHolderTransform);
        itemToPick.SetLocalPosition(new Vector3(0, 0, 1), false, true);

        holdingObject = itemTransform.GetComponent<PickableObject>();
    }

    public void DropItem()
    {
        if (holdingObject)
        {
            PickableObject pickable = holdingObject;

            pickable.SetParent(itemDropTransform);
            pickable.SetLocalPosition(new Vector3(0, 0, 1), true, true);
            pickable.Drop();

            pickable.SetInteractable(true);

            holdingObject = null;
        }
    }

    private bool CheckInteractableInRange()
    {
        ChangeTargetInteractable(null);
        Collider2D[] hits = Physics2D.OverlapCircleAll(interactableDetector.position, interactableDetectRange, interactableLayerMask);

        if (hits.Length == 0)
        {
            return false;
        }

        foreach (var hit in hits)
        {
            Interactable interactable = hit.GetComponent<Interactable>();

            if (interactable.Equals(holdingObject) || interactable.Equals(targetInteractable))
            {
                continue;
            }

            if (interactable && interactable.isInteractable)
            {
                if (targetInteractable)
                {
                    float distanceBetweenOldTarget = Vector2.Distance(transform.position, targetInteractable.objectCollider.bounds.center);
                    float distanceBetweenNewTarget = Vector2.Distance(transform.position, interactable.objectCollider.bounds.center);

                    if (distanceBetweenNewTarget < distanceBetweenOldTarget)
                    {
                        ChangeTargetInteractable(interactable);
                    }
                }
                else
                {
                    ChangeTargetInteractable(interactable);
                }
            }

        }

        if (targetInteractable)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void ChangeTargetInteractable(Interactable newInteractable)
    {
        if (targetInteractable)
        {
            targetInteractable.HideObjectHighlight();
        }

        targetInteractable = newInteractable;

        if (targetInteractable)
        {
            targetInteractable.ShowObjectHighlight();
        }
    }

    public void EnableMove()
    {
        canMove = true;
    }

    public void DisableMove()
    {
        canMove = false;
        rb.velocity = Vector3.zero;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(interactableDetector.position, interactableDetectRange);
    }
}
