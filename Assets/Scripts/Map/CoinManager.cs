using TMPro;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public Transform target;
    public static CoinManager instance;
    public TMP_Text coinText;
    private int coinCount; //this is just for testing

    private const string CoinKey = "CoinCount";

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        coinCount=  PlayerPrefs.GetInt(CoinKey, 0);
        UpdateUI();
    }

    public void AddCoin(int amount)
    {
        coinCount += amount;
        GameStats.Instance.AddCoins(amount);
        SaveCoins();
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
            SaveCoins();
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        coinText.text = coinCount.ToString();
    }

    private void SaveCoins()
    {
        PlayerPrefs.SetInt(CoinKey, coinCount);
        PlayerPrefs.Save();
    }
}
