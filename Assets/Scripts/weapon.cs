
using UnityEngine;

public enum WeaponType
{
    Pistol,
    DoublePistol,
    Rifle
}

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class Weapon : ScriptableObject
{
    public WeaponType weaponType;
    public GameObject weaponPrefab;

    public float fireRate;
}