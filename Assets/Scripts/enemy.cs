using BarthaSzabolcs.Tutorial_SpriteFlash;
using System.Collections;
using UnityEngine;

public class enemy : MonoBehaviour, IDamageble
{
    public Transform target;
    public Rigidbody2D rb;

    private float speed =3f;
    private float triggerDistance =10f;

    private bool isTriggered = false;

    public HealthBar healthBar;
    public SpriteRenderer cenemyspriteRenderer;
    public GameObject goldPrefab;
    public AudioClip bulletHitSound;
    private AudioManager sound;

    Animator animator;

    [field: SerializeField] public float health { get; set; }
    [field: SerializeField] public bool isKnocking { get; set; }
    [field: SerializeField] public float knockbackDuration { get; set; }
    [field: SerializeField] public SimpleFlash flashEffect { get; set; }

    // Start is called before the first frame update
    private void Start()
    {
        rb =GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        cenemyspriteRenderer = GetComponent<SpriteRenderer>();
        sound = AudioManager.instance;

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
            Die();
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
            if(isTriggered && !isKnocking){
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
    


    public void takeDamage(float damage, Vector2 direction, float knockbackForce)
    {   
        health -= damage;
        healthBar.setHealth(health);
        flashEffect.Flash();
        sound.BulletHitSound(bulletHitSound);
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

    private void DropGold()
    {
        Instantiate(goldPrefab, transform.position, Quaternion.identity);
    }

   private void Die()
    {
        DropGold();
        Destroy(gameObject);
    }
}
