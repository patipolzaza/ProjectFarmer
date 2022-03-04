using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class Player : MonoBehaviour
{
    public float moveSpeed { get; private set; } = 1.75f;
    private Vector2 moveInput;

    [SerializeField] private Transform interactableDetector;
    [SerializeField] private float interactableDetectRange;
    [SerializeField] private LayerMask interactableLayerMask;

    public Item inHandItem { get; private set; }
    public Interactable targetInteractable { get; private set; }
    private bool isDetectInteractable = false;

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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Interact();
        }
    }

    private void FixedUpdate()
    {

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
        float velocityX = moveInput.x * moveSpeed;
        float velocityY = moveInput.y * moveSpeed;

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
        if (targetInteractable)
        {
            targetInteractable.Interact();
        }
    }

    private bool CheckInteractableInRange()
    {
        Collider2D hit = Physics2D.OverlapCircle(interactableDetector.position, interactableDetectRange, interactableLayerMask);
        if (hit)
        {
            Interactable interactable = hit.GetComponent<Interactable>();
            if (targetInteractable && interactable != targetInteractable)
            {
                targetInteractable.HideObjectHighlight();
            }

            interactable.ShowObjectHighlight();
            targetInteractable = interactable;
            return true;
        }
        else
        {
            if (targetInteractable)
            {
                targetInteractable.HideObjectHighlight();
            }
            return false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(interactableDetector.position, interactableDetectRange);
    }
}
