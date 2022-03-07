using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 800;
    private Vector2 moveInput;

    [SerializeField] private Transform interactableDetector;
    [SerializeField] private float interactableDetectRange;
    [SerializeField] private LayerMask interactableLayerMask;

    public Item holdingItem { get; private set; }
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
    }

    private void OnValidate()
    {
        if (!rb)
        {
            rb = GetComponent<Rigidbody2D>();
            rb.isKinematic = true;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        if (!anim)
        {
            anim = GetComponent<Animator>();
        }
    }

    private void Update()
    {
        isDetectInteractable = CheckInteractableInRange();
        CheckMoveInput();
        Move();

        if (Input.GetKeyDown(KeyCode.J))
        {
            Interact();
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            DropItem();
        }
    }

    private void FixedUpdate()
    {

    }

    public void SetHoldingItem(Item item)
    {
        Transform itemTransform;
        Collider2D itemCollider;
        Vector2 newPosition = new Vector2();

        if (item)
        {
            if (holdingItem)
            {
                holdingItem.Drop(this);
            }

            item.SetInteractable(false);
            item.HideObjectHighlight();
            itemTransform = item.transform;
            itemCollider = item.objectCollider;

            newPosition.Set(itemHolderTransform.position.x, itemHolderTransform.position.y + itemCollider.bounds.size.y / 2);
            itemTransform.SetParent(itemHolderTransform);
            itemTransform.position = newPosition;
            holdingItem = item;
        }
        else
        {
            if (holdingItem)
            {
                itemTransform = holdingItem.transform;
                itemCollider = holdingItem.objectCollider;

                newPosition.Set(itemDropTransform.position.x, itemDropTransform.position.y + itemCollider.bounds.size.y / 2);
                holdingItem.SetInteractable(true);
                holdingItem.HideObjectHighlight();
                itemTransform.SetParent(null);
                itemTransform.position = newPosition;
                holdingItem = null;
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

    private void Move()
    {
        float velocityX = moveInput.x * moveSpeed * Time.deltaTime;
        float velocityY = moveInput.y * moveSpeed * Time.deltaTime;

        velocityWorkspace.Set(velocityX, velocityY);

        rb.velocity = velocityWorkspace;
    }

    private void Flip()
    {
        facingDirection *= -1;
        transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
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
        if (holdingItem)
        {
            holdingItem.Drop(this);
        }
    }

    private bool CheckInteractableInRange()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(interactableDetector.position, interactableDetectRange, interactableLayerMask);
        if (hits.Length > 0)
        {
            foreach (var hit in hits)
            {
                Interactable interactable = hit.GetComponent<Interactable>();
                if (interactable && interactable.isInteractable)
                {
                    if (targetInteractable)
                    {
                        float distanceBetweenOldTarget = Vector2.Distance(transform.position, targetInteractable.objectCollider.bounds.center);
                        float distanceBetweenNewTarget = Vector2.Distance(transform.position, interactable.objectCollider.bounds.center);
                        
                        if (distanceBetweenNewTarget < distanceBetweenOldTarget)
                        {
                            if (interactable != targetInteractable)
                            {
                                targetInteractable.HideObjectHighlight();
                            }

                            targetInteractable = interactable;
                        }
                    }
                    else
                    {
                        targetInteractable = interactable;
                    }
                }
            }

            if (targetInteractable)
            {
                targetInteractable.ShowObjectHighlight();
                return true;
            }
        }

        if (targetInteractable)
        {
            targetInteractable.HideObjectHighlight();
            targetInteractable = null;
        }

        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(interactableDetector.position, interactableDetectRange);
    }
}
