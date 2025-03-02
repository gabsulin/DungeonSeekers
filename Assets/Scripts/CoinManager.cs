using TMPro;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public static CoinManager instance;
    public TMP_Text coinText;
    private int coinCount = 30;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        UpdateUI();
    }

    public void AddCoin(int amount)
    {
        coinCount += amount;
        UpdateUI();
    }

    public bool CanAfford(int amount)
    {
        return coinCount >= amount;
    }

    public void Buy(int amount)
    {
        if (CanAfford(amount))
        {
            coinCount -= amount;
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        coinText.text = coinCount.ToString();
    }
}
