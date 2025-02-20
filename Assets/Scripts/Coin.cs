using UnityEngine;
using DG.Tweening;
using System.Collections;
using TMPro;

public class Coin : MonoBehaviour
{
    public float moveDuration = 2f;
    public Ease easeType = Ease.InQuad;
    Vector3 targetPosition;
    bool hasStartedMoving = false;

    private void Start()
    {
        CollectCoin();
    }

    void CollectCoin()
    {
        if (CoinManager.instance == null || CoinManager.instance.coinTarget == null)
        {
            Debug.LogError("Coin Target UI is missing!");
            Destroy(gameObject);
            return;
        }

        

        hasStartedMoving = true;

        transform.DOMove(targetPosition, moveDuration)
            .SetEase(easeType)
            .SetDelay(0.5f)
            .OnComplete(() =>
            {
                CoinManager.instance.AddCoin(1);
            });

        GetComponent<Collider2D>().enabled = false;
        GetComponent<Animator>().enabled = false;
    }

    
}
