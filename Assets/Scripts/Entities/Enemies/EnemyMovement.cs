using System.Collections;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public enum EnemyType
    {
        Melee,
        Magic,
        Bow
    }

    [SerializeField] private EnemyType enemyType;

    private PlayerObj player;
    private EnemyObj enemy;
    private EnemyHpSystem enemyHp;
    private PlayerHpSystem playerHp;
    private SPUM_Prefabs anim;
    private RangedEnemyAttack rangedEnemyAttack;
    private MeleeEnemyAttack meleeEnemyAttack;
    private BossFlip flip;

    [HideInInspector] public float distance;

    [SerializeField] public float attackThreshold;
    [SerializeField] public float moveThreshold;

    private Coroutine patrolCoroutine;

    void Start()
    {
        player = FindAnyObjectByType<PlayerObj>();
        enemy = GetComponent<EnemyObj>();
        anim = GetComponentInChildren<SPUM_Prefabs>();
        enemyHp = GetComponent<EnemyHpSystem>();
        playerHp = FindAnyObjectByType<PlayerHpSystem>();
        rangedEnemyAttack = GetComponent<RangedEnemyAttack>();
        meleeEnemyAttack = GetComponent<MeleeEnemyAttack>();
        flip = GetComponent<BossFlip>();
    }

    void Update()
    {
        if (player != null)
        {
            distance = Vector2.Distance(transform.position, player.transform.position);
            flip.LookAtPlayer();
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
        bool canSeePlayer = (rangedEnemyAttack != null && rangedEnemyAttack.CanSeePlayer()) ||
                            (meleeEnemyAttack != null && meleeEnemyAttack.CanSeePlayer());

        if (distance > moveThreshold && !enemyHp.stunned)
        {
            if (patrolCoroutine == null)
            {
                patrolCoroutine = StartCoroutine(Patrol());
            }
        }
        else if (distance > attackThreshold && distance <= moveThreshold && playerHp.currentHp > 0 && !enemyHp.stunned && canSeePlayer)
        {
            StopPatrol();
            MoveToPlayer();
        }
        else if (distance <= attackThreshold && playerHp.currentHp > 0 && !enemyHp.stunned && canSeePlayer)
        {
            StopPatrol();
            AttackPlayer();
        }
        else if (!enemyHp.stunned && !canSeePlayer)
        {
            if (patrolCoroutine == null)
            {
                patrolCoroutine = StartCoroutine(Patrol());
            }
        }
        else if (enemyHp.stunned)
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

        switch (enemyType)
        {
            case EnemyType.Melee:
                anim.PlayAnimation(4);
                break;
            case EnemyType.Bow:
                anim.PlayAnimation(5);
                break;
            case EnemyType.Magic:
                anim.PlayAnimation(6);
                break;
        }
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

                isPathClear = IsValidPosition(goalPos) &&
                    Physics2D.Linecast(transform.position, goalPos, LayerMask.GetMask("Collision")) == false;

                yield return new WaitForSeconds(0.1f);
                attempts++;
            }
            while (!isPathClear && enemyHp.currentHealth > 0 && attempts < maxAttempts);

            if (attempts == maxAttempts)
            {
                enemy.SetMovePos(Vector2.zero);
                yield return new WaitForSeconds(2f);
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
        return !Physics2D.OverlapPoint(pos, LayerMask.GetMask("Collision"));
    }
}
