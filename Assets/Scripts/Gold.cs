
using UnityEngine;

public class Gold : MonoBehaviour
{
    public AudioManager sound;
    public AudioClip goldPickUp;

    [SerializeField] private int goldAmount = 1;

    private void Awake()
    {
        sound = AudioManager.instance;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<player>().AddGold(goldAmount);
            sound.CoinSound(goldPickUp);
            Destroy(gameObject);
        }
        
    }
}
