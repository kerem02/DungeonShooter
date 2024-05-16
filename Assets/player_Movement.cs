using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    public Rigidbody2D rb;
    public Camera cam;

    Vector2 movement;
    Vector2 mousePos;

    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
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

        Vector2 lookDir = mousePos - rb.position;
        if (lookDir.x > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (lookDir.x < 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

    }

    void handleMovement()
    {
        rb.velocity = movement.normalized * moveSpeed;
    }
}