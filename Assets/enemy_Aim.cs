using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class enemy_Aim : MonoBehaviour
{
    public Camera cam;
    public SpriteRenderer gunSpriteRenderer;
    public Transform target;

    public float followSpeed = 1f;

    private int defaultSortingOrder = 9;
    private int flippedSortingOrder = 11;

    private void Awake()
    {
        gunSpriteRenderer = GetComponent<SpriteRenderer>();
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        AimWeapon();
    }

    void AimWeapon()
    {   if (target != null)
        {
            Vector2 targetDirection = target.position - transform.position;
            float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(0, 0, angle + 180)), Time.deltaTime * followSpeed);

            bool shouldFlip = targetDirection.x > 0;
            gunSpriteRenderer.flipY = shouldFlip;
            gunSpriteRenderer.sortingOrder = shouldFlip ? flippedSortingOrder : defaultSortingOrder;
        }
    }
}
