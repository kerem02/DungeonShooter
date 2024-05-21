using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class rangedEnemy : MonoBehaviour, IDamageble
{
    public Transform target;
    private float speed =4.5f;
    public Rigidbody2D rb;
    public GameObject bulletPrefab;
    private float triggerDistance = 10f;


    private float distanceToShoot = 10f;
    private float distanceToStop = 8f;

    private float fireRate = 1.2f;
    private float timeToFire = 0;
    private bool isTriggered = false;
    public Transform firingPoint;
    public SpriteRenderer renemyspriteRenderer;
    public HealthBar healthBar;

    Animator animator;

    [field: SerializeField] public float health { get; set; }


    // Start is called before the first frame update
    private void Start()
    {
        rb =GetComponent<Rigidbody2D>();
        renemyspriteRenderer = rb.GetComponent<SpriteRenderer>();
        healthBar.setMaxHealth(health);
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    private void Update()
    {
        //get the target
        if(!target){ 
            getTarget();
        }else{
            RotateTowardsTarget();
        }

        if(target != null && Vector2.Distance(target.position, transform.position) <= distanceToShoot){
            shoot();
        }

        if( health <= 0)
        {
            Destroy(gameObject);
        }
    }
    private void shoot(){
        if(timeToFire <=0f){
            Instantiate(bulletPrefab, firingPoint.position,firingPoint.rotation);
            timeToFire = fireRate;
        }else{
            timeToFire -= Time.deltaTime;
        }
    }

    private void FixedUpdate(){

        if (rb.velocity == new Vector2(0, 0))
        {
            animator.SetBool("isWalking", false);
        }
        else
            animator.SetBool("isWalking", true);

        if (target != null){
            if(Vector2.Distance(target.position, transform.position) <= triggerDistance){
            isTriggered =true;
            }
            if(Vector2.Distance(target.position, transform.position) >= distanceToStop && isTriggered){
                rb.velocity = (target.position - transform.position).normalized * speed;
            }else{
                rb.velocity = Vector2.zero;
            }
        }

    }

    private void RotateTowardsTarget(){
        Vector2 targetDirection = target.position - transform.position;
        renemyspriteRenderer.flipX = targetDirection.x > 0;

    }

    private void getTarget(){
        if(GameObject.FindGameObjectWithTag("Player")){
            target = GameObject.FindGameObjectWithTag("Player").transform;
            GetComponentInChildren<enemy_Aim>().target = target;
        }
    }

    public void takeDamage(float damage)
    {
        health -= damage;
        healthBar.setHealth(health);
    }

   
}
