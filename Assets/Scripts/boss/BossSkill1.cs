using UnityEngine;

public class BossSkill1 : MonoBehaviour
{
    private float timeTrack = 1; // Timer to track time between skill triggers
    public float triggerTime = 1f; // Time interval between skill triggers
    public float damage = 10f; // Damage dealt by the skill
    public player player; // Reference to the player script
    public Transform targetTransform; // Transform of the target (player)
    public float lifeTime = 5; // Lifetime of the skill before it gets destroyed

    // Start is called before the first frame update
    void Start()
    {
        player = FindAnyObjectByType<player>(); // Find the player in the scene
        Destroy(gameObject, lifeTime); // Destroy the skill object after its lifetime
        targetTransform = player.transform; // Set the target transform to the player's transform
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null || targetTransform != null)
        {
            if (timeTrack <= 0f && Vector3.Distance(targetTransform.position, transform.position) < 2)
            {
                player.takeDamage(damage, Vector2.zero, 0); // Deal damage to the player
                timeTrack = triggerTime; // Reset the timer
            }
            else
            {
                timeTrack -= Time.deltaTime; // Decrease the timer
            }
        }
    }
}
