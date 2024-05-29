using UnityEngine;

// Enum to define different types of weapons
public enum WeaponType
{
    Pistol,
    DoublePistol,
    Rifle
}

// ScriptableObject to define a Weapon
[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class Weapon : ScriptableObject
{
    public WeaponType weaponType; // Type of the weapon
    public GameObject weaponPrefab; // Prefab for the weapon

    public float fireRate; // Rate of fire for the weapon
}
