using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]

public class Enemy : LivingEntity
{

    //public enum LodTier 
    //{ 
    //    CLOSE, 
    //    //MID , 
    //    FAR,
    //}

    //public LodTier tier;
    public LayerMask targetLayer;

    public bool attackPermission;

    //private readonly Collider[] playerColliders = new Collider[10];

    [SerializeField] private float damage = 10f;
    [SerializeField] private float speed = 5.5f;
    //[SerializeField] private float rotateSpeed = 360f;
    
    private Rigidbody rb;
    private Animator animator;
    private Collider enemyCollider;
    private float lastAttackTime;
    private float attackBetweenTime;
    private const float radius = 1f;
    private const float eyeHeight = 1.0f;
    private const float range = 1f;



    private void Awake()
    {
        animator = GetComponent<Animator>();
        enemyCollider = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
        attackBetweenTime = 1f;
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        //초기화
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        lastAttackTime = 0f;
        attackPermission = false;
        enemyCollider.enabled = true;

        //living Enemy List에 추가
        GameManager.Instance.PoolManager.AddInList(this);
        StartCoroutine(CheckLineOfSight());
    }

    public void InitData(EnemyDataSO _data)
    {
        if (_data == null)
        {
            Debug.LogError("Enemy Init Failed");
            return;
        }

        damage = _data.damage;
        speed = _data.speed;
        animator.runtimeAnimatorController = _data.runtimeAnimationController;
    }


    //public void SetLodTier(LodTier _tier)
    //{
    //    if (tier == _tier)
    //        return;

    //    tier = _tier;

    //    switch(tier)
    //    {
    //        //Collider 존재,
    //        case LodTier.CLOSE:
    //            enemyCollider.enabled = true;
    //            animator.enabled = true;
    //            rb.isKinematic = false;
    //            skinMeshRenderer.enabled = true;
    //            break;

    //        case LodTier.FAR:
    //            enemyCollider.enabled = false;
    //            animator.enabled = false;
    //            rb.isKinematic = true;
    //            skinMeshRenderer.enabled = false;
    //            break;
    //    }
            
    //}

    protected override void Die()
    {
        base.Die();
        
        enemyCollider.enabled = false;
        StartCoroutine(DieRoutine());
    }


    //수정 필요
    public void ChasingPlayer()
    {
        Vector3 playerPos = EnemyManager.playerPosition;
        Vector3 dir = playerPos - rb.position;
        dir.Normalize();

        rb.MovePosition(rb.transform.position + dir * speed * Time.fixedDeltaTime);
    }

    
    public void RotateToPlayer()
    {
        Vector3 dir = EnemyManager.playerPosition - rb.position;
        dir.y = 0f; // 높낮이는 삭제

        if (dir.sqrMagnitude < 0.01f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(dir, Vector3.up);
        rb.MoveRotation(targetRotation);
    }


    //Player가 적 감지해서 거리 내에 들어온 애들은 공격허가를 내줌
    //허가 받은 Enemy는 주기적으로 LineOfSight를 검사해서 내 앞에 Player가 있는지, 장애물이 없는지 확인한다.
    private IEnumerator CheckLineOfSight()
    {
        while(true)
        {
            if(attackPermission)
            {
                Vector3 origin = rb.position + Vector3.up * eyeHeight;
                Vector3 dir = rb.transform.forward;
                RaycastHit hit;

                if (Physics.SphereCast(origin,radius, dir, out hit, range, targetLayer))
                {
                    Attack(hit);
                    //Debug.Log("Player 공격!");
                }
            
            }
            
            yield return new WaitForSeconds(0.25f);
        }


    }

    //공격허가 상태에서 Player 공격
    private void Attack(RaycastHit hit)
    {
        if(Time.time > lastAttackTime + attackBetweenTime)
        {
            lastAttackTime = Time.time;

            IDamageable target = hit.collider.GetComponent<IDamageable>();
            
            if (target != null)
            {
                target.onDamage(damage, hit.point, hit.normal);
            }
        }
    }

    public override void onDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        base.onDamage(damage, hitPoint, hitNormal);
        //나중에 Enemy Damage Effect 추가
    }

    private IEnumerator DieRoutine()
    {
        animator.SetTrigger("Die");
        yield return new WaitForSeconds(1f);
        GameManager.Instance.PoolManager.RemoveInList(this);
        GameManager.Instance.PoolManager.EnqueueForReuse(this);
    }

}
