using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    private PlayerObj player;
    [SerializeField] ParticleSystem boomParticles;

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

        if (collision.collider.CompareTag("Enemy") && player._playerState == PlayerObj.PlayerState.attack && !hasHitEnemy)
        {
            EnemyHpSystem enemyHp = collision.collider.GetComponent<EnemyHpSystem>();
            if (enemyHp != null)
            {
                hasHitEnemy = true;
                enemyHp.TakeDamage(50);
                StartCoroutine(ResetHitFlag());
                /*boomParticles.transform.position = enemyHp.transform.position;
                boomParticles.Play();*/

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
        yield return new WaitForSeconds(0.5f);
        hasHitEnemy = false;
    }
}




/*private void OnTriggerEnter2D(Collider2D collision)
    {
        //enemy ma taky PlayerObj a tim padem je playerstate na attack a nefunguje podminka tak jak ma
        if (collision.CompareTag("Enemy") && player.objType == PlayerObj.ObjType.Player && player._playerState == PlayerObj.PlayerState.attack)
        {
            EnemyHpSystem enemyHp = collision.GetComponent<EnemyHpSystem>();
            if (enemyHp != null)
            {
                enemyHp.TakeDamage(50);
                boomParticles.transform.position = enemyHp.transform.position;
                boomParticles.Play();
            }
        }
    }*/