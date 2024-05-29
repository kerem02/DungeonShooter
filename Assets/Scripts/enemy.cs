using BarthaSzabolcs.Tutorial_SpriteFlash;
using System.Collections;
using UnityEngine;

public class enemy : MonoBehaviour, IDamageble
{
    public Transform target; // Target for the enemy
    public Rigidbody2D rb; // Rigidbody2D component of the enemy

    private float speed = 4.5f; // Movement speed of the enemy
    private float triggerDistance = 14f; // Distance at which the enemy is triggered

    private bool isTriggered = false; // Whether the enemy is triggered to follow the target

    public HealthBar healthBar; // Health bar for the enemy
    public SpriteRenderer cenemyspriteRenderer; // SpriteRenderer of the enemy
    public GameObject goldPrefab; // Prefab for the gold dropped by the enemy
    public AudioClip bulletHitSound; // Sound to play when the enemy is hit by a bullet
    private AudioManager sound; // Reference to the AudioManager

    Animator animator; // Animator component for the enemy

    [field: SerializeField] public float health { get; set; } // Health of the enemy
    [field: SerializeField] public bool isKnocking { get; set; } // Whether the enemy is being knocked back
    [field: SerializeField] public float knockbackDuration { get; set; } // Duration of knockback
    [field: SerializeField] public SimpleFlash flashEffect { get; set; } // Flash effect when taking damage

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component
        animator = GetComponent<Animator>(); // Get the Animator component
        cenemyspriteRenderer = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component
        sound = AudioManager.instance; // Get the instance of the AudioManager

        healthBar.setMaxHealth(health); // Set the max health on the health bar
    }

    // Update is called once per frame
    private void Update()
    {
        // Get the target
        if (!target)
        {
            getTarget();
        }
        else
        {
            RotateTowardsTarget(); // Rotate towards the target
        }

        // Check if health is zero or less
        if (health <= 0)
        {
            Die();
        }
    }

    // FixedUpdate is called at a fixed interval and is independent of frame rate
    private void FixedUpdate()
    {
        // Update walking animation
        if (rb.velocity == Vector2.zero)
        {
            animator.SetBool("isWalking", false);
        }
        else
        {
            animator.SetBool("isWalking", true);
        }

        // Move forward
        if (target != null)
        {
            if (Vector2.Distance(target.position, transform.position) <= triggerDistance)
            {
                isTriggered = true;
            }
            if (isTriggered && !isKnocking)
            {
                rb.velocity = (target.position - transform.position).normalized * speed; // Move towards target
            }
        }
    }

    // Rotate the enemy towards the target
    private void RotateTowardsTarget()
    {
        Vector2 targetDirection = target.position - transform.position;
        cenemyspriteRenderer.flipX = targetDirection.x > 0;
    }

    // Get the target (player)
    private void getTarget()
    {
        if (GameObject.FindGameObjectWithTag("Player"))
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    // Method to handle taking damage
    public void takeDamage(float damage, Vector2 direction, float knockbackForce)
    {
        health -= damage; // Decrease health
        healthBar.setHealth(health); // Update health bar
        flashEffect.Flash(); // Trigger flash effect
        sound.BulletHitSound(bulletHitSound); // Play bullet hit sound
        StartCoroutine(Knockback(direction, knockbackForce)); // Apply knockback
    }

    // Coroutine to handle knockback
    public IEnumerator Knockback(Vector2 direction, float knockbackForce)
    {
        Vector2 force = direction * knockbackForce;
        isKnocking = true;
        rb.AddForce(force, ForceMode2D.Impulse); // Apply knockback force
        yield return new WaitForSeconds(knockbackDuration); // Wait for knockback duration

        isKnocking = false;
    }

    // Method to drop gold on death
    private void DropGold()
    {
        Instantiate(goldPrefab, transform.position, Quaternion.identity); // Instantiate gold prefab
    }

    // Method to handle death
    private void Die()
    {
        DropGold(); // Drop gold
        Destroy(gameObject); // Destroy the enemy
    }
}

