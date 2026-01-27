using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Gun Data", menuName = "Weapons/Gun Data")] 
public class GunData : ScriptableObject
{
    public float damage = 15f;
    public int ammoCapacity = 15;

    public float reloadingTime = 1f;
    public float shootingRate = 10f;
    public float shootingRange = 10f;
    public float fireVelocity = 45f;

    //public Projectile projectilePrefabs;
}
