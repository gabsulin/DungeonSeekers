using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapFunctionality : MonoBehaviour
{
    [Header("Scene Objects")]
    [SerializeField] Tilemap nextScene;
    [SerializeField] Tilemap prevScene;
    [SerializeField] GameObject chest;
    [SerializeField] GameObject areaExit;
    [SerializeField] GameObject areaEntrance;

    [Header("Wave Settings")]
    [SerializeField] GameObject[] enemyPrefabs;
    [SerializeField] int[] enemiesPerWave = { 2, 4, 6 };
    private int currentWaveIndex = 0;
    private bool allWavesCompleted = false;
    private bool waveInProgress = false;

    [Header("Spawn Area")]
    [SerializeField] BoxCollider2D enemySpawnArea;

    [Header("Runtime")]
    [SerializeField] public List<EnemyHpSystem> spumEnemies;
    [SerializeField] public List<EnemyHealth> enemies;

    private WaveTextEffect waveTextEffect;
    private bool isSpawningEnemies = true;

    void Start()
    {
        Canvas canvas = FindFirstObjectByType<Canvas>();
        waveTextEffect = canvas.transform.Find("WaveText").GetComponent<WaveTextEffect>();

        DisplayWaveStartEffects();
        chest.SetActive(false);
        nextScene.gameObject.SetActive(true);
        prevScene.gameObject.SetActive(true);

        StartCoroutine(WaitForEnemiesSpawn());
    }

    void Update()
    {
        // When wave is done and no enemies left
        if (!isSpawningEnemies && spumEnemies.Count == 0 && enemies.Count == 0 && !allWavesCompleted && !waveInProgress)
        {
            waveInProgress = true;

            if (currentWaveIndex < enemiesPerWave.Length - 1)
            {
                currentWaveIndex++;
                StartCoroutine(WaitForEnemiesSpawn());
            }
            else
            {
                allWavesCompleted = true;

                nextScene.gameObject.SetActive(false);
                prevScene.gameObject.SetActive(false);
                chest.SetActive(true);
                areaExit.SetActive(true);
                areaEntrance.SetActive(true);
            }
        }
    }

    private IEnumerator WaitForEnemiesSpawn()
    {
        yield return new WaitForSeconds(0.1f);

        areaEntrance.SetActive(false);
        areaExit.SetActive(false);
        isSpawningEnemies = true;

        DisplayWaveStartEffects();

        yield return new WaitForSeconds(2);
        SpawnEnemies();
    }

    private void DisplayWaveStartEffects()
    {
        if (waveTextEffect != null)
        {
            waveTextEffect.DisplayWaveText(currentWaveIndex + 1);
        }

        // Optional: play wave start sound here
    }

    private void SpawnEnemies()
    {
        int currentEnemiesCount = enemiesPerWave[currentWaveIndex];

        for (int i = 0; i < currentEnemiesCount; i++)
        {
            Vector2 randomPosition = GetRandomPosition();
            GameObject selectedEnemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

            GameObject enemy = Instantiate(selectedEnemyPrefab, randomPosition, Quaternion.identity);

            EnemyHpSystem spumEnemyHpSystem = enemy.GetComponent<EnemyHpSystem>();
            if (spumEnemyHpSystem != null)
            {
                spumEnemies.Add(spumEnemyHpSystem);
                spumEnemyHpSystem.SetManager(this);
            }
            if(spumEnemyHpSystem == null)
            {
                EnemyHealth enemyHpSystem = enemy.GetComponent<EnemyHealth>();
                if (enemyHpSystem != null)
                {
                    enemies.Add(enemyHpSystem);
                    enemyHpSystem.SetManager(this);
                }
            }
            
        }

        isSpawningEnemies = false;
        waveInProgress = false;
    }

    private Vector2 GetRandomPosition()
    {
        if (enemySpawnArea == null)
            return Vector2.zero;

        Bounds bounds = enemySpawnArea.bounds;
        const int maxAttempts = 500;
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
            Debug.LogWarning("No valid spawn position found, using center.");
            return bounds.center;
        }

        return spawnPos;
    }

    private bool IsValidPosition(Vector2 pos)
    {
        return !Physics2D.OverlapPoint(pos, LayerMask.GetMask("Collision"));
    }
}