using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapFunctionality : MonoBehaviour
{
    [Header("Scene Objects")]
    [SerializeField] GameObject chest;
    [SerializeField] GameObject areaExit;

    [Header("Wave Settings")]
    [SerializeField] GameObject[] enemyPrefabs;
    private List<int> enemiesPerWave = new List<int>();
    private int currentWaveIndex = 0;
    private bool allWavesCompleted = false;
    private bool waveInProgress = false;

    [Header("Spawn Area")]
    [SerializeField] Tilemap groundTilemap;
    private List<Vector2> validSpawnPositions = new List<Vector2>();

    [Header("Runtime")]
    [SerializeField] public List<EnemyHpSystem> spumEnemies;
    [SerializeField] public List<EnemyHealth> enemies;

    private WaveTextEffect waveTextEffect;
    private bool isSpawningEnemies = true;

    void Start()
    {
        GameObject canvasGO = GameObject.Find("Canvas");
        if (canvasGO != null)
        {
            waveTextEffect = canvasGO.transform.Find("WaveText").GetComponent<WaveTextEffect>();
        }

        GenerateRandomWaves();

        StartCoroutine(WaitForWaveTexToSpawn());
        chest.SetActive(false);

        StartCoroutine(WaitForEnemiesSpawn());
        GameStats.Instance.OnRoomStart();
        CacheValidSpawnTiles();
    }

    void Update()
    {
        if (!isSpawningEnemies && spumEnemies.Count == 0 && enemies.Count == 0 && !allWavesCompleted && !waveInProgress)
        {
            waveInProgress = true;

            if (currentWaveIndex < enemiesPerWave.Count - 1)
            {
                currentWaveIndex++;
                StartCoroutine(WaitForEnemiesSpawn());
            }
            else
            {
                allWavesCompleted = true;
                chest.SetActive(true);
                areaExit.SetActive(true);

                GameStats.Instance.OnRoomCleared();
            }
        }
    }

    private void GenerateRandomWaves()
    {
        int totalWaves = Random.Range(2, 6);

        for (int i = 0; i < totalWaves; i++)
        {
            int minEnemies = 2 + i;
            int maxEnemies = 4 + i * 2;
            int enemiesInWave = Random.Range(minEnemies, maxEnemies + 1);
            enemiesPerWave.Add(enemiesInWave);
        }
    }

    private IEnumerator WaitForEnemiesSpawn()
    {
        yield return new WaitForSeconds(0.1f);

        isSpawningEnemies = true;

        DisplayWaveStartEffects();

        yield return new WaitForSeconds(2);
        SpawnEnemies();
    }

    private IEnumerator WaitForWaveTexToSpawn()
    {
        yield return new WaitForSeconds(1f);
        DisplayWaveStartEffects();
    }

    private void DisplayWaveStartEffects()
    {
        if (waveTextEffect != null)
        {
            waveTextEffect.DisplayWaveText(currentWaveIndex + 1);
        }
        // you can slap in a lil SFX here if you want
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
            else
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

    /*private Vector2 GetRandomPosition()
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
    }*/

    private void CacheValidSpawnTiles()
    {
        validSpawnPositions.Clear();

        BoundsInt bounds = groundTilemap.cellBounds;
        TileBase[] allTiles = groundTilemap.GetTilesBlock(bounds);

        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                TileBase tile = allTiles[x + y * bounds.size.x];
                if (tile != null)
                {
                    Vector3Int cellPos = new Vector3Int(bounds.x + x, bounds.y + y, 0);
                    Vector3 worldPos = groundTilemap.GetCellCenterWorld(cellPos);
                    validSpawnPositions.Add(worldPos);
                }
            }
        }
    }
    private Vector2 GetRandomPosition()
    {
        if (validSpawnPositions.Count == 0)
        {
            Debug.LogWarning("No valid spawn tiles found!");
            return transform.position; // fallback
        }

        return validSpawnPositions[Random.Range(0, validSpawnPositions.Count)];
    }
}
