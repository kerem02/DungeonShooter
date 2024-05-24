using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shooting : MonoBehaviour
{
    public Transform firepoint;
    public Transform firepoint2;
    public GameObject bulletPrefab;
    public AudioClip shootSound;
    private AudioManager sound;
    private player player;

    private float nextFireTime = 0f;
 


    void Awake()
    {

        player = GetComponentInParent<player>();
        sound = AudioManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.timeScale == 0) return;

        Weapon currentWeapon = player.GetCurrentWeapon();
        if (Input.GetButtonDown("Fire1") && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + currentWeapon.fireRate;

            if(currentWeapon.weaponType == WeaponType.DoublePistol)
            {
                DoubleShoot();
            }
            else
            {
                Shoot();
            }
        }
        
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firepoint.position, firepoint.rotation);
        sound.GunSound(shootSound);
    }
    void DoubleShoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firepoint.position, firepoint.rotation);
        GameObject bullet2 = Instantiate(bulletPrefab, firepoint2.position, firepoint2.rotation);
        sound.GunSound(shootSound);
    }


}
