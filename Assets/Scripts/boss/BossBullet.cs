using UnityEngine;

public class BossBullet : MonoBehaviour
{
    [Range(1, 10)]
    [SerializeField] private float speed = 10f; // Speed of the bullet

    [Range(1, 10)]
    [SerializeField] private float lifeTime = 3f; // Lifetime of the bullet before it gets destroyed

    [SerializeField] private float knockbackForce = 1; // Knockback force applied by the bullet

    private Rigidbody2D rb; // Rigidbody2D component of the bullet
    public GameObject hitEffect; // Effect to instantiate when the bullet hits something

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component
        Destroy(gameObject, lifeTime); // Destroy the bullet after its lifetime
    }

    // FixedUpdate is called at a fixed interval and is independent of frame rate
    private void FixedUpdate()
    {
        rb.velocity = transform.up * speed; // Set the velocity of the bullet
    }

    // Method called when the bullet collides with another object
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out IDamageble damageble))
        {
            Instantiate(hitEffect, transform.position, transform.rotation); // Instantiate the hit effect

            Vector2 direction = (other.transform.position - transform.position).normalized; // Calculate knockback direction
            damageble.takeDamage(10, direction, knockbackForce); // Apply damage and knockback
            Destroy(gameObject); // Destroy the bullet
        }

        if (!other.gameObject.CompareTag("EnemyBullet"))
        {
            Destroy(gameObject); // Destroy the bullet if it hits something other than another enemy bullet
        }
    }
}
