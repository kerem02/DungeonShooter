using BarthaSzabolcs.Tutorial_SpriteFlash;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class player : MonoBehaviour, IDamageble
{
    public float moveSpeed = 5f; // Player movement speed

    public Rigidbody2D rb; // Rigidbody2D component for the player
    public Camera cam; // Main camera reference
    public SpriteRenderer charspriteRenderer; // SpriteRenderer for the player character
    [SerializeField] private TrailRenderer tr; // TrailRenderer for dash effect

    private float dashSpeed = 10f; // Speed during dash
    private float dashTime = 0.2f; // Duration of dash
    private float nextDashTime = 0f; // Time until the next dash is available
    private float dashCooldown = 1f; // Cooldown time for dash
    private float meleeknockbackForce = 10f; // Knockback force for melee attacks

    public int gold = 0; // Player's gold count
    public int healthPotionCount = 0; // Count of health potions the player has
    private int healAmount = 50; // Amount of health restored by a potion
    [SerializeField] private int maxHealth; // Maximum health of the player

    private bool isDashing = false; // Whether the player is currently dashing

    public TextMeshProUGUI potionCountText; // UI text for potion count
    public TextMeshProUGUI goldCountText; // UI text for gold count

    public HealthBar healthBar; // Health bar UI component
    public GameObject hitEffect; // Effect to instantiate when the player is hit
    public GameObject deadScreen; // UI panel to show when the player dies
    private AudioManager sound; // Reference to the AudioManager
    public AudioClip knifeHitSound; // Sound to play when hit by a knife
    public AudioClip bulletHitSound; // Sound to play when hit by a bullet

    Vector2 movement; // Player's movement vector
    Vector2 mousePos; // Mouse position

    Animator animator; // Animator component for the player

    [field: SerializeField] public float health { get; set; } // Current health of the player
    [field: SerializeField] public bool isKnocking { get; set; } // Whether the player is being knocked back
    [field: SerializeField] public float knockbackDuration { get; set; } // Duration of knockback
    [field: SerializeField] public SimpleFlash flashEffect { get; set; } // Flash effect when taking damage

    public GameObject[] weaponObjects; // Array of weapon objects
    public Weapon[] allweapons; // Array of all available weapons
    private List<Weapon> ownedWeapons = new List<Weapon>(); // List of owned weapons
    private int currentWeaponIndex = 0; // Index of the currently equipped weapon
    private Weapon currentWeapon; // Currently equipped weapon

    private void Awake()
    {
        animator = GetComponent<Animator>(); // Get the Animator component
        charspriteRenderer = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component
        sound = AudioManager.instance; // Get the AudioManager instance
        health = maxHealth; // Set the player's health to max health
        healthBar.setMaxHealth(health); // Initialize the health bar
        deadScreen.SetActive(false); // Hide the dead screen UI
        UpdatePotionUI(); // Update the potion UI
        UpdateGoldUI(); // Update the gold UI

        // Deactivate all weapon prefabs
        foreach (var weapon in allweapons)
        {
            weapon.weaponPrefab.SetActive(false);
        }

        // Add the first weapon to the owned weapons list and equip it
        ownedWeapons.Add(allweapons[0]);
        EquipWeapon(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= nextDashTime)
        {
            StartCoroutine(Dash()); // Start the dash coroutine
            nextDashTime = Time.time + dashCooldown; // Set the next dash time
        }
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        mousePos = cam.ScreenToWorldPoint(Input.mousePosition); // Get the mouse position in world space

        if (Input.GetKeyDown(KeyCode.E) && health < maxHealth && healthPotionCount > 0)
        {
            Heal(healAmount); // Heal the player if conditions are met
        }

        if (health <= 0)
        {
            Die(); // Handle player death if health is zero or less
        }
    }

    // FixedUpdate is called at a fixed interval and is independent of frame rate
    void FixedUpdate()
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

        if (!isDashing && !isKnocking)
        {
            handleMovement(); // Handle player movement
        }

        UpdateDirection(); // Update the player's facing direction
    }

    // Handle player movement
    void handleMovement()
    {
        rb.velocity = movement.normalized * moveSpeed; // Set the player's velocity
    }

    // Update the player's facing direction based on the mouse position
    void UpdateDirection()
    {
        Vector2 lookDir = (mousePos - rb.position).normalized;
        charspriteRenderer.flipX = lookDir.x > 0;
    }

    // Coroutine for dashing
    IEnumerator Dash()
    {
        Vector2 dashDirection = movement.normalized;
        float startTime = Time.time;

        isDashing = true;
        tr.emitting = true;
        while (Time.time < startTime + dashTime)
        {
            rb.velocity = dashDirection * dashSpeed; // Set the dash velocity
            yield return null;
        }
        tr.emitting = false;
        isDashing = false;
    }

    // Method to handle taking damage
    public void takeDamage(float damage, Vector2 direction, float knockbackForce)
    {
        Instantiate(hitEffect, transform.position, transform.rotation); // Instantiate the hit effect
        health -= damage; // Decrease health
        healthBar.setHealth(health); // Update health bar
        flashEffect.Flash(); // Trigger flash effect
        StartCoroutine(Knockback(direction, knockbackForce)); // Apply knockback
        sound.BulletHitSound(bulletHitSound); // Play bullet hit sound
    }

    // Method to handle taking melee damage
    public void takeMeleeDamage(float damage, Vector2 direction, float knockbackForce)
    {
        Instantiate(hitEffect, transform.position, transform.rotation); // Instantiate the hit effect
        health -= damage; // Decrease health
        healthBar.setHealth(health); // Update health bar
        flashEffect.Flash(); // Trigger flash effect
        StartCoroutine(Knockback(direction, knockbackForce)); // Apply knockback
        sound.KnifeHitSound(knifeHitSound); // Play knife hit sound
    }

    // Method to handle collisions with other objects
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("MeleeEnemy"))
        {
            Vector2 difference = (transform.position - other.transform.position).normalized;
            takeMeleeDamage(25, difference, meleeknockbackForce); // Take melee damage if colliding with a melee enemy
        }
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

    // Method to add gold to the player
    public void AddGold(int amount)
    {
        gold += amount;
        UpdateGoldUI(); // Update gold UI
        Debug.Log("Gold : " + gold);
    }

    // Method to add potions to the player
    public void AddPotion(int count)
    {
        healthPotionCount += count;
        UpdatePotionUI(); // Update potion UI
    }

    // Method to update the potion UI
    private void UpdatePotionUI()
    {
        potionCountText.text = "x" + healthPotionCount;
    }

    // Method to heal the player
    public void Heal(int amount)
    {
        health += amount;
        if (health >= maxHealth)
        {
            health = maxHealth;
        }
        healthPotionCount--;
        UpdatePotionUI(); // Update potion UI
        healthBar.setHealth(health); // Update health bar
    }

    // Method to update the gold UI
    public void UpdateGoldUI()
    {
        goldCountText.text = "x" + gold;
    }

    // Method to show the dead screen
    public void ShowDeadPanel()
    {
        deadScreen.SetActive(true);
    }

    // Method to handle player death
    private void Die()
    {
        Debug.Log("You are dead");
        ShowDeadPanel(); // Show the dead screen UI
        gameObject.SetActive(false); // Deactivate the player
    }

    // Method to restart the game
    public void TryAgain()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reload the current scene
    }

    // Method to quit the game
    public void QuitGame()
    {
        Application.Quit(); // Quit the application
    }

    // Method to equip a weapon by index
    public void EquipWeapon(int index)
    {
        if (index >= 0 && index < allweapons.Length)
        {
            foreach (var weaponObject in weaponObjects)
            {
                weaponObject.SetActive(false); // Deactivate all weapon objects
            }
            currentWeaponIndex = index;
            weaponObjects[index].SetActive(true); // Activate the selected weapon
            currentWeapon = allweapons[index]; // Set the current weapon
        }
    }

    // Method to equip a weapon by type
    public void EquipWeaponByType(WeaponType weaponType)
    {
        for (int i = 0; i < allweapons.Length; i++)
        {
            if (allweapons[i].weaponType == weaponType)
            {
                EquipWeapon(i);
                break;
            }
        }
    }

    // Method to buy a weapon
    public void BuyWeapon(int index)
    {
        if (index >= 0 && index < allweapons.Length && !ownedWeapons.Contains(allweapons[index]))
        {
            ownedWeapons.Add(allweapons[index]);
        }
    }

    // Method to get the current weapon
    public Weapon GetCurrentWeapon()
    {
        return currentWeapon;
    }
}
