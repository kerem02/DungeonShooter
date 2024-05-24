using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void GunSound(AudioClip clip)
    {
        GameObject soundObject = new GameObject("TemporaryAudio");
        AudioSource tempAudioSource = soundObject.AddComponent<AudioSource>();
        tempAudioSource.clip = clip;
        tempAudioSource.volume = 0.5f;
        tempAudioSource.Play();
        Destroy(soundObject, clip.length);
    }

    public void KnifeHitSound(AudioClip clip)
    {
        GameObject soundObject = new GameObject("TemporaryAudio");
        AudioSource tempAudioSource = soundObject.AddComponent<AudioSource>();
        tempAudioSource.clip = clip;
        tempAudioSource.volume = 0.5f;
        tempAudioSource.Play();
        Destroy(soundObject, clip.length);
    }

    public void BulletHitSound(AudioClip clip)
    {
        GameObject soundObject = new GameObject("TemporaryAudio");
        AudioSource tempAudioSource = soundObject.AddComponent<AudioSource>();
        tempAudioSource.clip = clip;
        tempAudioSource.volume = 0.5f;
        tempAudioSource.Play();
        Destroy(soundObject, clip.length);
    }

    public void CoinSound(AudioClip clip)
    {
        GameObject soundObject = new GameObject("TemporaryAudio");
        AudioSource tempAudioSource = soundObject.AddComponent<AudioSource>();
        tempAudioSource.clip = clip;
        tempAudioSource.volume = 0.1f;
        tempAudioSource.Play();
        Destroy(soundObject, clip.length);
    }
}
