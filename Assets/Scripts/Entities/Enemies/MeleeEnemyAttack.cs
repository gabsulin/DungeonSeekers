using System.Collections;
using UnityEngine;

public class MeleeEnemyAttack : MonoBehaviour
{
    EnemyMovement enemyDistance;
    EnemyHpSystem enemyHp;
    PlayerHpSystem playerHp;

    Transform aimTarget;

    Coroutine attackingRoutine;

    private void Awake()
    {
        attackingRoutine = null;
    }
    void Start()
    {
        enemyDistance = GetComponent<EnemyMovement>();
        enemyHp = GetComponent<EnemyHpSystem>();
        playerHp = FindFirstObjectByType<PlayerHpSystem>();

        if (playerHp != null)
        {
            Transform aimTargetTransform = playerHp.transform;
            aimTarget = aimTargetTransform.Find("AimTarget");
        }
    }

    private void Update()
    {
        if (aimTarget != null)
        {
            StartAttacking();
        }
    }

    public void StartAttacking()
    {
        if (attackingRoutine == null && enemyHp.currentHealth > 0 && playerHp.currentHp > 0)
        {
            attackingRoutine = StartCoroutine(AttackingRoutine());
        }
    }

    public void StopAttacking()
    {
        if (attackingRoutine != null)
        {
            StopCoroutine(attackingRoutine);
            attackingRoutine = null;
        }
    }

    public IEnumerator AttackingRoutine()
    {
        StopAttacking();

        while (enemyHp.currentHealth > 0)
        {
            if (playerHp.currentHp > 0 && CanSeePlayer() && enemyHp.stunned == false)
            {
                Attack();
            }
            yield return new WaitForSeconds(0.75f);
        }
        attackingRoutine = null;
    }

    public bool CanSeePlayer()
    {
        RaycastHit2D[] hits = Physics2D.LinecastAll(transform.position, aimTarget.position);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Collision"))
            {
                return false;
            }
        }
        return true;
    }


    private void Attack()
    {
        
    }
}
