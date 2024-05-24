
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    private AudioSource musicSource;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.loop = true;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayMusic(AudioClip clip)
    {
        if (musicSource.clip != clip)
        {
            musicSource.clip = clip;
            musicSource.volume = 0.05f;
            musicSource.Play();
        }
    }

    public void GunSound(AudioClip clip)
    {
        PlayTemporarySound(clip, 0.5f);
    }

    public void EnemyGunSound(AudioClip clip)
    {
        PlayTemporarySound(clip, 0.3f);
    }

    public void KnifeHitSound(AudioClip clip)
    {
        PlayTemporarySound(clip, 0.6f);
    }

    public void BulletHitSound(AudioClip clip)
    {
        PlayTemporarySound(clip, 0.2f);
    }

    public void CoinSound(AudioClip clip)
    {
        PlayTemporarySound(clip, 0.2f);
    }

    public void BulletMapHitSound(AudioClip clip)
    {
        PlayTemporarySound(clip, 0.2f);
    }

    private void PlayTemporarySound(AudioClip clip, float volume)
    {
        GameObject soundObject = new GameObject("TemporaryAudio");
        AudioSource tempAudioSource = soundObject.AddComponent<AudioSource>();
        tempAudioSource.clip = clip;
        tempAudioSource.volume = volume;
        tempAudioSource.Play();
        Destroy(soundObject, clip.length);
    }
}
