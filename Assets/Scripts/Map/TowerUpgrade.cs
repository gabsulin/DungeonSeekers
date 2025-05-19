using System;
using System.Collections;
using System.Resources;
using UnityEngine;

public class TowerUpgrade : MonoBehaviour, IInteractable
{
    [SerializeField] private int maxLevel = 7;
    [SerializeField] private float upgradeTime = 3f;
    [SerializeField] private int[] upgradeCosts = { 10, 20, 30, 40, 50, 60, 70 };

    Animator anim;
    private CoinManager coinManager;
    SpriteRenderer spriteRenderer;

    private int currentLevel = 0;
    private bool isUpgrading = false;
    Color upgradingColor = new Color(0.6f, 0.6f, 0.6f, 0.6f);


    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void Interact()
    {
        if (!isUpgrading && CanUpgrade())
        {
            StartCoroutine(UpgradeToNextLevel());
        }
    }

    private bool CanUpgrade()
    {
        if (currentLevel >= maxLevel)
        {
            Debug.Log("Tower is already at max level!");
            return false;
        }

        int cost = upgradeCosts[currentLevel];
        if (coinManager.CanAfford(cost))
        {
            return true;
        }
        else
        {
            Debug.Log("Not enough resources to upgrade!");
            return false;
        }
    }

    private IEnumerator UpgradeToNextLevel()
    {
        isUpgrading = true;
        spriteRenderer.color = upgradingColor;
        Debug.Log($"Upgrading to level {currentLevel + 1}...");

        coinManager.Buy(upgradeCosts[currentLevel]);

        yield return new WaitForSeconds(upgradeTime);

        currentLevel++;
        Debug.Log($"Upgrade complete! Tower is now level {currentLevel}");

        isUpgrading = false;
        spriteRenderer.color = Color.white;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            anim.SetTrigger("Upgrade");
        }
    }
}
