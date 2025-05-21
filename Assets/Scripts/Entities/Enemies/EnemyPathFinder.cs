using System.Collections.Generic;
using UnityEngine;

public class EnemyPathfinder : MonoBehaviour
{
    public Transform player;
    public GridManager grid;
    public float moveSpeed = 2f;

    private List<Vector2Int> currentPath = new List<Vector2Int>();
    private int pathIndex = 0;

    void Update()
    {
        Vector2Int enemyPos = grid.WorldToGrid(transform.position);
        Vector2Int playerPos = grid.WorldToGrid(player.position);

        // Recalculate path if target moved or we have no path
        if (currentPath.Count == 0 || currentPath[currentPath.Count - 1] != playerPos)
        {
            currentPath = AStarPathFinder.FindPath(enemyPos, playerPos, grid);
            pathIndex = 0;
        }

        // Follow path if valid
        if (currentPath.Count > 1 && pathIndex < currentPath.Count - 1)
        {
            Vector2Int nextStep = currentPath[pathIndex + 1]; // currentPath[0] is the current position
            Vector2 targetWorld = grid.GridToWorld(nextStep);
            MoveTo(targetWorld);

            // Check if we reached the next tile
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
