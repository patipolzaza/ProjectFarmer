using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class Player : MonoBehaviour
{
    public float moveSpeed { get; private set; }
    private Vector2 moveInput;

    public float facingDirection { get; private set; }

    private Rigidbody2D rb;
    private Animator anim;

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

    private void Flip()
    {
        facingDirection *= -1;
        transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
    }
}
