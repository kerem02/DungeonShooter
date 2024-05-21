using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayerMovement : MonoBehaviour, IDamageble
{
    public float moveSpeed = 5f;

    public Rigidbody2D rb;
    public Camera cam;
    public SpriteRenderer charspriteRenderer;
    [SerializeField] private TrailRenderer tr;

    private float dashSpeed = 10f;
    private float dashTime = 0.2f;
    private float nextDashTime = 0f;
    private float dashCooldown = 1f;

    private bool isDashing = false;
    

    Vector2 movement;
    Vector2 mousePos;

    Animator animator;

    [field: SerializeField]public float health { get; set; }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        charspriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && Time.time >= nextDashTime)
        {
            StartCoroutine(Dash());
            nextDashTime = Time.time + dashCooldown;
        }
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

        if(isDashing == false)
        {
            handleMovement();
        }
        

        UpdateDirection();

    }

    void handleMovement()
    {
        rb.velocity = movement.normalized * moveSpeed;
    }

    void UpdateDirection()
    {
        Vector2 lookDir = (mousePos - rb.position).normalized;
        charspriteRenderer.flipX = lookDir.x > 0;
    }

    IEnumerator Dash()
    {
        Vector2 dashDirection = movement.normalized;
        float startTime = Time.time;

        isDashing = true;
        tr.emitting = true;
        while (Time.time < startTime + dashTime)
        {
            rb.velocity = dashDirection * dashSpeed;
            yield return null;
        }
        tr.emitting = false;
        isDashing = false;
    }

    public void takeDamage(float damage)
    {
        health -= damage;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("MeleeEnemy"))
        {
            takeDamage(25);
        }
    }
}