using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHpSystem : MonoBehaviour
{
    private MapFunctionality manager;

    EnemyObj enemy;
    SPUM_Prefabs anim;

    public int maxHealth = 100;
    public int currentHealth;

    public bool stunned;

    int coinsDrop;
    int coinsAmount;

    [SerializeField] GameObject coinPrefab;
    private void Awake()
    {
        currentHealth = maxHealth;
    }
    private void Start()
    {
        enemy = GetComponent<EnemyObj>();
        anim = GetComponentInChildren<SPUM_Prefabs>();

        coinsDrop = Random.Range(0, 4);
        Debug.Log(coinsDrop);
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (enemy._enemyState != EnemyObj.EnemyState.death)
        {
            anim._anim.ResetTrigger("Attack");
            anim._anim.SetFloat("RunState", 0f);
            anim._anim.SetFloat("AttackState", 0f);
            anim._anim.SetFloat("SkillState", 0f);

            enemy._enemyState = EnemyObj.EnemyState.death;

            StartCoroutine(PlayDeathAnimation());
        }

        RemoveFromList();
        if (gameObject != null)
            Destroy(gameObject, 1);

        if(coinsDrop == 3)
        {
            StartCoroutine(DropCoins());
        }
    }

    IEnumerator DropCoins()
    {
        coinsAmount = Random.Range(3, 8);
        for (int i = 0; i < coinsAmount; i++)
        {
            GameObject coin = Instantiate(coinPrefab, transform.position, Quaternion.identity);

            Rigidbody2D rb = coin.GetComponent<Rigidbody2D>();
            if (rb == null)
                rb = coin.AddComponent<Rigidbody2D>();

            rb.gravityScale = 1.0f;

            Vector2 randomDirection = Random.insideUnitCircle + Vector2.up;
            float randomForce = Random.Range(1, 3);
            rb.AddForce(randomDirection * randomForce, ForceMode2D.Impulse);

            yield return new WaitForSeconds(0.25f);

            rb.gravityScale = 0;
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0;
            AnimateCoinMovement(coin);
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

    public void Stun()
    {
        StartCoroutine(StunReset());
    }

    private void RemoveFromList()
    {
        MapFunctionality manager = FindFirstObjectByType<MapFunctionality>();

        if (manager != null && manager.enemies.Contains(this))
        {
            manager.enemies.Remove(this);
        }
    }

    public void SetManager(MapFunctionality manager)
    {
        this.manager = manager;
    }

    private IEnumerator PlayDeathAnimation()
    {
        anim._anim.speed = 1;
        anim.PlayAnimation(2);
        float animationLength = anim._anim.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(animationLength / 2f / anim._anim.speed);

        anim._anim.speed = 1;
        yield return new WaitForSeconds(animationLength / 2f);
    }

    private IEnumerator StunReset()
    {
        stunned = true;
        anim._anim.speed = 0.75f;
        anim.PlayAnimation(3);
        yield return new WaitForSeconds(2);
        anim._anim.speed = 1f;
        stunned = false;
    }
}
