using System.Collections;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    private PlayerObj player;
    [SerializeField] ParticleSystem boomParticles;
    [SerializeField] int damage;

    private bool hasHitEnemy = false;

    void Start()
    {
        player = FindFirstObjectByType<PlayerObj>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HandleCollision(collision.collider);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        HandleCollision(collider);
    }

    private void HandleCollision(Collider2D collider)
    {
        if (hasHitEnemy) return;

        Weapon weapon = PlayerController.Instance?.GetCurrentWeapon();
        bool isMelee = weapon is Melee;
        var state = player._playerState;

        if ((collider.CompareTag("Enemy") && (state == PlayerObj.PlayerState.attack || state == PlayerObj.PlayerState.stun)) ||
            (collider.CompareTag("MiniBoss") && state == PlayerObj.PlayerState.attack))
        {
            if (collider.CompareTag("MiniBoss"))
            {
                var bossHp = collider.GetComponent<BossHpSystem>();
                if (bossHp != null)
                {
                    hasHitEnemy = true;
                    bossHp.TakeDamage(damage, isMelee);
                    StartCoroutine(ResetHitFlag());
                }
            }
            else
            {
                if (TryDealDamageToEnemy(collider) && gameObject.activeSelf == true)
                {
                    hasHitEnemy = true;
                    StartCoroutine(ResetHitFlag());
                }
            }
        }
    }

    private bool TryDealDamageToEnemy(Component target)
    {
        var spumEnemyHp = target.GetComponent<EnemyHpSystem>();
        if (spumEnemyHp != null)
        {
            spumEnemyHp.TakeDamage(damage);
            return true;
        }

        var enemyHp = target.GetComponent<EnemyHealth>();
        if (enemyHp != null)
        {
            enemyHp.TakeDamage(damage);
            return true;
        }

        return false;
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
