using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class PoolManager : MonoBehaviour
{

    #region Pool

    private Dictionary<int, string> enemyId = new();
    private Dictionary<string, Queue<Enemy>> enemyPools = new();
    //private Dictionary<string, Queue<Bullet>> bulletPools = new();

    #endregion
    
    #region Data

    //살아있는 상태의 GameObject들
    public List<Enemy> livingEnemyList = new List<Enemy>();
    [SerializeField] private int initCount = 100;

    #endregion

    //GameObject 별로 Dictionary <string,queue> 존재
    //Enemy별로 pool 생성 후
    public void CreateEnemyPool(EnemyDataSO enemyData)
    {
        if (enemyData == null || string.IsNullOrEmpty(enemyData.id) || enemyData.prefab == null)
        {
            Debug.LogError("Pool 생성 불가");
            return;
        }

        //이미 해당 enemy의 id queue가 존재할 경우
        if (enemyPools.ContainsKey(enemyData.id))
        {
            Debug.LogError("이미 Pool이 존재합니다");
            return;
        }

        //id에 따른 Queue를 생성
        //size가 100인 queue
        enemyPools[enemyData.id] = new Queue<Enemy>(initCount);

        //큐에 넣기
        InstantiateEnemy(enemyData, enemyPools[enemyData.id], initCount);

        Debug.Log("Pool 생성 완료!");
        Debug.Log($"현재 Pool의 길이: {enemyPools.Count}");
    }

    public Enemy GetEnemy(EnemyDataSO enemyData, Vector3 pos, Quaternion rot)
    {
        //큐가 없으면 만들어.
        if(!enemyPools.ContainsKey(enemyData.id))
        {
            CreateEnemyPool(enemyData);
            Debug.Log("Pool 없음, 생성");
        }

        //해당 queue에서 하나 빼온다.
        var q = enemyPools[enemyData.id];

        //큐에 남은 애들이 없을 경우 10마리 씩 생성
        if (q.Count <= 0)
        {
            InstantiateEnemy(enemyData, q, 10);
            Debug.Log($"Enemy 추가 생성, Enemy id: {enemyData.id}");
        }

        var e = q.Dequeue();
        e.transform.SetPositionAndRotation(pos, rot);
        e.gameObject.SetActive(true);

        return e; //반환
    }


    //죽은 다시 큐에 넣기, gameObject 만으로 다시 재사용 할 수 없을까?
    //1. enemy에 poolId를 부여하기.
    //2. 해당 GameObject에 대한 poolId를 Manager에서 기억하기
    public void EnqueueForReuse(Enemy _enemy)
    {
        int id = _enemy.GetInstanceID();

        if (!enemyId.TryGetValue(id, out var objId))
        {
            Debug.LogError("this Game Object doesn't have key for enemyPools");
            Destroy(_enemy.gameObject);
            return;
        }
           
        _enemy.gameObject.SetActive(false);
        enemyPools[objId].Enqueue(_enemy); // enemy가 돌아가야할 큐에 다시 집어넣기.
    }

    public void InstantiateEnemy(EnemyDataSO enemyData, Queue<Enemy> q, int count)
    {

        for(int i = 0; i < count;i++)
        {
            var e = Instantiate(enemyData.prefab);
            
            //생성한 enemy data를 초기화해줌.
            if(e.TryGetComponent<Enemy>(out var enemy))
            {
                enemy.InitData(enemyData);
            
                //enemy 객체의 id를 id 딕셔너리에 저장
                //딕셔너리의 단순 조회는 O(1)이다.
                int id = enemy.GetInstanceID();
                enemyId[id] = enemyData.id;

                q.Enqueue(enemy);
                e.SetActive(false);
            }

        }

        Debug.Log($"생성된 Enemy 숫자: {q.Count}");
    }

    //살아있는 Enemy 리스트로 관리하기 위한, List 추가 , List 삭제 구현
    //살아있는 애들만 List로 관리, 죽으면 List에서 삭제
    //Enemy List의 관리 자체는 Enemy에서 관리!!
    //다시 태어나면 (OnEnable) -> 다시 AddInList

    public void AddInList(Enemy _enemy)
    {
        livingEnemyList.Add(_enemy);
    }

    public void RemoveInList(Enemy _enemy)
    {
        livingEnemyList.Remove(_enemy); //이는 자동으로 indexOf를 사용해서 찾아준다.
    }

}
