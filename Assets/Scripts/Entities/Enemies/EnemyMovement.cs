using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public enum EnemyType { Melee, Magic, Bow }

    [SerializeField] private EnemyType enemyType;

    private Transform player;
    private EnemyObj enemy;
    private EnemyHpSystem enemyHp;
    private PlayerHpSystem playerHp;
    private SPUM_Prefabs anim;
    private RangedEnemyAttack rangedEnemyAttack;
    private MeleeEnemyAttack meleeEnemyAttack;
    private BossFlip flip;
    private Rigidbody2D rb;
    public float speed;

    [HideInInspector] public float distance;

    [SerializeField] public float attackThreshold;
    [SerializeField] public float moveThreshold;

    // 🔄 Pathfinding
    private GridManager grid;
    private List<Vector2Int> currentPath = new List<Vector2Int>();
    private int pathIndex = 0;
    private float pathRecalculationTimer = 0f;
    private float pathRecalculationInterval = 0.25f;

    private void Start()
    {
        player = FindAnyObjectByType<PlayerController>().transform;
        enemy = GetComponent<EnemyObj>();
        anim = GetComponentInChildren<SPUM_Prefabs>();
        enemyHp = GetComponent<EnemyHpSystem>();
        playerHp = FindAnyObjectByType<PlayerHpSystem>();
        rangedEnemyAttack = GetComponent<RangedEnemyAttack>();
        meleeEnemyAttack = GetComponent<MeleeEnemyAttack>();
        flip = GetComponent<BossFlip>();
        grid = FindAnyObjectByType<GridManager>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (player == null || enemy == null || playerHp == null || grid == null || enemyHp.currentHealth <= 0)
            return;

        distance = Vector2.Distance(transform.position, player.transform.position);
        flip.LookAtPlayer();

        pathRecalculationTimer -= Time.deltaTime;

        if (pathRecalculationTimer <= 0f)
        {
            Vector2Int enemyPos = grid.WorldToGrid(transform.position);
            Vector2Int playerPos = grid.WorldToGrid(player.position);

            currentPath = AStarPathFinder.FindPath(enemyPos, playerPos, grid);
            pathIndex = 0;

            for (int i = 0; i < currentPath.Count; i++)
            {
                if (currentPath[i] == enemyPos)
                {
                    pathIndex = i;
                    break;
                }
            }

            pathRecalculationTimer = pathRecalculationInterval;
        }
        if (currentPath.Count > 1 && pathIndex < currentPath.Count - 1 && distance > attackThreshold && anim._anim.GetBool("Die") == false)
        {
            Vector2Int nextStep = currentPath[pathIndex + 1];
            Vector2 targetWorld = grid.GridToWorld(nextStep);
            MoveTo(targetWorld);

            if (Vector2.Distance(transform.position, targetWorld) < 0.05f)
                pathIndex++;
        }
        //HandleBehavior();
    }
    void MoveTo(Vector2 target)
    {
        rb.MovePosition(Vector2.MoveTowards(rb.position, target, speed * Time.deltaTime));
    }
    private void HandleBehavior()
    {
        bool canSeePlayer = (rangedEnemyAttack && rangedEnemyAttack.CanSeePlayer()) ||
                            (meleeEnemyAttack && meleeEnemyAttack.CanSeePlayer());

        if (playerHp.currentHp <= 0)
        {
            Idle();
            return;
        }

        if (distance <= attackThreshold)
        {
            AttackPlayer();
        }
        else if (distance <= moveThreshold)
        {
            MoveAlongPath();
        }
        else
        {
            Idle();
        }
    }

    private void MoveAlongPath()
    {
        if (currentPath.Count > 1 && pathIndex < currentPath.Count - 1)
        {
            Vector2Int nextStep = currentPath[pathIndex + 1];
            Vector2 targetWorld = grid.GridToWorld(nextStep);

            if (IsValidPosition(targetWorld))
            {
                transform.position = Vector2.MoveTowards(transform.position, targetWorld, speed * Time.deltaTime);

                if (enemy._enemyState != EnemyObj.EnemyState.move)
                {
                    enemy._enemyState = EnemyObj.EnemyState.move;
                    anim.PlayAnimation(1); // Run animation
                }

                if (Vector2.Distance(transform.position, targetWorld) < 0.05f)
                {
                    pathIndex++;
                }
            }
        }
    }

    private void AttackPlayer()
    {
        enemy._enemyState = EnemyObj.EnemyState.attack;

        switch (enemyType)
        {
            case EnemyType.Melee:
                anim.PlayAnimation(4); // Melee attack
                break;
            case EnemyType.Bow:
                anim.PlayAnimation(5); // Bow attack
                break;
            case EnemyType.Magic:
                anim.PlayAnimation(6); // Magic attack
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

        anim.PlayAnimation(0); // Idle
    }

    private bool IsValidPosition(Vector2 pos)
    {
        return !Physics2D.OverlapPoint(pos, LayerMask.GetMask("Collision"));
    }
}
