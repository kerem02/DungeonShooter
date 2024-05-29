using BarthaSzabolcs.Tutorial_SpriteFlash;
using System.Collections;
using UnityEngine;

public class rangedEnemy : MonoBehaviour, IDamageble
{
    public Transform target; // Target for the enemy
    public Rigidbody2D rb; // Rigidbody2D component of the enemy
    public GameObject bulletPrefab; // Prefab for the enemy's bullets
    public Transform firingPoint; // Point from which the enemy fires bullets
    public SpriteRenderer renemyspriteRenderer; // SpriteRenderer of the enemy
    public HealthBar healthBar; // Health bar for the enemy
    public GameObject goldPrefab; // Prefab for the gold dropped by the enemy
    private AudioManager sound; // Reference to the AudioManager
    public AudioClip bulletHitSound; // Sound to play when bullet hits
    public AudioClip gunSound; // Sound to play when firing gun

    [SerializeField] private float fireRate = 1.2f; // Rate of fire for the enemy
    [SerializeField] private int goldDropAmount; // Amount of gold dropped on death
    private float speed = 4.5f; // Movement speed of the enemy
    private float triggerDistance = 14f; // Distance at which the enemy is triggered
    private float distanceToShoot = 8f; // Distance within which the enemy shoots
    private float distanceToStop = 7f; // Distance within which the enemy stops moving
    private float timeToFire = 0; // Time until the next shot can be fired
    private float smoothingSpeed = 8f; // Smoothing speed for stopping movement

    private bool isTriggered = false; // Whether the enemy is triggered to follow the target

    Animator animator; // Animator component for the enemy

    [field: SerializeField] public float health { get; set; } // Health of the enemy
    [field: SerializeField] public bool isKnocking { get; set; } // Whether the enemy is being knocked back
    [field: SerializeField] public float knockbackDuration { get; set; } // Duration of knockback
    [field: SerializeField] public SimpleFlash flashEffect { get; set; } // Flash effect when taking damage

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component
        renemyspriteRenderer = rb.GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component
        healthBar.setMaxHealth(health); // Set the max health on the health bar
        animator = GetComponent<Animator>(); // Get the Animator component
        sound = AudioManager.instance; // Get the instance of the AudioManager
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
            RotateTowardsTarget();
        }

        // Shoot if within shooting distance
        if (target != null && Vector2.Distance(target.position, transform.position) <= distanceToShoot)
        {
            shoot();
        }

        // Check if health is zero or less
        if (health <= 0)
        {
            Die();
        }
    }

    // Method to handle shooting
    private void shoot()
    {
        if (timeToFire <= 0f)
        {
            Instantiate(bulletPrefab, firingPoint.position, firingPoint.rotation); // Instantiate a bullet
            sound.EnemyGunSound(gunSound); // Play gun sound
            timeToFire = fireRate; // Reset time to fire
        }
        else
        {
            timeToFire -= Time.deltaTime; // Decrease time to fire
        }
    }

    // FixedUpdate is called at a fixed interval and is independent of frame rate
    private void FixedUpdate()
    {
        // Update walking animation
        if (rb.velocity.magnitude <= 0.5f)
        {
            animator.SetBool("isWalking", false);
        }
        else
        {
            animator.SetBool("isWalking", true);
        }

        // Handle movement
        if (target != null)
        {
            if (Vector2.Distance(target.position, transform.position) <= triggerDistance)
            {
                isTriggered = true;
            }
            if (Vector2.Distance(target.position, transform.position) >= distanceToStop && isTriggered && !isKnocking)
            {
                rb.velocity = (target.position - transform.position).normalized * speed; // Move towards target
            }
            else
            {
                rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, smoothingSpeed * Time.deltaTime); // Smoothly stop movement
            }
        }
    }

    // Rotate the enemy towards the target
    private void RotateTowardsTarget()
    {
        Vector2 targetDirection = target.position - transform.position;
        renemyspriteRenderer.flipX = targetDirection.x > 0;
    }

    // Get the target (player)
    private void getTarget()
    {
        if (GameObject.FindGameObjectWithTag("Player"))
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
            GetComponentInChildren<enemy_Aim>().target = target;
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
        for (int i = 0; i < goldDropAmount; i++)
        {
            Instantiate(goldPrefab, transform.position, Quaternion.identity); // Instantiate gold prefab
        }
    }

    // Method to handle death
    private void Die()
    {
        DropGold(); // Drop gold
        Destroy(gameObject); // Destroy the enemy
    }
}

