using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossSkill2 : MonoBehaviour
{
    private float speed = 10f;
    private float lifeTime =10;
    [SerializeField] private float knockbackForce = 1;

    private Rigidbody2D rb;
    public GameObject enemyBullet;
    public GameObject hitEffect;

    // Start is called before the first frame update

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        rb.velocity = transform.right * -1 * speed;
        Destroy(gameObject,lifeTime);

    }
    
    private void OnTriggerEnter2D (Collider2D other){

        if (other.TryGetComponent(out IDamageble damageble))
        {
            Instantiate(hitEffect, transform.position, transform.rotation);

            Vector2 direction = (other.transform.position - transform.position).normalized;
            damageble.takeDamage(20, direction, knockbackForce);
            Destroy(gameObject);

        }
        if (other.gameObject.CompareTag("Boss")){
            return;
        }
        
        if(other.gameObject.CompareTag("Bullet")||other.gameObject.CompareTag("EnemyBullet")){
            Destroy(other.gameObject);
            return;
        }

        for(int rot =0; rot <=360; rot +=30 ){
            Instantiate(enemyBullet,transform.position,Quaternion.Euler(0,0,rot));
        }
        Destroy(gameObject);
    }
}
