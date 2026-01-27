using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(PoolManager))]
public class GameManager : MonoBehaviour
{
    //Singleton pattern
    public static GameManager Instance;

    private PoolManager poolManager;
    public PoolManager PoolManager { get => poolManager; private set => poolManager = value; }


    private void Awake()
    {
        PoolManager = GetComponent<PoolManager>();

        //Singleton
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
    }

    private void FixedUpdate()
    {
        ControlEnemy();
    }


    private void ControlEnemy()
    {
        var list = PoolManager.livingEnemyList;
        
        for(int i =  0; i < list.Count; i++)
        {
            list[i].ChasingPlayer();
            list[i].RotateToPlayer();
        }
    }
}
