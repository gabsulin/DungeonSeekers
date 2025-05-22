using System.Collections.Generic;
using UnityEngine;

public class EnemyPathfinder : MonoBehaviour
{
    public Transform player;
    public GridManager grid;
    public float moveSpeed = 2f;

    private List<Vector2Int> currentPath = new List<Vector2Int>();
    private int pathIndex = 0;

    private float pathRecalculationTimer = 0f;
    private float pathRecalculationInterval = 0.5f;

    void Update()
    {
        pathRecalculationTimer -= Time.deltaTime;

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

        if (currentPath.Count > 1 && pathIndex < currentPath.Count - 1)
        {
            Vector2Int nextStep = currentPath[pathIndex + 1];
            Vector2 targetWorld = grid.GridToWorld(nextStep);
            MoveTo(targetWorld);

            if (Vector2.Distance(transform.position, targetWorld) < 0.05f)
                pathIndex++;
        }
    }

    void MoveTo(Vector2 target)
    {
        transform.position = Vector2.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
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
    }
}
