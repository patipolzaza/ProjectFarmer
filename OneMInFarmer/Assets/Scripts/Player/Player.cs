using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 275;
    private Vector2 moveInput;

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
        facingDirection = transform.localScale.x / transform.localScale.x;
        wallet = new Wallet(10);
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

        if (targetInteractable)
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                if (holdingObject is Item)
                {
                    if (holdingObject is AnimalFood && targetInteractable is Animal)
                    {
                        UseItem();
                        return;
                    }
                }

                Interact();
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
    }

    public void FixedUpdate()
    {
        Move();
    }

    public void SetHoldingItem(PickableObject item)
    {
        Transform itemTransform;
        Collider2D itemCollider;
        Vector2 newPosition = new Vector2();

        if (item)
        {
            if (holdingObject)
            {
                holdingObject.Drop(this);
            }

            ChangeTargetInteractable(null);

            item.SetInteractable(false);
            item.HideObjectHighlight();
            itemTransform = item.transform;
            itemCollider = item.objectCollider;

            newPosition.Set(0, itemCollider.bounds.extents.y - itemCollider.offset.y);
            itemTransform.SetParent(itemHolderTransform);
            itemTransform.localPosition = newPosition;
            holdingObject = item;
        }
        else
        {
            if (holdingObject)
            {
                itemTransform = holdingObject.transform;
                itemCollider = holdingObject.objectCollider;

                itemTransform.SetParent(itemDropTransform);
                newPosition.Set(itemCollider.bounds.extents.x - itemCollider.offset.x, itemCollider.bounds.extents.y - itemCollider.offset.y);
                itemTransform.localPosition = newPosition;
                itemTransform.SetParent(null);
                holdingObject.SetInteractable(true);
                holdingObject = null;
            }
        }
    }

    private void CheckMoveInput()
    {
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

    private void DropItem()
    {
        if (holdingObject)
        {
            holdingObject.Drop(this);
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(interactableDetector.position, interactableDetectRange);
    }
}
