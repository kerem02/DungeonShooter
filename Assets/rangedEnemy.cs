using BarthaSzabolcs.Tutorial_SpriteFlash;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class rangedEnemy : MonoBehaviour, IDamageble
{
    public Transform target;
    public Rigidbody2D rb;
    public GameObject bulletPrefab;
    public Transform firingPoint;
    public SpriteRenderer renemyspriteRenderer;
    public HealthBar healthBar;

    private float speed =4.5f;
    private float triggerDistance = 10f;
    private float distanceToShoot = 10f;
    private float distanceToStop = 9f;
    private float fireRate = 1.2f;
    private float timeToFire = 0;
    private float smoothingSpeed = 8f;

    private bool isTriggered = false;

    Animator animator;

    [field: SerializeField] public float health { get; set; }
    [field: SerializeField] public bool isKnocking { get; set; }
    [field: SerializeField] public float knockbackDuration { get; set; }
    [field: SerializeField] public SimpleFlash flashEffect { get; set; }


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

        if (rb.velocity.magnitude <= 0.5f)
        {
            animator.SetBool("isWalking", false);
        }
        else
            animator.SetBool("isWalking", true);

        if (target != null){
            if(Vector2.Distance(target.position, transform.position) <= triggerDistance){
            isTriggered =true;
            }
            if(Vector2.Distance(target.position, transform.position) >= distanceToStop && isTriggered && !isKnocking){
                rb.velocity = (target.position - transform.position).normalized * speed;
            }else{
                rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, smoothingSpeed * Time.deltaTime);
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

    public void takeDamage(float damage, Vector2 direction, float knockbackForce)
    {
        health -= damage;
        healthBar.setHealth(health);
        flashEffect.Flash();
        StartCoroutine(Knockback(direction, knockbackForce));
    }

    public IEnumerator Knockback(Vector2 direction, float knockbackForce)
    {
        Vector2 force = direction * knockbackForce;
        isKnocking = true;

        rb.AddForce(force, ForceMode2D.Impulse);
        yield return new WaitForSeconds(knockbackDuration);

        isKnocking = false;
    }


}
