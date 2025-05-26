using System.Collections.Generic;
using UnityEngine;

public class EnemyPathfinder : MonoBehaviour
{
    [HideInInspector] public Transform player;
    [HideInInspector] public Animator animator;
    [HideInInspector] public GridManager grid;
    Rigidbody2D rb;
    public float moveSpeed = 2f;
    public float maxDistance = 2f;
    float distance;

    private List<Vector2Int> currentPath = new List<Vector2Int>();
    private int pathIndex = 0;

    private float pathRecalculationTimer = 0f;
    private float pathRecalculationInterval = 0.25f;
    private float attackTimer = 0f;
    private float attackCooldown = 1.5f;

    private void Start()
    {
        player = FindFirstObjectByType<PlayerController>().transform;
        animator = GetComponent<Animator>();
        grid = FindFirstObjectByType<GridManager>();
    }
    void Update()
    {
        distance = Vector2.Distance(transform.position, player.position);
        pathRecalculationTimer -= Time.deltaTime;
        attackCooldown -= Time.deltaTime;

        if (pathRecalculationTimer <= 0f)
        {
            Vector2Int enemyPos = grid.WorldToGrid(transform.position);
            Vector2Int playerPos = grid.WorldToGrid(player.position);

            if (currentPath.Count == 0 ||
                currentPath[currentPath.Count - 1] != playerPos ||
                !currentPath.Contains(enemyPos))
            {
                currentPath = AStarPathFinder.FindPath(enemyPos, playerPos, grid, 1000);

                pathIndex = 0;
                for (int i = 0; i < currentPath.Count; i++)
                {
                    if (currentPath[i] == enemyPos)
                    {
                        pathIndex = i;
                        break;
                    }
                }
            }
            pathRecalculationTimer = pathRecalculationInterval;
        }

        if (currentPath.Count > 1 && pathIndex < currentPath.Count - 1 && distance > maxDistance && animator.GetBool("Die") == false)
        {
            Vector2Int nextStep = currentPath[pathIndex + 1];
            Vector2 targetWorld = grid.GridToWorld(nextStep);
            MoveTo(targetWorld);    

            if (Vector2.Distance(transform.position, targetWorld) < 0.05f)
                pathIndex++;
        }
        else if (currentPath.Count > 1 && pathIndex < currentPath.Count - 1 && distance <= maxDistance && attackCooldown <= 0f)
        {
            AttackPlayer();
        }
    }

    void MoveTo(Vector2 target)
    {
        animator.SetBool("IsMoving", true);
        rb.MovePosition(Vector2.MoveTowards(rb.position, target, moveSpeed * Time.deltaTime));
    }

    void AttackPlayer()
    {
        animator.SetTrigger("Attack");
        animator.SetBool("IsMoving", false);
        //animator.SetBool("IsIdle", false);
        attackCooldown = 1.5f;
    }

    void OnDrawGizmos()
    {
        if (currentPath == null || currentPath.Count == 0)
            return;

        Gizmos.color = Color.red;

        for (int i = 0; i < currentPath.Count; i++)
        {
            Vector2 worldPos = grid.GridToWorld(currentPath[i]);
            Gizmos.DrawSphere(worldPos, 0.1f);

            if (i < currentPath.Count - 1)
            {
                Vector2 nextWorld = grid.GridToWorld(currentPath[i + 1]);
                Gizmos.DrawLine(worldPos, nextWorld);
            }
        }

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, player.position);
    }
}
