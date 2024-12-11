using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapFunctionality : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] Transform enemySpawnpoint;
    void Start()
    {
        SpawnEnemies();
    }

    void Update()
    {


        /*
         
        if(enemiesInRoom.Count == 0)
        {
            IsClear(currentRoom, true);
        }

        if(IsClear(currentRoom, true))
        {
            OpenRoom();
            SpawnChest();
        }
         */
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        /*
         if (collision.CompareTag("Player") && !IsClear(currentRoom, false))
        {
            CloseRoom();
            SpawnEnemies();
        }
         */

    }

    /*
    private bool IsClear(currentRoom, isClear)
    {

    }
     */

    private void SpawnEnemies()
    {
        for(int i = 0; i < 5; i++)
        {
            Instantiate(enemyPrefab, enemySpawnpoint.transform.position, Quaternion.identity);
        }
        
    }
}


/*
SpawnEnemies();
OpenRoom();
CloseRoom();
SpawnChest();
IsClear(currentRoom, bool isClear);
GetCurrentRoom();
 */