using UnityEngine;

public class bullet : MonoBehaviour
{
    [Range(1, 60)]
    [SerializeField] private float speed = 25f; // Speed of the bullet

    [Range(1, 60)]
    [SerializeField] private float lifeTime = 1f; // Lifetime of the bullet before it gets destroyed

    private Rigidbody2D rb; // Rigidbody2D component of the bullet

    public GameObject hitEffect; // Effect when bullet hits a target
    public GameObject wallHitEffect; // Effect when bullet hits a wall
    private AudioManager sound; // Reference to the AudioManager
    public AudioClip bulletMapCollideSound; // Sound when bullet collides with the map
    public AudioClip bulletHitSound; // Sound when bullet hits a target

    [SerializeField] private float knockbackForce = 1f; // Knockback force applied to the target

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component
        Destroy(gameObject, lifeTime); // Destroy the bullet after its lifetime

        sound = AudioManager.instance; // Get the instance of the AudioManager
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        rb.velocity = transform.right * speed * -1; // Set the velocity of the bullet
    }

    // Method called when the bullet collides with another object
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IDamageble damageble))
        {
            // Instantiate the hit effect
            Instantiate(hitEffect, transform.position, transform.rotation);

            // Calculate knockback direction
            Vector2 direction = (collision.transform.position - transform.position).normalized;
            damageble.takeDamage(20, direction, knockbackForce); // Apply damage and knockback
            Destroy(gameObject); // Destroy the bullet
        }
        else if (collision.gameObject.CompareTag("Boss"))
        {
            // Instantiate the wall hit effect
            Instantiate(wallHitEffect, transform.position, transform.rotation);
            sound.BulletHitSound(bulletHitSound); // Play bullet hit sound
            Destroy(gameObject); // Destroy the bullet
        }
        else if (!collision.gameObject.CompareTag("Bullet"))
        {
            // Instantiate the wall hit effect
            Instantiate(wallHitEffect, transform.position, transform.rotation);
            sound.BulletMapHitSound(bulletMapCollideSound); // Play bullet map collide sound
            Destroy(gameObject); // Destroy the bullet
        }
    }
}
