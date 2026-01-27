using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;


public class ForOptimization : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Enemy enemy;
    [SerializeField] private int enemyCount = 1000;
    [SerializeField] private float nearDist = 15f;

    private Vector3 randPos;
    private readonly List<Enemy> enemies = new List<Enemy>();

    void Start()
    {
        CreateEnemy();
        //Debug.Log("Complete Instantiate");
    }


    private void FixedUpdate()
    {
        ControlEnemy();
    }

    private void CreateEnemy()
    {
        for (int i = 0; i < enemyCount; i++)
        {
            randPos = transform.position + Random.insideUnitSphere * 30f;
            randPos.y = 0f; //높낮이 삭제

            var newEnemy = Instantiate(enemy,randPos,Quaternion.identity);
            enemies.Add(newEnemy);
        }
        
    }
    

    //Manager에서 한번에 실행시킴
    private void ControlEnemy()
    {
        for(int i = 0; i < enemies.Count; i++)
        {
            enemies[i].ChasingPlayer();
            enemies[i].RotateToPlayer();
        }
    }
    
    private void UpdateLodTier()
    {
        for(int i = 0; i < enemyCount; i++)
        {
            
        }
    }

}
