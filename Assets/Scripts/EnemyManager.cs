using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private Player player;
    
    public static Vector3 playerPosition {  get; private set; }

    private void FixedUpdate()
    {
        playerPosition = player.transform.position;
    }

}
