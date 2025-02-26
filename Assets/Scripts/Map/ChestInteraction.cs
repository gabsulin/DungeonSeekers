using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChestInteraction : MonoBehaviour, IInteractable
{
    public Animator anim;
    [SerializeField] GameObject coinPrefab;
    [SerializeField] Transform coinTarget;

    int minCoins = 5;
    int maxCoins = 15;
    float forceMin = 1;
    float forceMax = 3;

    public void Interact()
    {
        anim.SetBool("ChestOpen", true);
        StartCoroutine(SpawnCoins());
    }

    IEnumerator SpawnCoins()
    {
        int coinsAmount = Random.Range(minCoins, maxCoins);
        yield return new WaitForSeconds(0.85f);

        for (int i = 0; i < coinsAmount; i++)
        {
            GameObject coin = Instantiate(coinPrefab, transform.position, Quaternion.identity);
            Coin coinScript = coin.GetComponent<Coin>();

            if (coinScript != null)
            {
                coinScript.SetTarget(coinTarget);
            }

            Rigidbody2D rb = coin.GetComponent<Rigidbody2D>();
            if (rb == null)
                rb = coin.AddComponent<Rigidbody2D>();

            rb.gravityScale = 1.0f;

            Vector2 randomDirection = Random.insideUnitCircle + Vector2.up;
            float randomForce = Random.Range(forceMin, forceMax);
            rb.AddForce(randomDirection * randomForce, ForceMode2D.Impulse);
        }

        yield return new WaitForSeconds(0.75f);

        foreach (GameObject coin in GameObject.FindGameObjectsWithTag("Coin"))
        {
            Coin coinScript = coin.GetComponent<Coin>();
            if (coinScript != null)
            {
                coinScript.MoveToTarget();
            }
        }
    }
    IEnumerator AnimateCoinMovement(GameObject coin)
    {
        float duration = 1f;

        while (coin != null)
        {
            float elapsedTime = 0f;
            Vector2 startPos = coin.transform.position;
            Vector2 midPos = startPos + new Vector2(0, -0.25f);
            Vector2 endPos = startPos;

            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration;

                if (coin == null)
                    yield break;

                if (t < 0.5f)
                    coin.transform.position = Vector3.Lerp(startPos, midPos, t * 2);
                else
                    coin.transform.position = Vector3.Lerp(midPos, endPos, (t - 0.5f) * 2);

                elapsedTime += Time.deltaTime;
                yield return null;

            }

            if (coin != null)
                coin.transform.position = endPos;
        }
    }

    /*public IEnumerator SpawnWeapon()
    {
        yield return new WaitForSeconds(1);
        Instantiate(weaponPrefab, transform.position, Quaternion.AngleAxis(90, Vector3.forward));
    }*/
}