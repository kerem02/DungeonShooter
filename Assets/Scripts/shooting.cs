using UnityEngine;

public class shooting : MonoBehaviour
{
    public Transform firepoint; // The point from which the bullet is fired
    public Transform firepoint2; // The second fire point for double pistols
    public GameObject bulletPrefab; // The bullet prefab to instantiate
    public AudioClip shootSound; // Sound to play when shooting
    private AudioManager sound; // Reference to the AudioManager
    private player player; // Reference to the player script

    private float nextFireTime = 0f; // Time until the next shot can be fired

    void Awake()
    {
        player = GetComponentInParent<player>(); // Get the player script from the parent object
        sound = AudioManager.instance; // Get the instance of the AudioManager
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 0) return; // Do nothing if the game is paused

        Weapon currentWeapon = player.GetCurrentWeapon(); // Get the current weapon from the player
        if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + currentWeapon.fireRate; // Set the next fire time based on the weapon's fire rate

            if (currentWeapon.weaponType == WeaponType.DoublePistol)
            {
                DoubleShoot(); // Fire two bullets if the weapon is a double pistol
            }
            else
            {
                Shoot(); // Fire a single bullet otherwise
            }
        }
    }

    // Method to shoot a single bullet
    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firepoint.position, firepoint.rotation); // Instantiate a bullet at the fire point
        sound.GunSound(shootSound); // Play the shooting sound
    }

    // Method to shoot two bullets
    void DoubleShoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firepoint.position, firepoint.rotation); // Instantiate the first bullet
        GameObject bullet2 = Instantiate(bulletPrefab, firepoint2.position, firepoint2.rotation); // Instantiate the second bullet
        sound.GunSound(shootSound); // Play the shooting sound
    }
}
