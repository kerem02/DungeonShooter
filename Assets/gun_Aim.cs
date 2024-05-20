using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class gun_Aim : MonoBehaviour
{
    public Camera cam;
    public SpriteRenderer gunSpriteRenderer;

    private int defaultSortingOrder = 9;
    private int flippedSortingOrder = 11;

    private void Awake()
    {
        gunSpriteRenderer = GetComponent<SpriteRenderer>();
    }


    // Update is called once per frame
    void Update()
    {
        AimWeapon();
    }

    void AimWeapon()
    {
        Vector2 mousePosition = cam.ScreenToWorldPoint( Input.mousePosition );
        Vector2 weaponPosition = transform.position;
        Vector2 direction = mousePosition - weaponPosition;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0,0, angle + 180));

        bool shouldFlip = direction.x > 0;
        gunSpriteRenderer.flipY = shouldFlip;
        gunSpriteRenderer.sortingOrder = shouldFlip ? flippedSortingOrder : defaultSortingOrder;
    }


}
