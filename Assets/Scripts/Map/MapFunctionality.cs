using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapFunctionality : MonoBehaviour
{
    [SerializeField] Tilemap nextScene;
    [SerializeField] Tilemap prevScene;

    [SerializeField] public List<EnemyHpSystem> enemies;

    [SerializeField] GameObject enemyPrefab;
    [SerializeField] Transform enemySpawnpoint;

    bool isCleared;
    void Start()
    {
        //StartCoroutine(WaitForEnemiesSpawn());
        SpawnEnemies();
        nextScene.gameObject.SetActive(false);
        prevScene.gameObject.SetActive(false);
    }
    private void SpawnEnemies()
    {
        MapFunctionality manager = this;
        for (int i = 0; i < 5; i++)
        {
            
        }
        GameObject enemy = Instantiate(enemyPrefab, enemySpawnpoint.transform.position, Quaternion.identity);
        EnemyHpSystem enemyHpSystem = enemy.GetComponent<EnemyHpSystem>();

        if (enemyHpSystem != null)
        {
            enemies.Add(enemyHpSystem);
            enemyHpSystem.SetManager(manager);
        }
    }

    void Update()
    {
        if (enemies.Count == 0)
        {
            nextScene.gameObject.SetActive(true);
            prevScene.gameObject.SetActive(true);
        }
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


    private IEnumerator WaitForEnemiesSpawn()
    {
        yield return new WaitForSeconds(3);
        SpawnEnemies();
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