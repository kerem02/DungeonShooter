using UnityEngine;

public class BossSkill2 : MonoBehaviour
{
    private float speed = 10f; // Speed of the skill projectile
    private float lifeTime = 10; // Lifetime of the skill before it gets destroyed
    [SerializeField] private float knockbackForce = 1; // Knockback force applied by the skill

    private Rigidbody2D rb; // Rigidbody2D component of the skill
    public GameObject enemyBullet; // Prefab for enemy bullets spawned on impact
    public GameObject hitEffect; // Effect to instantiate on impact

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component
    }

    // FixedUpdate is called at a fixed interval and is independent of frame rate
    private void FixedUpdate()
    {
        rb.velocity = transform.right * -1 * speed; // Set the velocity of the skill projectile
        Destroy(gameObject, lifeTime); // Destroy the skill projectile after its lifetime
    }

    // Method called when the skill projectile collides with another object
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out IDamageble damageble))
        {
            Instantiate(hitEffect, transform.position, transform.rotation); // Instantiate the hit effect

            Vector2 direction = (other.transform.position - transform.position).normalized; // Calculate knockback direction
            damageble.takeDamage(20, direction, knockbackForce); // Apply damage and knockback
            Destroy(gameObject); // Destroy the skill projectile
        }

        if (other.gameObject.CompareTag("Boss"))
        {
            return; // Ignore collisions with the boss
        }

        if (other.gameObject.CompareTag("Bullet") || other.gameObject.CompareTag("EnemyBullet"))
        {
            Destroy(other.gameObject); // Destroy the other bullet
            return; // Exit the method
        }

        // Spawn enemy bullets in a circular pattern
        for (int rot = 0; rot <= 360; rot += 30)
        {
            Instantiate(enemyBullet, transform.position, Quaternion.Euler(0, 0, rot));
        }
        Destroy(gameObject); // Destroy the skill projectile
    }
}

