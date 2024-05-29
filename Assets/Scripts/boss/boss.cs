using System.Collections;
using BarthaSzabolcs.Tutorial_SpriteFlash;
using UnityEngine;

public class boss : MonoBehaviour
{
    public int maxHealth = 100; // Maximum health of the boss
    public int curentHealth; // Current health of the boss
    public float rotateSpeed = 0.25f; // Rotation speed of the boss
    private float triggerDistance = 14f; // Distance at which the boss is triggered
    public float meleeDamage; // Damage dealt by the boss in melee attacks
    public float spawnRate; // Rate at which the boss spawns enemies
    private float timeToSpawn = 10f; // Time until the next spawn
    public float distanceToShoot = 5f; // Distance at which the boss shoots
    public float distanceToStop = 3f; // Distance at which the boss stops moving
    public float meleeRate; // Rate of melee attacks
    private float timeToMelee; // Time until the next melee attack
    public float skill2Rate; // Rate of skill 2 attacks
    private float timeToFire; // Time until the next skill 2 attack
    public float skill1Rate; // Rate of skill 1 attacks
    private float timeToSkill = 5f; // Time until the next skill 1 attack
    public float minDistance = 10f; // Minimum distance to move
    public float maxDistance = 30f; // Maximum distance to move
    public float moveSpeed = 5f; // Movement speed of the boss

    private bool isTriggered = false; // Whether the boss is triggered
    private bool isMoving = false; // Whether the boss is moving

    private Vector2 targetPosition; // Target position for the boss
    public player player; // Reference to the player script
    public HealthBar healthBar; // Reference to the health bar
    public Transform target; // Target transform (player)
    public Rigidbody2D rb; // Rigidbody2D component
    public GameObject bulletPrefab; // Prefab for bullets
    public GameObject skill1Prefab; // Prefab for skill 1
    public GameObject meleeEnemyPrefab; // Prefab for melee enemies
    public GameObject rangedEnemyPrefab; // Prefab for ranged enemies
    public Transform firingPoint; // Firing point transform
    public SpriteRenderer bossspriteRenderer; // SpriteRenderer for the boss
    public SimpleFlash flashEffect; // Flash effect when taking damage
    private AudioManager sound; // Reference to the AudioManager
    public AudioClip gunSound; // Sound to play when shooting
    public Transform playerTransform; // Reference to the player's transform
    public GameObject winScreenPanel; // Win screen panel

    Animator animator; // Animator component

    // Start is called before the first frame update
    private void Start()
    {
        player = FindAnyObjectByType<player>(); // Find the player in the scene
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component
        bossspriteRenderer = rb.GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component
        animator = GetComponent<Animator>(); // Get the Animator component
        sound = AudioManager.instance; // Get the AudioManager instance
        curentHealth = maxHealth; // Set current health to max health
        healthBar.setMaxHealth(maxHealth); // Set the max health on the health bar
        winScreenPanel.SetActive(false); // Hide the win screen panel
        SetRandomTargetPosition(); // Set a random target position
    }

