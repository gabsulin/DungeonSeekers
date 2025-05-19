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
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (hasHitEnemy) return;

        Weapon weapon = PlayerController.Instance?.GetCurrentWeapon();
        bool isMelee = weapon is Melee;

        if(weapon is Melee)
        {
            if (player._playerState == PlayerObj.PlayerState.attack && collision.collider.CompareTag("Enemy") && !hasHitEnemy)
            {
                EnemyHpSystem enemyHp = collision.collider.GetComponent<EnemyHpSystem>();
                if (enemyHp != null)
                {
                    hasHitEnemy = true;
                    enemyHp.TakeDamage(damage);
                    StartCoroutine(ResetHitFlag());

                }
            }
            else if (collision.collider.CompareTag("MiniBoss") && player._playerState == PlayerObj.PlayerState.attack && !hasHitEnemy)
            {
                BossHpSystem bossHp = collision.collider.GetComponent<BossHpSystem>();
                if (bossHp != null)
                {
                    hasHitEnemy = true;
                    bossHp.TakeDamage(damage, isMelee);
                    StartCoroutine(ResetHitFlag());
                }
            }
        } else
        {
            if (collision.collider.CompareTag("Enemy") && !hasHitEnemy)
            {
                EnemyHpSystem enemyHp = collision.collider.GetComponent<EnemyHpSystem>();
                if (enemyHp != null)
                {
                    hasHitEnemy = true;
                    enemyHp.TakeDamage(damage);
                    StartCoroutine(ResetHitFlag());

                }
            }
            else if (collision.collider.CompareTag("MiniBoss") && !hasHitEnemy)
            {
                BossHpSystem bossHp = collision.collider.GetComponent<BossHpSystem>();
                if (bossHp != null)
                {
                    hasHitEnemy = true;
                    bossHp.TakeDamage(damage, isMelee);
                    StartCoroutine(ResetHitFlag());
                }
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

        Weapon weapon = PlayerController.Instance?.GetCurrentWeapon();
        bool isMelee = weapon is Melee;

        if(weapon is Melee)
        {
            if (player._playerState == PlayerObj.PlayerState.attack && collision.CompareTag("Enemy") && !hasHitEnemy)
            {
                EnemyHpSystem enemyHp = collision.GetComponent<EnemyHpSystem>();
                if (enemyHp != null)
                {
                    hasHitEnemy = true;
                    enemyHp.TakeDamage(damage);
                    StartCoroutine(ResetHitFlag());
                    gameObject.SetActive(false);

                }
            }
            else if (collision.CompareTag("MiniBoss") && player._playerState == PlayerObj.PlayerState.attack && !hasHitEnemy)
            {
                BossHpSystem bossHp = collision.GetComponent<BossHpSystem>();
                if (bossHp != null)
                {
                    hasHitEnemy = true;
                    bossHp.TakeDamage(damage, isMelee);
                    StartCoroutine(ResetHitFlag());
                }
            }
        } else
        {
            if (collision.CompareTag("Enemy") && !hasHitEnemy)
            {
                EnemyHpSystem enemyHp = collision.GetComponent<EnemyHpSystem>();
                if (enemyHp != null)
                {
                    hasHitEnemy = true;
                    enemyHp.TakeDamage(damage);
                    StartCoroutine(ResetHitFlag());
                    gameObject.SetActive(false);

                }
            }
            else if (collision.CompareTag("MiniBoss") && !hasHitEnemy)
            {
                BossHpSystem bossHp = collision.GetComponent<BossHpSystem>();
                if (bossHp != null)
                {
                    hasHitEnemy = true;
                    bossHp.TakeDamage(damage, isMelee);
                    StartCoroutine(ResetHitFlag());
                }
            }
        }
        
    }

    private IEnumerator ResetHitFlag()
    {
        yield return new WaitForSeconds(0.35f);
        hasHitEnemy = false;
    }

    public void ResetHit()
    {
        hasHitEnemy = false;
    }
}