using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapFunctionality : MonoBehaviour
{
    [SerializeField] Tilemap nextScene;
    [SerializeField] Tilemap prevScene;
    //[SerializeField] Tilemap chest;
    [SerializeField] GameObject chest;
    [SerializeField] GameObject areaExit;
    [SerializeField] GameObject areaEntrance;

    [SerializeField] public List<EnemyHpSystem> enemies;
    [SerializeField] int enemiesCount;

    [SerializeField] GameObject enemyPrefab;
    [SerializeField] BoxCollider2D enemySpawnArea;

    bool isSpawningEnemies = true;
    void Start()
    {
        StartCoroutine(WaitForEnemiesSpawn());
        chest.gameObject.SetActive(false);
        nextScene.gameObject.SetActive(true);
        prevScene.gameObject.SetActive(true);
    }
    private void SpawnEnemies()
    {
        MapFunctionality manager = this;
        for (int i = 0; i < enemiesCount; i++)
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
            nextScene.gameObject.SetActive(false);
            prevScene.gameObject.SetActive(false);
            chest.gameObject.SetActive(true);
            areaExit.SetActive(true);
            areaEntrance.SetActive(true);
        }
    }

    private IEnumerator WaitForEnemiesSpawn()
    {
        yield return new WaitForSeconds(0.1f);
        areaEntrance.SetActive(false);
        areaExit.SetActive(false);
        isSpawningEnemies = true;
        yield return new WaitForSeconds(3);
        SpawnEnemies();
    }

    private Vector2 GetRandomPosition()
    {
        if (enemySpawnArea == null)
        {
            return Vector2.zero;
        }

        Bounds bounds = enemySpawnArea.bounds;

        const int maxAttempts = 100;
        int attempts = 0;
        Vector2 spawnPos;

        do
        {
            float randomX = Random.Range(bounds.min.x, bounds.max.x);
            float randomY = Random.Range(bounds.min.y, bounds.max.y);

            spawnPos = new Vector2(randomX, randomY);
            attempts++;
        }
        while (!IsValidPosition(spawnPos) && attempts < maxAttempts);

        if (attempts >= maxAttempts)
        {
            Debug.LogWarning("Could not find a valid position for enemy spawn.");
            return bounds.center;
        }

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
