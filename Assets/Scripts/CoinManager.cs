using TMPro;
using UnityEngine;
using DG.Tweening;

public class CoinManager : MonoBehaviour
{
    public static CoinManager instance;
    public TMP_Text coinText;
    private int coinCount = 30;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        coinText.text = coinCount.ToString();
    }

    public void AddCoin(int amount)
    {
        coinCount += amount;
        coinText.text = coinCount.ToString();
    }

    public void Buy(int amount)
    {
        coinCount -= amount;
        coinText.text = coinCount.ToString();
    }
}
