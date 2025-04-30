using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    private PlayerObj player;
    [SerializeField] ParticleSystem boomParticles;
    [SerializeField] int damage;

    private bool hasHitEnemy = false;
    void Start()
    {
        player = GetComponentInParent<PlayerObj>();
        if(player == null)
        {
            player = FindFirstObjectByType<PlayerObj>();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (hasHitEnemy) return;

        if (player._playerState == PlayerObj.PlayerState.attack && collision.collider.CompareTag("Enemy") && !hasHitEnemy)
        {
            EnemyHpSystem enemyHp = collision.collider.GetComponent<EnemyHpSystem>();
            if (enemyHp != null)
            {
                hasHitEnemy = true;
                enemyHp.TakeDamage(damage);
                StartCoroutine(ResetHitFlag());

            }
        } else if(collision.collider.CompareTag("MiniBoss") && player._playerState == PlayerObj.PlayerState.attack && !hasHitEnemy)
        {
            BossHpSystem bossHp = collision.collider.GetComponent<BossHpSystem>();
            if (bossHp != null)
            {
                hasHitEnemy = true;
                bossHp.TakeDamage(damage);
                StartCoroutine(ResetHitFlag());
                Debug.Log("hit");
            }
        }

        if(collision.collider.CompareTag("Enemy") && player._playerState == PlayerObj.PlayerState.stun && !hasHitEnemy)
        {
            EnemyHpSystem enemyHp = collision.collider.GetComponent<EnemyHpSystem>();
            if (enemyHp != null)
            {
                hasHitEnemy = true;
                enemyHp.Stun();
                StartCoroutine(ResetHitFlag());

            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasHitEnemy) return;

        if (player._playerState == PlayerObj.PlayerState.attack && collision.CompareTag("Enemy") && !hasHitEnemy)
        {
            EnemyHpSystem enemyHp = collision.GetComponent<EnemyHpSystem>();
            if (enemyHp != null)
            {
                hasHitEnemy = true;
                enemyHp.TakeDamage(damage);
                StartCoroutine(ResetHitFlag());
                Destroy(gameObject);

            }
        }
        else if (collision.CompareTag("MiniBoss") && player._playerState == PlayerObj.PlayerState.attack && !hasHitEnemy)
        {
            BossHpSystem bossHp = collision.GetComponent<BossHpSystem>();
            if (bossHp != null)
            {
                hasHitEnemy = true;
                bossHp.TakeDamage(damage);
                StartCoroutine(ResetHitFlag());
                Debug.Log("hit");
            }
        }
    }

    private IEnumerator ResetHitFlag()
    {
        yield return new WaitForSeconds(0.35f);
        hasHitEnemy = false;
    }
}