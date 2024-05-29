using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
    [Range(1,10)]
    [SerializeField] private float speed = 10f;

    [Range(1,10)]
    [SerializeField] private float lifeTime =3f;

    [SerializeField] private float knockbackForce = 1;

    private Rigidbody2D rb;
    public GameObject hitEffect;

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject,lifeTime);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        rb.velocity = transform.up *speed;
    }
    private void OnTriggerEnter2D (Collider2D other){

        if (other.TryGetComponent(out IDamageble damageble))
        {
            Instantiate(hitEffect, transform.position, transform.rotation);

            Vector2 direction = (other.transform.position - transform.position).normalized;
            damageble.takeDamage(10, direction, knockbackForce);
            Destroy(gameObject);

        }


        if (!other.gameObject.CompareTag("EnemyBullet"))
        {
            Destroy(gameObject);
        }
  
    }
    
}
