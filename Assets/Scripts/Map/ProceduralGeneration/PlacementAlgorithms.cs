using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public static class PlacementAlgorithms
{
    //finds valid positions for object placement using a simple grid based approach
    public static List<Vector2Int> FindValidPositions(
        HashSet<Vector2Int> floorPositions,
        HashSet<Vector2Int> wallPositions,
        HashSet<Vector2Int> occupiedPositions,
        ObjectPlacementData objectData,
        ObjectPlacementSettings settings)
    {
        List<Vector2Int> validPositions = new List<Vector2Int>();

        foreach (Vector2Int floorPos in floorPositions)
        {
            if (IsValidPlacementPosition(floorPos, floorPositions, wallPositions, occupiedPositions, objectData, settings))
            {
                validPositions.Add(floorPos);
            }
        }
        return validPositions;
    }

    //checks if a specific position is valid for object placement
    public static bool IsValidPlacementPosition(
        Vector2Int position,
        HashSet<Vector2Int> floorPositions,
        HashSet<Vector2Int> wallPositions,
        HashSet<Vector2Int> occupiedPositions,
        ObjectPlacementData objectData,
        ObjectPlacementSettings settings)
    {
        //must be on a floor tile, must not be occupied, must have atleast min distance from walls and objects and must not block ciritical paths
        if (!floorPositions.Contains(position)) return false;
        if (occupiedPositions.Contains(position)) return false;
        if (!objectData.canPlaceNearWalls && !IsMinimumDistanceFromWalls(position, wallPositions, objectData.minDistanceFromWalls)) return false;
        if (!IsMinimumDistanceFromObjects(position, occupiedPositions, objectData.minDistanceBetweenObjects)) return false;
        if (settings.ensureRoomConnectivity && objectData.blockMovement)
            if (WouldBlockCriticalPath(position, floorPositions, wallPositions, occupiedPositions, settings)) return false;

        return true;
    }

    //checks if positions maintain min distance from walls
    private static bool IsMinimumDistanceFromWalls(Vector2Int position, HashSet<Vector2Int> wallPositions, int minDistance)
    {
        for (int x = -minDistance; x <= minDistance; x++)
        {
            for (int y = -minDistance; y <= minDistance; y++)
            {
                Vector2Int checkPos = position + new Vector2Int(x, y);
                if (wallPositions.Contains(checkPos)) return false;
            }
        }
        return true;
    }

    //checks if positions maintain min distance from other objects
    private static bool IsMinimumDistanceFromObjects(Vector2Int position, HashSet<Vector2Int> occupiedPositions, int minDistance)
    {
        for (int x = -minDistance; x <= minDistance; x++)
        {
            for (int y = -minDistance; y <= minDistance; y++)
            {
                Vector2Int checkPos = position + new Vector2Int(x, y);
                if (occupiedPositions.Contains(checkPos)) return false;
            }
        }
        return true;
    }

    //checks if placing an object at this position would block critical paths
    private static bool WouldBlockCriticalPath(
        Vector2Int position,
        HashSet<Vector2Int> floorPositions,
        HashSet<Vector2Int> wallPositions,
        HashSet<Vector2Int> occupiedPositions,
        ObjectPlacementSettings settings)
    {
        HashSet<Vector2Int> tempOccupied = new HashSet<Vector2Int>(occupiedPositions);
        tempOccupied.Add(position);

        return !IsRoomConnected(floorPositions, tempOccupied, settings.minPathWidth);
    }

    //checks if the room remains connected
    private static bool IsRoomConnected(HashSet<Vector2Int> floorPositions, HashSet<Vector2Int> blockedPositions, int minPathWidth)
    {
        if (floorPositions.Count == 0) return true;

        HashSet<Vector2Int> availablePositions = new HashSet<Vector2Int>(floorPositions);
        foreach (Vector2Int blocked in blockedPositions)
        {
            availablePositions.Remove(blocked);
        }

        if (availablePositions.Count == 0) return false;

        //start filling from first available position
        Vector2Int startPos = availablePositions.First();
        HashSet<Vector2Int> visited = new HashSet<Vector2Int>();
        Queue<Vector2Int> queue = new Queue<Vector2Int>();

        queue.Enqueue(startPos);
        visited.Add(startPos);

        while (queue.Count > 0)
        {
            Vector2Int current = queue.Dequeue();

            foreach(Vector2Int direction in Direction2D.cardinalDirectionsList)
            {
                Vector2Int neighbour = current + direction;
                if(availablePositions.Contains(neighbour) && !visited.Contains(neighbour))
                {
                    visited.Add(neighbour);
                    queue.Enqueue(neighbour);
                }
            }
        }
        float reachabilityRatio = (float)visited.Count / availablePositions.Count;
        return reachabilityRatio > 0.8;
    }

    //analyzes room structure
    public static List<HashSet<Vector2Int>> AnalyzeRoomStructure(HashSet<Vector2Int> floorPositions)
    {
        List<HashSet<Vector2Int>> rooms = new List<HashSet<Vector2Int>>();
        HashSet<Vector2Int> unprocessed = new HashSet<Vector2Int>(floorPositions);

        while (unprocessed.Count > 0)
        {
            Vector2Int start = unprocessed.First();
            HashSet<Vector2Int> room = FloodFillRoom(start, unprocessed);

            if(room.Count > 0)
            {
                rooms.Add(room);
                foreach (Vector2Int pos in room)
                {
                    unprocessed.Remove(pos);
                }
            }
        }
        return rooms;
    }

    //flood fills a connected room area
    private static HashSet<Vector2Int> FloodFillRoom(Vector2Int start, HashSet<Vector2Int> availablePositions)
    {
        HashSet<Vector2Int> room = new HashSet<Vector2Int>();
        Queue<Vector2Int> queue = new Queue<Vector2Int>();

        if(!availablePositions.Contains(start)) return room;

        queue.Enqueue(start);
        room.Add(start);

        while (queue.Count > 0)
        {
            Vector2Int current = queue.Dequeue();

            foreach (Vector2Int direction in Direction2D.cardinalDirectionsList)
            {
                Vector2Int neighbour = current + direction;

                if(availablePositions.Contains(neighbour) && !room.Contains(neighbour))
                {
                    room.Add(neighbour);
                    queue.Enqueue(neighbour);
                } 
            }
        }
        return room;
    }

    //select positions for object placement
    public static List<Vector2Int> SelectPlacementPositions(
        List<Vector2Int> validPositions,
        ObjectPlacementData objectData,
        ObjectPlacementSettings settings,
        int roomSize)
    {
        List<Vector2Int> selectedPositions = new List<Vector2Int>();

        if (validPositions.Count == 0) return selectedPositions;

        //calculate target number of objects based how big the room is and the density
        int maxObjects = Mathf.FloorToInt(roomSize * objectData.maxDensityPerRoom);
        int targetObjects = Mathf.FloorToInt(maxObjects * settings.globalObjectDensity);

        foreach (Vector2Int position in validPositions)
        {
            if (selectedPositions.Count >= targetObjects) break;

            if(Random.value <= objectData.spawnChance)
            {
                selectedPositions.Add(position);
            }
        }
        return selectedPositions;
    }
}