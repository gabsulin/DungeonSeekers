using System.Collections.Generic;
using UnityEngine;
public class BossMove : StateMachineBehaviour
{
    [Header("Pathfinding")]
    List<Vector2Int> currentPath = new List<Vector2Int>();
    int pathIndex = 0;
    GridManager grid;
    float pathRecalculationTimer = 0f;
    float pathRecalculationInterval = 0.25f;

    Transform bossTransform;

    public float speed = 2.5f;
    public float attackRange = 15f;

    Transform player;
    Rigidbody2D rb;
    BossFlip boss;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = animator.GetComponent<Rigidbody2D>();
        boss = animator.GetComponent<BossFlip>();
        grid = FindFirstObjectByType<GridManager>();
        bossTransform = animator.transform;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss.LookAtPlayer();
        float distance = Vector2.Distance(player.position, rb.position);

        pathRecalculationTimer -= Time.deltaTime;

        if (pathRecalculationTimer <= 0)
        {
            Vector2Int bossGridPos = grid.WorldToGrid(bossTransform.position);
            Vector2Int playerGridPos = grid.WorldToGrid(player.position);

            currentPath = AStarPathFinder.FindPath(bossGridPos, playerGridPos, grid);
            pathIndex = 0;

            for (int i = 0; i < currentPath.Count; i++)
            {
                if (currentPath[i] == bossGridPos)
                {
                    pathIndex = i;
                    break;
                }
            }
            pathRecalculationTimer = pathRecalculationInterval;
        }
        if (distance <= attackRange)
        {
            animator.SetTrigger("Attack");
            return;
        }
        if (currentPath.Count > 1 && pathIndex < currentPath.Count - 1)
        {
            Vector2Int nextStep = currentPath[pathIndex + 1];
            Vector2 targetWorld = grid.GridToWorld(nextStep);
            Vector2 newPos = Vector2.MoveTowards(rb.position, targetWorld, speed * Time.deltaTime);
            rb.MovePosition(newPos);

            if (Vector2.Distance(rb.position, targetWorld) < 0.05f)
            {
                pathIndex++;
            }
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack");
    }
}