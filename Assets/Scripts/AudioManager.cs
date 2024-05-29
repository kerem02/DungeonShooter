using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance; // Singleton instance of AudioManager
    private AudioSource musicSource; // AudioSource for playing music

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Prevent this object from being destroyed when changing scenes
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.loop = true; // Enable looping for the music
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances of AudioManager
        }
    }

    public void PlayMusic(AudioClip clip)
    {
        if (musicSource.clip != clip)
        {
            musicSource.clip = clip;
            musicSource.volume = 0.05f; // Set the volume level for the music
            musicSource.Play(); // Play the music clip
        }
    }

    // Play gunshot sound
    public void GunSound(AudioClip clip)
    {
        PlayTemporarySound(clip, 0.5f);
    }

    // Play enemy gunshot sound
    public void EnemyGunSound(AudioClip clip)
    {
        PlayTemporarySound(clip, 0.3f);
    }

    // Play knife hit sound
    public void KnifeHitSound(AudioClip clip)
    {
        PlayTemporarySound(clip, 0.6f);
    }

    // Play bullet hit sound
    public void BulletHitSound(AudioClip clip)
    {
        PlayTemporarySound(clip, 0.2f);
    }

    // Play coin collection sound
    public void CoinSound(AudioClip clip)
    {
        PlayTemporarySound(clip, 0.2f);
    }

    // Play bullet hitting map sound
    public void BulletMapHitSound(AudioClip clip)
    {
        PlayTemporarySound(clip, 0.2f);
    }

    // Helper method to play a temporary sound
    private void PlayTemporarySound(AudioClip clip, float volume)
    {
        GameObject soundObject = new GameObject("TemporaryAudio"); // Create a new temporary GameObject
        AudioSource tempAudioSource = soundObject.AddComponent<AudioSource>(); // Add an AudioSource to the GameObject
        tempAudioSource.clip = clip; // Set the AudioClip to play
        tempAudioSource.volume = volume; // Set the volume level
        tempAudioSource.Play(); // Play the AudioClip
        Destroy(soundObject, clip.length); // Destroy the GameObject after the clip has finished playing
    }
}