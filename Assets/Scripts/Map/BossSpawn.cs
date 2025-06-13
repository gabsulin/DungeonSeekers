using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBossController : MonoBehaviour
{
    [Header("Scene Objects")]
    [SerializeField] GameObject bossPrefab;
    [SerializeField] GameObject areaExit;
    [SerializeField] GameObject chest;

    private bool bossSpawned = false;
    private bool battleOver = false;

    void Start()
    {
        chest.SetActive(false);
        areaExit.SetActive(false);
        StartCoroutine(StartFinalBattle());
        AudioManager.Instance.PlayMusic("Boss1", true);
    }

    private IEnumerator StartFinalBattle()
    {
        yield return new WaitForSeconds(0.1f);
        areaExit.SetActive(false);
        yield return new WaitForSeconds(1.5f);

        Vector2 centerPos = transform.position;
        if (!bossSpawned)
        {
            GameObject bossGO = Instantiate(bossPrefab, centerPos, Quaternion.identity);
        }
        bossSpawned = true;
    }

    void Update()
    {
        if (!battleOver && FindAnyObjectByType<EnemyHealth>() == null &&
            FindAnyObjectByType<EnemyHpSystem>() == null &&
            FindAnyObjectByType<BossHpSystem>() == null)
        {
            battleOver = true;
            areaExit.SetActive(true);
            chest.SetActive(true);
            GameStats.Instance.OnRoomCleared();
        }

        if (Input.GetKeyDown(KeyCode.LeftControl)) {
            areaExit.SetActive(true);
            chest.SetActive(true);
        }
    }
}
