using UnityEngine;

public class DamageToEnemy : MonoBehaviour
{
    [SerializeField] int damage;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            TryDealDamageToEnemy(collision);
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
}
