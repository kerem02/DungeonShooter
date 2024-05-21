using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class enemy : MonoBehaviour, IDamageble
{
    public Transform target;
    private float speed =3f;
    public Rigidbody2D rb;
    private float triggerDistance =10f;
    private bool isTriggered = false;
    public HealthBar healthBar;
    public SpriteRenderer cenemyspriteRenderer;

    Animator animator;

    [field: SerializeField] public float health { get; set; }

    // Start is called before the first frame update
    private void Start()
    {
        rb =GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        cenemyspriteRenderer = GetComponent<SpriteRenderer>();

        healthBar.setMaxHealth(health);
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
        //rotate towards the target

        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate(){

        if (rb.velocity == new Vector2(0, 0))
        {
            animator.SetBool("isWalking", false);
        }
        else
            animator.SetBool("isWalking", true);

        //move forward

        if (target != null){
            if(Vector2.Distance(target.position, transform.position) <= triggerDistance){
                isTriggered =true;
            }
            if(isTriggered){
                rb.velocity = (target.position - transform.position).normalized *speed;
            }
        }
    }

    private void RotateTowardsTarget(){
        Vector2 targetDirection = target.position - transform.position;
        cenemyspriteRenderer.flipX = targetDirection.x > 0;

    }

    private void getTarget(){
        if(GameObject.FindGameObjectWithTag("Player")){
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }
    


    public void takeDamage(float damage)
    {   
        health -= damage;
        healthBar.setHealth(health);
    }
}
