using UnityEngine;

public class Gold : MonoBehaviour
{
    public AudioManager sound; // Reference to the AudioManager
    public AudioClip goldPickUp; // Sound to play when gold is picked up

    [SerializeField] private int goldAmount = 1; // Amount of gold this object gives

    private void Awake()
    {
        sound = AudioManager.instance; // Get the instance of the AudioManager
    }

    // Method called when another collider enters this object's trigger collider
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<player>().AddGold(goldAmount); // Add gold to the player
            sound.CoinSound(goldPickUp); // Play the gold pick-up sound
            Destroy(gameObject); // Destroy the gold object
        }
    }
}

