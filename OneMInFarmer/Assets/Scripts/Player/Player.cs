using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class Player : MonoBehaviour
{
    public float moveSpeed { get; private set; } = 250;
    private Vector2 moveInput;

    public Item inHandItem { get; private set; }
    public Item targetItem { get; private set; }

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
        CheckMoveInput();

        Move();
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
        if (inHandItem)
        {

        }
        else if (targetItem)
        {
            //TODO: 
        }
    }

    private void PickupItem()
    {
        if (targetItem)
        {
            if (!inHandItem)
            {
                //GameObject targetObject;
            }
            else
            {
                DropItem();
            }
        }
    }

    private void DropItem()
    {
        if (inHandItem)
        {

        }
        else
        {
            PickupItem();
        }
    }
}
