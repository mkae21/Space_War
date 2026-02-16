using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Gun Data", menuName = "Weapons/Gun Data")] 
public class GunData : ScriptableObject
{
    [Header("Model")]
    public GameObject prefab;

    //[Header("Holder")]
    //public Transform holder;

    [Header("data")]
    public float damage = 15f;
    public int ammoCapacity = 15;

    public float reloadingTime = 1f;
    public float shootingRate = 10f;
    public float shootingRange = 10f;
    public float fireVelocity = 45f;

    [Header("Spawn Point Offset")]
    public float posX;
    public float posY;
    public float posZ;
    public float roatX;
    public float roatY;
    public float roatZ;

    //ÃÑ¿¡ µû¸¥ Animation
    [Header("Animation")]
    public AnimatorOverrideController playerUppderBodyAnimation;

    //public Projectile projectilePrefabs -> ÃÑ¿¡ µû¸¥ ÃÑ¾Ë

}
