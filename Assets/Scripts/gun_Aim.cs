using UnityEngine;

public class gun_Aim : MonoBehaviour
{
    public Camera cam; // Reference to the main camera
    public SpriteRenderer gunSpriteRenderer; // SpriteRenderer for the gun

    private int defaultSortingOrder = 9; // Default sorting order for the gun sprite
    private int flippedSortingOrder = 11; // Sorting order for the gun sprite when flipped

    private void Awake()
    {
        gunSpriteRenderer = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 0) return; // Do nothing if the game is paused
        AimWeapon(); // Aim the weapon
    }

    // Method to aim the weapon
    void AimWeapon()
    {
        Vector2 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition); // Get the mouse position in world space
        Vector2 weaponPosition = transform.position; // Get the weapon position
        Vector2 direction = mousePosition - weaponPosition; // Calculate the direction to aim
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // Calculate the angle to rotate the weapon
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 180)); // Rotate the weapon

        bool shouldFlip = direction.x > 0; // Determine if the weapon should be flipped
        gunSpriteRenderer.flipY = shouldFlip; // Flip the gun sprite vertically if needed
        gunSpriteRenderer.sortingOrder = shouldFlip ? flippedSortingOrder : defaultSortingOrder; // Set the sorting order based on the flip
    }
}

