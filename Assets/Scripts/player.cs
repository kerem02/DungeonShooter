using BarthaSzabolcs.Tutorial_SpriteFlash;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


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

    public int gold = 0;
    public int healthPotionCount = 0;
    private int healAmount = 50;
    [SerializeField] private int maxHealth;

    private bool isDashing = false;

    public TextMeshProUGUI potionCountText;
    public TextMeshProUGUI goldCountText;

    public HealthBar healthBar;
    public GameObject hitEffect;
    public GameObject deadScreen;
    private AudioManager sound;
    public AudioClip knifeHitSound;
    public AudioClip bulletHitSound;

    Vector2 movement;
    Vector2 mousePos;

    Animator animator;

    [field: SerializeField] public float health { get; set; }
    [field: SerializeField] public bool isKnocking { get; set; }
    [field: SerializeField] public float knockbackDuration { get; set; }
    [field: SerializeField] public SimpleFlash flashEffect { get ; set; }

    public GameObject[] weaponObjects;
    public Weapon[] allweapons;
    private List<Weapon> ownedWeapons = new List<Weapon>();
    private int currentWeaponIndex = 0;
    private Weapon currentWeapon;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        charspriteRenderer = GetComponent<SpriteRenderer>();
        sound = AudioManager.instance;
        healthBar.setMaxHealth(health);
        deadScreen.SetActive(false);
        UpdatePotionUI();
        UpdateGoldUI();

        foreach (var weapon in allweapons)
        {
            weapon.weaponPrefab.SetActive(false);
        }

        ownedWeapons.Add(allweapons[0]);
        EquipWeapon(0);

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

        if (Input.GetKeyDown(KeyCode.E) && health < maxHealth && healthPotionCount > 0)
        {
            Heal(healAmount);
        }

        if(health <= 0)
        {
            Die();
        }
    
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
        sound.BulletHitSound(bulletHitSound);
    }

    public void takeMeleeDamage(float damage, Vector2 direction, float knockbackForce)
    {
        Instantiate(hitEffect, transform.position, transform.rotation);
        health -= damage;
        healthBar.setHealth(health);
        flashEffect.Flash();
        StartCoroutine(Knockback(direction, knockbackForce));
        sound.KnifeHitSound(knifeHitSound);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("MeleeEnemy"))
        {
            Vector2 difference = (transform.position - other.transform.position).normalized;
            takeMeleeDamage(25, difference, meleeknockbackForce);
            
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

    public void AddGold(int amount)
    {
        gold += amount;
        UpdateGoldUI();
        Debug.Log("Gold : " + gold);
    }

    public void AddPotion(int count)
    {
        healthPotionCount += count;
        UpdatePotionUI();
    }

    private void UpdatePotionUI()
    {
        potionCountText.text = "x" + healthPotionCount;
    }

    public void Heal(int amount)
    {
        health += amount;
        if(health >= 100)
        {
            health = 100;
        }
        healthPotionCount--;
        UpdatePotionUI();
        healthBar.setHealth(health);

    }

    public void UpdateGoldUI()
    {
        goldCountText.text = "x" + gold;
    }

    public void ShowDeadPanel()
    {
        deadScreen.SetActive(true);
    }

    private void Die()
    {
        Debug.Log("You are dead");
        ShowDeadPanel();
        gameObject.SetActive(false);
    }

    public void TryAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void EquipWeapon(int index)
    {
        if(index >= 0 && index < allweapons.Length)
        {
            foreach(var weaponObject in weaponObjects)
            {
                weaponObject.SetActive(false);
            }
            currentWeaponIndex = index;
            weaponObjects[index].SetActive(true);
            currentWeapon = allweapons[index];
        }
    }

    public void EquipWeaponByType(WeaponType weaponType)
    {
        for(int i = 0; i < allweapons.Length; i++)
        {
            if (allweapons[i].weaponType == weaponType)
            {
                EquipWeapon(i);
                break;
            }
        }
    }

    public void BuyWeapon(int index)
    {
        if(index >= 0 && index < allweapons.Length && !ownedWeapons.Contains(allweapons[index]))
        {
            ownedWeapons.Add(allweapons[index]);
        }
    }

    public Weapon GetCurrentWeapon()
    {
        return currentWeapon;
    }

}