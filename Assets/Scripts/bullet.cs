
using UnityEngine;

public class bullet : MonoBehaviour
{
    [Range(1,60)]
    [SerializeField] private float speed = 25f;

    [Range(1,60)]
    [SerializeField] private float lifeTime =1f;

    private Rigidbody2D rb;

    public GameObject hitEffect;
    public GameObject wallHitEffect;
    private AudioManager sound;
    public AudioClip bulletMapCollideSound;
    public AudioClip bulletHitSound;

    [SerializeField] private float knockbackForce = 1f;

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject,lifeTime);

        sound = AudioManager.instance;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        rb.velocity = transform.right * speed * -1;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IDamageble damageble))
        {
            Instantiate(hitEffect, transform.position, transform.rotation);

            Vector2 direction = (collision.transform.position - transform.position).normalized;
            damageble.takeDamage(20,direction, knockbackForce);
            Destroy(gameObject);

        }

        else if (collision.gameObject.CompareTag("Boss"))
        {
            Instantiate(wallHitEffect, transform.position, transform.rotation);
            sound.BulletHitSound(bulletHitSound);
            Destroy(gameObject);
        }
        else if (!collision.gameObject.CompareTag("Bullet"))
        {
            Instantiate(wallHitEffect, transform.position, transform.rotation);
            sound.BulletMapHitSound(bulletMapCollideSound);
            Destroy(gameObject);
        }


    }

}
