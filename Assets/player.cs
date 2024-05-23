using BarthaSzabolcs.Tutorial_SpriteFlash;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class player : MonoBehaviour, IDamageble
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
    private float meleeknockbackForce = 10f;
 

    private bool isDashing = false;

    public HealthBar healthBar;

    public GameObject hitEffect;


    Vector2 movement;
    Vector2 mousePos;

    Animator animator;

    [field: SerializeField] public float health { get; set; }
    [field: SerializeField] public bool isKnocking { get; set; }
    [field: SerializeField] public float knockbackDuration { get; set; }
    [field: SerializeField] public SimpleFlash flashEffect { get ; set; }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        charspriteRenderer = GetComponent<SpriteRenderer>();
        healthBar.setMaxHealth(health);
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

        if(isDashing == false && isKnocking == false)
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

    public void takeDamage(float damage, Vector2 direction, float knockbackForce)
    {
        Instantiate(hitEffect, transform.position, transform.rotation); 
        health -= damage;
        healthBar.setHealth(health);
        flashEffect.Flash();
        StartCoroutine(Knockback(direction, knockbackForce));
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("MeleeEnemy"))
        {
            Vector2 difference = (transform.position - other.transform.position).normalized;
            takeDamage(25, difference, meleeknockbackForce);
        }
    }

    public IEnumerator Knockback(Vector2 direction, float knockbackForce)
    {
        Vector2 force = direction * knockbackForce;
        isKnocking = true;
        rb.AddForce(force, ForceMode2D.Impulse);
        yield return new WaitForSeconds(knockbackDuration);
   
        isKnocking = false;
    }
}