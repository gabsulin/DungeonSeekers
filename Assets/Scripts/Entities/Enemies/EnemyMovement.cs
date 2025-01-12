using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.VersionControl;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    PlayerObj player;
    EnemyObj enemy;
    EnemyHpSystem enemyHp;
    PlayerHpSystem playerHp;
    SPUM_Prefabs anim;
    RangedEnemyAttack enemyAttack;

    public float distance;

    private const float IdleThreshold = 8f;
    private const float AttackThreshold = 8f;
    private const float MoveThreshold = 10f;

    private Coroutine patrolCoroutine;

    void Start()
    {
        player = FindAnyObjectByType<PlayerObj>();
        enemy = GetComponent<EnemyObj>();
        anim = GetComponentInChildren<SPUM_Prefabs>();
        enemyHp = GetComponent<EnemyHpSystem>();
        playerHp = FindAnyObjectByType<PlayerHpSystem>();
        enemyAttack = GetComponent<RangedEnemyAttack>();
    }

    void Update()
    {
        if (player != null)
        {
            distance = Vector2.Distance(transform.position, player.transform.position);
        }

        if (enemy != null && player != null)
        {
            if (enemyHp.currentHealth > 0)
            {
                EnemyBehavior();
            }
            if (playerHp.currentHp <= 0)
            {
                Idle();
            }
        }
    }

    private void EnemyBehavior()
    {
        if (distance > MoveThreshold && enemyHp.stunned == false)
        {
            if (patrolCoroutine == null)
            {
                patrolCoroutine = StartCoroutine(Patrol());
            }
        }
        else if (distance > IdleThreshold && distance <= MoveThreshold && enemyAttack.CanSeePlayer() && enemyHp.stunned == false)
        {
            StopPatrol();
            MoveToPlayer();
        }
        else if (distance <= AttackThreshold && playerHp.currentHp > 0 && enemyAttack.CanSeePlayer() && enemyHp.stunned == false)
        {
            StopPatrol();
            AttackPlayer();
        }
        else if (!enemyAttack.CanSeePlayer() && enemyHp.stunned == false)
        {
            if (patrolCoroutine == null)
            {
                patrolCoroutine = StartCoroutine(Patrol());
            }
        }
        else if (enemyHp.stunned == true)
        {
            StopPatrol();
            Stun();
        }
    }

    private void MoveToPlayer()
    {
        Vector2 goalPos = player.transform.position;

        if (IsValidPosition(goalPos))
        {
            enemy.SetMovePos(goalPos);
        }
        else
        {
            if (patrolCoroutine == null)
            {
                patrolCoroutine = StartCoroutine(Patrol());
            }
        }

        if (enemy._enemyState != EnemyObj.EnemyState.move)
        {
            enemy._enemyState = EnemyObj.EnemyState.move;
            anim.PlayAnimation(1);
        }
    }

    private void AttackPlayer()
    {
        enemy._enemyState = EnemyObj.EnemyState.attack;
        anim.PlayAnimation(6);
    }

    private void Idle()
    {
        enemy._enemyState = EnemyObj.EnemyState.idle;

        anim._anim.ResetTrigger("Attack");
        anim._anim.SetFloat("RunState", 0f);
        anim._anim.SetFloat("AttackState", 0f);
        anim._anim.SetFloat("SkillState", 0f);

        anim.PlayAnimation(0);
    }

    private void Stun()
    {
        enemy._enemyState = EnemyObj.EnemyState.stun;
        anim.PlayAnimation(3);

        anim._anim.ResetTrigger("Attack");
        anim._anim.SetFloat("RunState", 1f);
        anim._anim.SetFloat("AttackState", 0f);
        anim._anim.SetFloat("SkillState", 0f);

    }

    private IEnumerator Patrol()
    {
        StopPatrol();

        if (enemy._enemyState != EnemyObj.EnemyState.move)
        {
            anim._anim.ResetTrigger("Attack");
            anim._anim.SetFloat("AttackState", 0f);
            anim._anim.SetFloat("SkillState", 0f);

            enemy._enemyState = EnemyObj.EnemyState.move;
        }
        Vector2 goalPos;
        bool isPathClear;
        const int maxAttempts = 500;
        while (playerHp.currentHp > 0)
        {
            int attempts = 0;

            do
            {
                int randomX = Random.Range(-10, 10);
                int randomY = Random.Range(-10, 10);
                goalPos = new Vector2(randomX, randomY);

                isPathClear = IsValidPosition(goalPos) && Physics2D.Linecast(transform.position, goalPos, LayerMask.GetMask("Collision")) == false;
                yield return new WaitForSeconds(0.1f);

                attempts += 1;
            }
            while (!isPathClear && enemyHp.currentHealth > 0 && attempts < maxAttempts);

            if (maxAttempts == attempts)
            {
                enemy.SetMovePos(Vector2.zero);
                yield return new WaitForSeconds(2);
                continue;
            }

            enemy.SetMovePos(goalPos);

            float idleTime = Random.Range(0.5f, 4f);
            yield return new WaitForSeconds(idleTime);
        }
        
    }

    private void StopPatrol()
    {
        if (patrolCoroutine != null)
        {
            StopCoroutine(patrolCoroutine);
            patrolCoroutine = null;
        }
    }

    private bool IsValidPosition(Vector2 pos)
    {
        if (Physics2D.OverlapPoint(pos, LayerMask.GetMask("Collision")))
        {
            return false;
        }
        return true;
    }
}
