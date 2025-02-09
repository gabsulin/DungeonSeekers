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
                /*boomParticles.transform.position = enemyHp.transform.position;
                boomParticles.Play();*/

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
                enemyHp.Stun(10);
                StartCoroutine(ResetHitFlag());
                /*boomParticles.transform.position = enemyHp.transform.position;
                boomParticles.Play();*/

            }
        }
    }

    private IEnumerator ResetHitFlag()
    {
        yield return new WaitForSeconds(0.35f);
        hasHitEnemy = false;
    }
}