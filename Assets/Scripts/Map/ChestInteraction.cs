using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChestInteraction : MonoBehaviour, IInteractable
{
    public Animator anim;
    [SerializeField] GameObject coinPrefab;
    [SerializeField] Transform coinTarget;

    bool isOpen = false;

    int minCoins = 5;
    int maxCoins = 15;
    float forceMin = 1.5f;
    float forceMax = 2;

    public void Interact()
    {
        if (isOpen) return;
        isOpen = true;
        anim.SetBool("ChestOpen", true);
        StartCoroutine(SpawnCoins());
        AudioManager.Instance.PlaySFX("ChestOpening");
    }

    IEnumerator SpawnCoins()
    {
        int coinsAmount = Random.Range(minCoins, maxCoins);
        yield return new WaitForSeconds(0.85f);

        List<Rigidbody2D> coinRigidbodies = new List<Rigidbody2D>();
        List<Coin> coinObjects = new List<Coin>();

        for (int i = 0; i < coinsAmount; i++)
        {
            AudioManager.Instance.PlaySFX("ChestCoin");
            GameObject coin = Instantiate(coinPrefab, transform.position, Quaternion.identity);
            Coin coinObj = coin.GetComponent<Coin>();

            if (coinObj != null)
            {
                coinObj.SetTarget(coinTarget);
                coinObjects.Add(coinObj);
            }

            Rigidbody2D rb = coin.GetComponent<Rigidbody2D>();
            if (rb == null)
                rb = coin.AddComponent<Rigidbody2D>();

            rb.gravityScale = 1.0f;
            coinRigidbodies.Add(rb);

            Vector2 randomDirection = Random.insideUnitCircle + Vector2.up;
            float randomForce = Random.Range(forceMin, forceMax);
            rb.AddForce(randomDirection * randomForce, ForceMode2D.Impulse);
            StartCoroutine(DisablePhysicsWithDelay(rb, coinObj, Random.Range(0.3f, 1.0f)));
        }
    }

    IEnumerator DisablePhysicsWithDelay(Rigidbody2D rb, Coin coin, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (rb != null)
        {
            rb.gravityScale = 0;
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0;
        }

        if (coin != null)
        {
            coin.StartFloatingAnimation();
        }
    }

}