using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Spawner : MonoBehaviour
{
#region Data
    [Header("Scriptable Object")]
    [SerializeField] EnemyDataSO humanoid;
    [SerializeField] EnemyDataSO mini;
    [SerializeField] GameObject bullet;
#endregion


    [Header("Spawn Rate")]
    [SerializeField] private float minRate = 1f;
    [SerializeField] private float maxRate = 10f;

    //Spawner 및 Object Pooling System 구현 완료.
    [Header("Spawn Radius")]
    [SerializeField] private float radius = 20f;

    private Vector3 randPosToSpawn;

    private void Awake()
    {
    }

    private void Start()
    {
        GameManager.Instance.PoolManager.CreateEnemyPool(humanoid);
        GameManager.Instance.PoolManager.CreateEnemyPool(mini);

        StartCoroutine(SpawnEnemy(humanoid));
        StartCoroutine(SpawnEnemy(mini));
    }

    private IEnumerator SpawnEnemy(EnemyDataSO _enemy)
    {
        while(true)
        {
            GetRandomPosition();
            randPosToSpawn.y = _enemy.yPosition;
            //활성화
            var enemy = GameManager.Instance.PoolManager.GetEnemy(_enemy,randPosToSpawn,Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(minRate,maxRate));
        }
    }

    private void GetRandomPosition()
    {
        randPosToSpawn = transform.position + Random.insideUnitSphere * radius;
        //해당 포지션에 장애물 없는지 확인?

    }

}
