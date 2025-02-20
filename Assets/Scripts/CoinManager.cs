using TMPro;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public static CoinManager instance;
    public TMP_Text coinText;
    private int coinCount = 0;
    public Transform coinTarget;

    private void Awake()
    {
        instance = this;
    }

    public void AddCoin(int amount)
    {
        coinCount += amount;
        coinText.text = coinCount.ToString();
    }
}
