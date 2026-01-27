using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof (PlayerInput))]
[RequireComponent (typeof (Rigidbody))]
[RequireComponent (typeof (GunController))]
public class Player : LivingEntity
{
    
    PlayerInput playerInput;
    Rigidbody rb;
    GunController gunController;
    Camera viewCamera;
    Animator animator;
    
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private float detectRadius = 0.8f;
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float sprintSpeed = 9.5f;
    //[SerializeField] private Transform aimTarget;

    private Vector3 moveVelocity;
    private Vector3 playerPos;
    private Vector3 detectingPos;
    private bool isRunning;
    private bool isMoving;
    private float currentSpeed;

    private readonly Collider[] hitColliders = new Collider[1024];
    
    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
        viewCamera = Camera.main;
        gunController = GetComponent<GunController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {

        if (isDead)
            return;

        //Weapon Input
        if(playerInput.leftMouseButtonPushed)
        {
            gunController.Shoot();
        }

        //움직임 감지
        movementInput();
        
        //animation
        UpdateAnimator();
        
        //Game-Algorithms
        CheckEnemyPermission();
    }

    private void FixedUpdate()
    {
        Move();
        BodyRotate();
    }


    public Vector3 GetPlayerPosition()
    {
        playerPos = rb.transform.position;
        return playerPos;
    }

    public override void onDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        base.onDamage(damage, hitPoint, hitNormal);

        //나중에 hit Effect 추가 연출할 것.
        //Debug.Log("Player On Damaged !!");

        if (isDead)
        {
            animator.SetTrigger("Die");
            playerInput.enabled = false;
            moveVelocity = Vector3.zero;
        }
    }


    private void UpdateAnimator()
    {
        animator.SetBool("Run", isRunning);
        animator.SetBool("Walk", isMoving);
    }
    
    private void BodyRotate()
    {
        //Look Input
        Ray ray = viewCamera.ScreenPointToRay(playerInput.mousePosition);
        
        Plane groundPlane = new Plane(Vector3.up, new Vector3(0, transform.position.y, 0)); //법선 벡터와 원점에서의 거리
        float rayDistance;

        if (groundPlane.Raycast(ray, out rayDistance))
        {
            Vector3 point = ray.GetPoint(rayDistance);


            Vector3 targetPoint = new Vector3(point.x, transform.position.y, point.z);
            Vector3 dir = targetPoint - rb.position;

            //너무 값이 적으면 회전 X
            if (dir.sqrMagnitude < 0.001f)
                return;

            Quaternion rot = Quaternion.LookRotation(dir, Vector3.up);
            rb.MoveRotation(rot);
            
        }
    }

    private void movementInput()
    {
        //입력 값
        float h = playerInput.moveHorizontal;
        float v = playerInput.moveVertical;

        Vector3 camForward = viewCamera.transform.forward;
        Vector3 camRight = viewCamera.transform.right;

        // y 벡터 값 삭제
        camForward.y = 0f;
        camRight.y = 0f;

        //단위 벡터로 변경 -> 카메라가 기울어진 경우 forward,right가 y=0일 때 크기가 1이 아님.
        camForward.Normalize();
        camRight.Normalize(); ;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               

        Vector3 targetDir = camRight * h + camForward * v;

        isMoving = (targetDir.sqrMagnitude > 0.01f ? true : false);
        isRunning = (isMoving && playerInput.leftShiftButtonPushed ? true : false);
        
        currentSpeed = (isMoving && !isDead ? (isRunning ? sprintSpeed : walkSpeed) : 0f);
        moveVelocity = (isMoving && !isDead ? targetDir.normalized * currentSpeed : Vector3.zero);

    }

    private void Move()
    {
        //rb.position과 transform.position의 차이점
        rb.MovePosition(rb.position + moveVelocity  * Time.fixedDeltaTime); //속도 X 시간
    }


    //OverlapSphereNonAlloc을 이용하여 Enemy 감지하고 AttackPermission 주기
    private void CheckEnemyPermission()
    {
        detectingPos = transform.position + Vector3.up * 2f;

        int num = Physics.OverlapSphereNonAlloc(detectingPos, detectRadius, hitColliders, targetLayer);

        if (num == 0)
        {
            //Debug.Log("enemy 주위에 없음");
            return;
        }

        for(int i = 0;  i < num; i++)
        {
            var enemy = hitColliders[i].GetComponent<Enemy>();
            
            if(enemy != null)
            {
                enemy.attackPermission = true;
                //Debug.Log("공격 허가!");
            }
        }
    }


    
    


}
