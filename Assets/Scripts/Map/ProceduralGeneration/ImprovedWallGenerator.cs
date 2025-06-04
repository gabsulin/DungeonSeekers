using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public static class ImprovedWallGenerator
{
    public static void CreateWalls(HashSet<Vector2Int> floorPositions, TilemapVisualizer tilemapVisualizer)
    {
        HashSet<Vector2Int> cleanedFloorPositions = CleanFloorPositions(floorPositions);

        var basicWallPositions = FindWallsInDirections(cleanedFloorPositions, Direction2D.cardinalDirectionsList);
        var cornerWallPositions = FindWallsInDirections(cleanedFloorPositions, Direction2D.diagonalDirectionsList);

        basicWallPositions = RemoveIsolatedWalls(basicWallPositions);
        cornerWallPositions = RemoveIsolatedWalls(cornerWallPositions);

        CreateBasicWalls(tilemapVisualizer, basicWallPositions, cleanedFloorPositions);
        CreateCornerWalls(tilemapVisualizer, cornerWallPositions, cleanedFloorPositions);
    }

    private static HashSet<Vector2Int> CleanFloorPositions(HashSet<Vector2Int> floorPositions)
    {
        HashSet<Vector2Int> cleanedPositions = new HashSet<Vector2Int>(floorPositions);

        List<Vector2Int> toRemove = new List<Vector2Int>();

        foreach (Vector2Int position in cleanedPositions)
        {
            int connectedNeighbors = CountConnectedNeighbours(position, cleanedPositions);
            if (connectedNeighbors == 0)
            {
                toRemove.Add(position);
            }
        }

        foreach (Vector2Int pos in toRemove)
        {
            cleanedPositions.Remove(pos);
        }
        return cleanedPositions;
    }

    private static int CountConnectedNeighbours(Vector2Int position, HashSet<Vector2Int> floorPositions)
    {
        int count = 0;
        foreach(Vector2Int direction in Direction2D.cardinalDirectionsList)
        {
            if(floorPositions.Contains(position + direction)) count++;
        }
        return count;
    }

    private static HashSet<Vector2Int> RemoveIsolatedWalls(HashSet<Vector2Int> wallPositions)
    {
        HashSet<Vector2Int> cleanedWalls = new HashSet<Vector2Int>();

        foreach(Vector2Int wallPosition in wallPositions)
        {
            int neighbouringWalls = CountWallNeighbours(wallPosition, wallPositions);
            if(neighbouringWalls > 0) cleanedWalls.Add(wallPosition);
        }
        return cleanedWalls;
    }

    private static int CountWallNeighbours(Vector2Int position, HashSet<Vector2Int> wallPositions)
    {
        int count = 0;
        foreach (Vector2Int direction in Direction2D.cardinalDirectionsList)
        {
            if (wallPositions.Contains(position + direction))
            {
                count++;
            }
        }
        return count;
    }

    public static HashSet<Vector2Int> FillSmallGaps(HashSet<Vector2Int> floorPositions, int maxGapSize = 2)
    {
        HashSet<Vector2Int> filledPositions = new HashSet<Vector2Int>(floorPositions);

        if (floorPositions.Count == 0) return filledPositions;

        int minX = floorPositions.Min(p => p.x);
        int maxX = floorPositions.Max(p => p.x);
        int minY = floorPositions.Min(p => p.y);
        int maxY = floorPositions.Max(p => p.y);

        for (int x = minX; x <= maxX + 1; x++)
        {
            for (int y = minY; y <= maxY + 1; y++)
            {
                Vector2Int pos = new Vector2Int(x, y);
                if (!floorPositions.Contains(pos))
                {
                    int surroundingFloorTiles = CountConnectedNeighbours(pos, floorPositions);

                    if(surroundingFloorTiles >= 4) filledPositions.Add(pos);
                }
            }
        }
        return filledPositions;
    }

    private static void CreateCornerWalls(TilemapVisualizer tilemapVisualizer, HashSet<Vector2Int> cornerWallPositions, HashSet<Vector2Int> floorPositions)
    {
        foreach (var position in cornerWallPositions)
        {
            string neighboursBinaryType = "";
            foreach (var direction in Direction2D.eightDirectionsList)
            {
                var neighbourPosition = position + direction;
                if (floorPositions.Contains(neighbourPosition))
                {
                    neighboursBinaryType += "1";
                }
                else
                {
                    neighboursBinaryType += "0";
                }
            }
            tilemapVisualizer.PaintSingleCornerWall(position, neighboursBinaryType);
        }
    }
    private static void CreateBasicWalls(TilemapVisualizer tilemapVisualizer, HashSet<Vector2Int> basicWallPositions, HashSet<Vector2Int> floorPositions)
    {
        foreach (var position in basicWallPositions)
        {
            string neighboursBinaryType = "";
            foreach (var direction in Direction2D.cardinalDirectionsList)
            {
                var neighbourPosition = position + direction;
                if (floorPositions.Contains(neighbourPosition))
                {
                    neighboursBinaryType += "1";
                }
                else
                {
                    neighboursBinaryType += "0";
                }
            }
            tilemapVisualizer.PaintSingleBasicWall(position, neighboursBinaryType);
        }
    }

    private static HashSet<Vector2Int> FindWallsInDirections(HashSet<Vector2Int> floorPositions, List<Vector2Int> directionList)
    {
        HashSet<Vector2Int> wallPositions = new HashSet<Vector2Int>();
        foreach (var position in floorPositions)
        {
            foreach (var direction in directionList)
            {
                var neighbourPosition = position + direction;
                if (floorPositions.Contains(neighbourPosition) == false)
                {
                    wallPositions.Add(neighbourPosition);
                }
            }
        }
        return wallPositions;
    }
}
