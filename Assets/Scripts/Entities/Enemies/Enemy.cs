using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum EnemyState
{
    Idle,
    Patrol,
    Chase,
    Attack,
    Death
}
public abstract class Enemy : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public GridManager grid;
    public Animator animator;
    public Rigidbody2D rb;

    [Header("Movement Settings")]
    //public float attackRange;

    [Header("Attack Settings")]
    public Collider2D attackHitbox;
    public float attackTimer;
    public float attackCooldown = 1f;
    public int damage = 1;

    public EnemyState currentState = EnemyState.Idle;
    protected EnemyState previousState = EnemyState.Idle;

    protected List<Vector2Int> currentPath = new List<Vector2Int>();
    protected int pathIndex = 0;

    protected Coroutine patrolCoroutine;

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        player = FindFirstObjectByType<PlayerController>().transform;
        grid = FindFirstObjectByType<GridManager>();
        rb = GetComponent<Rigidbody2D>();
    }
    protected virtual void ExecuteIdleState()
    {
        if (animator != null)
        {
            animator.ResetTrigger("Attack");
            animator.SetBool("IsMoving", false);
            animator.SetBool("IsIdle", true);
            animator.SetInteger("MovementType", 0);
        }
    }

    protected virtual void ExecutePatrolState()
    {
        if(animator != null)
        {
            animator.ResetTrigger("Attack");
            animator.SetBool("IsMoving", true);
            animator.SetBool("IsIdle", false);
            animator.SetInteger("MovementType", 1);
        }
        if(patrolCoroutine == null)
        {
            patrolCoroutine = StartCoroutine(Patrol());
        }
    }

    public abstract void Attack();

    protected virtual IEnumerator Patrol()
    {
        while (currentState == EnemyState.Patrol)
        {
            Vector2 goalPos;
            bool isValidDestination = false;
            int attempts = 0;
            const int maxAttempts = 50;

            do
            {
                float randomDistance = Random.Range(3f, 10f);
                float randomAngle = Random.Range(0, 360) * Mathf.Deg2Rad;
                Vector2 randomOffset = new Vector2(
                    Mathf.Cos(randomAngle) * randomDistance,
                    Mathf.Sin(randomAngle) * randomDistance
                );

                goalPos = (Vector2)transform.position + randomOffset;
                Vector2Int gridGoalPos = grid.WorldToGrid(goalPos);

                isValidDestination = grid.IsWalkable(gridGoalPos);

                if (isValidDestination)
                {
                    Vector2Int startPos = grid.WorldToGrid(transform.position);
                    List<Vector2Int> testPath = AStarPathFinder.FindPath(startPos, gridGoalPos, grid, 200);
                    isValidDestination = testPath.Count > 0;
                }

                attempts++;
                yield return new WaitForSeconds(0.01f);
            }
            while (!isValidDestination && attempts < maxAttempts);

            if (!isValidDestination)
            {
                yield return new WaitForSeconds(1f);
                continue;
            }

            Vector2Int start = grid.WorldToGrid(transform.position);
            Vector2Int goal = grid.WorldToGrid(goalPos);

            currentPath = AStarPathFinder.FindPath(start, goal, grid);
            pathIndex = 0;

            while (currentPath.Count > 0 && pathIndex < currentPath.Count && currentState == EnemyState.Patrol)
            {
                if (pathIndex < currentPath.Count)
                {
                    Vector2 nextPos = grid.GridToWorld(currentPath[pathIndex]);
                    //MoveEnemyTowards(nextPos);

                    if (Vector2.Distance(transform.position, nextPos) < 0.05f)
                    {
                        pathIndex++;
                    }
                }

                yield return null;
            }

            if (currentState == EnemyState.Patrol)
            {
                float idleTime = Random.Range(0.5f, 3f);
                if (animator != null)
                {
                    ExecuteIdleState();
                }

                yield return new WaitForSeconds(idleTime);

                if (currentState == EnemyState.Patrol && animator != null)
                {
                    ExecutePatrolState();
                }
            }
        }
        patrolCoroutine = null;
    }
    protected virtual void StopPatrol()
    {
        if (patrolCoroutine != null)
        {
            StopCoroutine(patrolCoroutine);
            patrolCoroutine = null;
        }
    }

    public abstract void OnCollisionEnter2D(Collision2D collision);

    protected virtual void OnDamageDealt()
    {
        //sounds, particles etc
    }

    protected virtual void ActivateAttackHitbox()
    {
        if (attackHitbox != null)
            attackHitbox.enabled = true;
    }

    protected virtual void DeactivateAttackHitbox()
    {
        if (attackHitbox != null)
            attackHitbox.enabled = false;
    }
    protected virtual void OnDrawGizmos()
    {
        // Draw attack range
        Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(new Vector2(transform.position.x, transform.position.y + 0.3f), attackRange);

        // Draw path
        if (currentPath != null && currentPath.Count > 0 && grid != null)
        {
            Gizmos.color = Color.green;

            for (int i = 0; i < currentPath.Count - 1; i++)
            {
                Vector2 current = grid.GridToWorld(currentPath[i]);
                Vector2 next = grid.GridToWorld(currentPath[i + 1]);

                Gizmos.DrawLine(current, next);
                Gizmos.DrawSphere(current, 0.1f);
            }

            if (currentPath.Count > 0)
                Gizmos.DrawSphere(grid.GridToWorld(currentPath[currentPath.Count - 1]), 0.1f);
        }
    }
}
