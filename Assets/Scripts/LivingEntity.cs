using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class LivingEntity : MonoBehaviour,IDamageable
{
    //체력 관리 시스템
    [SerializeField] public float startingHealth = 100f;
    protected float health { get; private set; }
    public bool isDead { get; private set; }

    //public event Action OnDeath;

    //초기화 ( 오브젝트 풀링 고려한 것)
    protected virtual void OnEnable() 
    {
        health = startingHealth;
        isDead = false;
    }

    public virtual void onDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        health -= damage;
        
        if (health <= 0 && !isDead)
        {
            Die();
        }

    }
    protected virtual void Die()
    {
        if (isDead)
            return;

        isDead = true;
    }
}
