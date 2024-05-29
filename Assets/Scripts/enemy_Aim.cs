using UnityEngine;

public class enemy_Aim : MonoBehaviour
{
    public Camera cam; // Reference to the main camera
    public SpriteRenderer gunSpriteRenderer; // SpriteRenderer for the gun
    public Transform target; // Target for the enemy to aim at

    private float followSpeed = 8f; // Speed at which the enemy aims towards the target

    private int defaultSortingOrder = 9; // Default sorting order for the gun sprite
    private int flippedSortingOrder = 11; // Sorting order for the gun sprite when flipped

    private void Awake()
    {
        gunSpriteRenderer = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component
        cam = Camera.main; // Get the main camera
    }

    // Update is called once per frame
    void Update()
    {
        AimWeapon(); // Aim the weapon
    }

    // Method to aim the weapon at the target
    void AimWeapon()
    {
        if (target != null)
        {
            Vector2 targetDirection = target.position - transform.position; // Calculate the direction to the target
            float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg; // Calculate the angle to rotate the weapon
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(0, 0, angle + 180)), Time.deltaTime * followSpeed); // Smoothly rotate the weapon

            bool shouldFlip = targetDirection.x > 0; // Determine if the weapon should be flipped
            gunSpriteRenderer.flipY = shouldFlip; // Flip the gun sprite vertically if needed
            gunSpriteRenderer.sortingOrder = shouldFlip ? flippedSortingOrder : defaultSortingOrder; // Set the sorting order based on the flip
        }
    }
}

