using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/EnemySO",fileName ="EnemySO")]
public class EnemyDataSO : ScriptableObject
{
    [Header("ID")]
    public string id;

    [Header("Stats")]
    public int health;
    public int damage;
    public float speed;

    [Header("Visual")]
    public GameObject prefab;

    [Header("Animation")]
    public RuntimeAnimatorController runtimeAnimationController;

    [Header("InStantiate Position of Y")]
    public float yPosition;

}