    // Update is called once per frame
    private void Update()
    {
        if (target != null)
        {
            RotateTowardsTarget(); // Rotate towards the target

            float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position); // Calculate distance to player
            if (distanceToPlayer <= triggerDistance)
            {
                isTriggered = true; // Trigger the boss
            }
            if (isTriggered)
            {
                if (isMoving)
                {
                    transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime); // Move towards the target position

                    if ((Vector2)transform.position == targetPosition)
                    {
                        isMoving = false;
                        SetRandomTargetPosition(); // Set a new random target position
                    }
                }
                else
                {
                    if (distanceToPlayer < minDistance || distanceToPlayer > maxDistance)
                    {
                        SetRandomTargetPosition(); // Set a new random target position
                    }
                    else
                    {
                        StartCoroutine(WaitAndMove(UnityEngine.Random.Range(0, 1f))); // Wait and then move
                    }
                }
            }
        }

        if (target != null && isTriggered)
        {
            bossSkill1(); // Execute skill 1
            shoot(); // Shoot bullets
            spawn(); // Spawn enemies
        }

        if (curentHealth <= 0)
        {
            Destroy(gameObject); // Destroy the boss
            winScreenPanel.SetActive(true); // Show the win screen panel
            Time.timeScale = 0; // Pause the game
        }

        timeToMelee -= Time.deltaTime; // Decrease time to melee
    }

    // FixedUpdate is called at a fixed interval and is independent of frame rate
    private void FixedUpdate()
    {
        if (!isMoving)
        {
            animator.SetBool("isWalking", false); // Stop walking animation
        }
        else
        {
            animator.SetBool("isWalking", true); // Start walking animation
        }
    }

    // Method to spawn enemies
    private void spawn()
    {
        if (timeToSpawn <= 0f)
        {
            Instantiate(meleeEnemyPrefab, transform.position, Quaternion.identity); // Spawn melee enemies
            Instantiate(meleeEnemyPrefab, transform.position, Quaternion.identity); // Spawn melee enemies
            Instantiate(rangedEnemyPrefab, transform.position, Quaternion.identity); // Spawn ranged enemies
            timeToSpawn = spawnRate; // Reset spawn timer
        }
        else
        {
            timeToSpawn -= Time.deltaTime; // Decrease spawn timer
        }
    }

    // Method for boss skill 1
    private void bossSkill1()
    {
        if (timeToSkill <= 0f)
        {
            Instantiate(skill1Prefab, target.position, Quaternion.identity); // Instantiate skill 1 prefab
            timeToSkill = skill1Rate; // Reset skill 1 timer
        }
        else
        {
            timeToSkill -= Time.deltaTime; // Decrease skill 1 timer
        }
    }

    // Method to shoot bullets
    private void shoot()
    {
        if (timeToFire <= 0f)
        {
            sound.EnemyGunSound(gunSound); // Play gun sound
            Instantiate(bulletPrefab, firingPoint.position, firingPoint.rotation); // Instantiate bullet prefab
            timeToFire = skill2Rate; // Reset shoot timer
        }
        else
        {
            timeToFire -= Time.deltaTime; // Decrease shoot timer
        }
    }

    // Method to rotate towards the target
    private void RotateTowardsTarget()
    {
        if (target != null)
        {
            Vector2 targetDirection = target.position - transform.position; // Calculate direction to target
            bossspriteRenderer.flipX = targetDirection.x > 0; // Flip sprite based on direction
        }
    }

    // Method called when another collider enters the trigger collider
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            takeDamage(20); // Take damage from bullets
            Destroy(other.gameObject); // Destroy the bullet
        }

        isMoving = false; // Stop moving
        StartCoroutine(WaitAndMove(0.2f)); // Wait and then move
    }

    // Method called when another collider enters the collision collider
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") && timeToMelee <= 0f)
        {
            Vector2 difference = (transform.position - other.transform.position).normalized;
            player.takeMeleeDamage(25, difference, 2); // Deal melee damage to player
            timeToMelee = meleeRate; // Reset melee timer
        }

        if (other.gameObject.CompareTag("Bullet"))
        {
            return; // Ignore bullet collisions
        }
    }

    // Method to take damage
    private void takeDamage(int damage)
    {
        curentHealth -= damage; // Decrease current health
        healthBar.setHealth(curentHealth); // Update health bar
        flashEffect.Flash(); // Trigger flash effect
    }

    // Method to set a random target position
    private void SetRandomTargetPosition()
    {
        Vector2 randomDirection = UnityEngine.Random.insideUnitCircle.normalized;
        float randomDistance = UnityEngine.Random.Range(minDistance, maxDistance);
        targetPosition = (Vector2)playerTransform.position + randomDirection * randomDistance; // Calculate random target position
        isMoving = true; // Start moving
    }

    // Coroutine to wait and then move
    private IEnumerator WaitAndMove(float waitTime)
    {
        yield return new WaitForSeconds(waitTime); // Wait for specified time
        SetRandomTargetPosition(); // Set a new random target position
    }
}
