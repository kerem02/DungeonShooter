using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    public Rigidbody2D rb;
    public Camera cam;
    public SpriteRenderer charspriteRenderer;

    Vector2 movement;
    Vector2 mousePos;

    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        charspriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
    }

    void FixedUpdate()
    {
        if (rb.velocity == new Vector2(0, 0))
        {
            animator.SetBool("isWalking", false);
        }
        else
            animator.SetBool("isWalking", true);

        handleMovement();

        UpdateDirection();

    }

    void handleMovement()
    {
        rb.velocity = movement.normalized * moveSpeed;
    }

    void UpdateDirection()
    {
        Vector2 lookDir = mousePos - rb.position;
        charspriteRenderer.flipX = lookDir.x > 0;
    }
}