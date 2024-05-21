using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shooting : MonoBehaviour
{
    public Transform firepoint;
    public GameObject bulletPrefab;
    public AudioSource audioSource;

    private float fireRate = 0.5f;
    private float nextFireTime = 0f;
 


    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            Shoot();
        }
        
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firepoint.position, firepoint.rotation);
        audioSource.Play();
    }
}
