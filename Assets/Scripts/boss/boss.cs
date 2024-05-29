
using System.Collections;
using BarthaSzabolcs.Tutorial_SpriteFlash;
using UnityEngine;

public class boss : MonoBehaviour
{

    public int maxHealth = 100;
    public int curentHealth;
    public float rotateSpeed = 0.25f;
    private float triggerDistance = 14f;
    public float meleeDamage;
    public float spawnRate;
    private float timeToSpawn=10f;
    public float distanceToShoot = 5f;
    public float distanceToStop = 3f;
    public float meleeRate;
    private float timeToMelee;
    public float skill2Rate;
    private float timeToFire;
    public float skill1Rate ;
    private float timeToSkill =5f;
    public float minDistance = 10f;
    public float maxDistance = 30f;
    public float moveSpeed = 5f;

    private bool isTriggered = false;
    private bool isMoving = false;

    private Vector2 targetPosition;
    public player player;
    public HealthBar healthBar;
    public Transform target;
    public Rigidbody2D rb;
    public GameObject bulletPrefab;
    public GameObject skill1Prefab;
    public GameObject meleeEnemyPrefab;
    public GameObject rangedEnemyPrefab;
    public Transform firingPoint;
    public SpriteRenderer bossspriteRenderer;
    public SimpleFlash flashEffect;
    private AudioManager sound;
    public AudioClip gunSound;
    public Transform playerTransform;
    public GameObject winScreenPanel;

    Animator animator;
    // Start is called before the first frame update
    private void Start()
    {
        player = FindAnyObjectByType<player>();
        rb =GetComponent<Rigidbody2D>();
        bossspriteRenderer = rb.GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        sound = AudioManager.instance;
        curentHealth =maxHealth;
        healthBar.setMaxHealth(maxHealth);
        winScreenPanel.SetActive(false);
        SetRandomTargetPosition();
    }

    // Update is called once per frame
    private void Update()
    {
        if(target != null){

            RotateTowardsTarget();
            
            float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
            if(distanceToPlayer <= triggerDistance) 
            {
                isTriggered = true;
            }
            if(isTriggered == true) 
            { 

            if (isMoving)
            {
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

                if ((Vector2)transform.position == targetPosition)
                {
                    isMoving = false;
                    SetRandomTargetPosition();
                }
            }
            else
            {
                
                if (distanceToPlayer < minDistance || distanceToPlayer > maxDistance)
                {
                    SetRandomTargetPosition();
                }
                else
                {
                    
                    StartCoroutine(WaitAndMove(UnityEngine.Random.Range(0, 1f)));
                }
            }
            }
        }

        if(target != null && isTriggered == true){
            bossSkill1();
            shoot();
            spawn();
        }
        
        
        
        if(curentHealth <= 0){
            Destroy(gameObject);
            winScreenPanel.SetActive(true);
            Time.timeScale = 0;
        }
        timeToMelee -= Time.deltaTime;

    }

    private void FixedUpdate()
    {
        if (isMoving == false)
        {
            animator.SetBool("isWalking", false);
        }
        else
            animator.SetBool("isWalking", true);
    }
    private void spawn(){
        if(timeToSpawn <=0f){
            Instantiate(meleeEnemyPrefab, transform.position,Quaternion.identity);
            Instantiate(meleeEnemyPrefab, transform.position,Quaternion.identity);
            Instantiate(rangedEnemyPrefab, transform.position,Quaternion.identity);
            timeToSpawn = spawnRate;
        }else{
            timeToSpawn -= Time.deltaTime;
        }
    }
    private void bossSkill1(){
        if(timeToSkill <=0f){
            Instantiate(skill1Prefab, target.position,Quaternion.identity);
            timeToSkill = skill1Rate;
        }else{
            timeToSkill -= Time.deltaTime;
        }
    }
    private void shoot(){
        if(timeToFire <=0f){
            sound.EnemyGunSound(gunSound);
            Instantiate(bulletPrefab, firingPoint.position,firingPoint.rotation);
            timeToFire = skill2Rate;
        }else{
            timeToFire -= Time.deltaTime;
        }
    }

    private void RotateTowardsTarget(){
        if(target!=null){
            Vector2 targetDirection = target.position - transform.position;
            bossspriteRenderer.flipX = targetDirection.x > 0;
        }
    }

    
    private void OnTriggerEnter2D(Collider2D other){

        if(other.gameObject.CompareTag("Bullet")){
            takeDamage(20);
            Destroy(other.gameObject);

        }

        isMoving = false;
        StartCoroutine(WaitAndMove(0.2f));

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") && timeToMelee <= 0f)
        {
            Vector2 difference = (transform.position - other.transform.position).normalized;
            player.takeMeleeDamage(25, difference, 2);
            timeToMelee = meleeRate;

        }

        if (other.gameObject.CompareTag("Bullet"))
        {
            return;
        }
    }
    private void takeDamage(int damage){
        curentHealth -= damage;
        healthBar.setHealth(curentHealth);
        flashEffect.Flash();
    }
    private void SetRandomTargetPosition()
    {
        Vector2 randomDirection = UnityEngine.Random.insideUnitCircle.normalized;
        float randomDistance = UnityEngine.Random.Range(minDistance, maxDistance);
        targetPosition = (Vector2)playerTransform.position + randomDirection * randomDistance;
        isMoving = true;
    }

    private IEnumerator WaitAndMove(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        SetRandomTargetPosition();
    }
    
}
