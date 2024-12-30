using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapFunctionality : MonoBehaviour
{
    [SerializeField] Tilemap nextScene;
    [SerializeField] Tilemap prevScene;
    [SerializeField] Tilemap chest;
    [SerializeField] GameObject areaExit;
    [SerializeField] GameObject areaEntrance;

    [SerializeField] public List<EnemyHpSystem> enemies;

    [SerializeField] GameObject enemyPrefab;
    [SerializeField] BoxCollider2D enemySpawnArea;

    bool isSpawningEnemies = true;
    void Start()
    {
        StartCoroutine(WaitForEnemiesSpawn());
        chest.gameObject.SetActive(false);
        nextScene.gameObject.SetActive(false);
        prevScene.gameObject.SetActive(false);
        areaExit.SetActive(false);
    }
    private void SpawnEnemies()
    {
        MapFunctionality manager = this;
        for (int i = 0; i < 5; i++)
        {
            Vector2 randomPositiom = GetRandomPosition();
            GameObject enemy = Instantiate(enemyPrefab, randomPositiom, Quaternion.identity);

            EnemyHpSystem enemyHpSystem = enemy.GetComponent<EnemyHpSystem>();
            if (enemyHpSystem != null)
            {
                enemies.Add(enemyHpSystem);
                enemyHpSystem.SetManager(manager);
            }
        }

        isSpawningEnemies = false;
    }

    void Update()
    {
        if (!isSpawningEnemies && enemies.Count == 0)
        {
            nextScene.gameObject.SetActive(true);
            prevScene.gameObject.SetActive(true);
            chest.gameObject.SetActive(true);
            areaExit.SetActive(true);
            areaEntrance.SetActive(true);
        }
    }

    private IEnumerator WaitForEnemiesSpawn()
    {
        yield return new WaitForSeconds(1);
        areaEntrance.SetActive(false);
        isSpawningEnemies = true;
        yield return new WaitForSeconds(3);
        SpawnEnemies();
    }

    private Vector2 GetRandomPosition()
    {
        if(enemySpawnArea == null)
        {
            return Vector2.zero;
        }

        Bounds bounds = enemySpawnArea.bounds;

        Vector2 spawnPos;
        do
        {
            float randomX = Random.Range(bounds.min.x, bounds.max.x);
            float randomY = Random.Range(bounds.min.y, bounds.max.y);

            spawnPos = new Vector2(randomX, randomY);
        }
        while (!IsValidPosition(spawnPos));

        return spawnPos;
    }

    private bool IsValidPosition(Vector2 pos)
    {
        if (Physics2D.OverlapPoint(pos, LayerMask.GetMask("Collision")))
        {
            return false;
        }
        return true;
    }
}
