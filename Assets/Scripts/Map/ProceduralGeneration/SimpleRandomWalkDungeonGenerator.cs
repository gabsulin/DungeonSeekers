using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using UnityEngine;

public class SimpleRandomWalkDungeonGenerator : AbstractDungeonGenerator
{
    [SerializeField] private SimpleRandomWalkSO randomWalkParameters;

    protected override void RunProceduralGeneration()
    {
        HashSet<Vector2Int> floorPositions = RunRandomWalk(randomWalkParameters, startPosition);

        tilemapVisualizer.Clear();
        tilemapVisualizer.PaintFloorTiles(floorPositions);
        HashSet<Vector2Int> wallPositions = GetWallPositions(floorPositions);
        WallGenerator.CreateWalls(floorPositions, tilemapVisualizer);

        if (objectGenerator != null && enableObjectGeneration)
        {
            objectGenerator.ClearObjects();
            objectGenerator.GenerateObjects(floorPositions, wallPositions);
        }
    }
    protected HashSet<Vector2Int> RunRandomWalk(SimpleRandomWalkSO parameters, Vector2Int position)
    {
        var currentPosition = startPosition;
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();

        for (int i = 0; i < parameters.iterations; i++)
        {
            var path = ProceduralGenerationAlgorithms.SimpleRandomWalk(currentPosition, parameters.walkLength);
            floorPositions.UnionWith(path);

            if (parameters.startRandomlyEachIteration)
                currentPosition = floorPositions.ElementAt(Random.Range(0, floorPositions.Count));
        }

        return floorPositions;
    }
    private HashSet<Vector2Int> GetWallPositions(HashSet<Vector2Int> floorPositions)
    {
        HashSet<Vector2Int> wallPositions = new HashSet<Vector2Int>();

        foreach (Vector2Int floorPos in floorPositions)
        {
            foreach (Vector2Int direction in Direction2D.cardinalDirectionsList)
            {
                Vector2Int neighborPos = floorPos + direction;
                if (!floorPositions.Contains(neighborPos))
                {
                    wallPositions.Add(neighborPos);
                }
            }
        }

        return wallPositions;
    }
}
