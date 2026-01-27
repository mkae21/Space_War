using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform firePosition;
    public Projectile bullet;

    private AudioSource audioSource;

    private float shootingRate;
    private float fireVelocity;

    private GunData data;
    private float lastShotTime;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    //장착 될 때 외부에서 주입
    public void InitGunData(GunData gunData)
    {
        data = gunData;
        lastShotTime = 0;
        shootingRate = data.shootingRate;
        fireVelocity = data.fireVelocity;
    }

    public void Shoot()
    {
        //Gun data가 없으면 불가능
        if (data == null)
            return;

        if(Time.time > lastShotTime + shootingRate / 100)
        {
            lastShotTime = Time.time;
            Projectile newProjectile = Instantiate(bullet, firePosition.position, firePosition.rotation) as Projectile;
            newProjectile.Init(data);
            audioSource.PlayOneShot(audioSource.clip);
        }

    }
    
}
