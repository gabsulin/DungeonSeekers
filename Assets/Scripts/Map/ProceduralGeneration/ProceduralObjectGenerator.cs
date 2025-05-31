using System.Collections.Generic;
using UnityEngine;

public class ProceduralObjectGenerator : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private ObjectPlacementSettings placementSettings;
    [SerializeField] private ObjectSpawner objectSpawner;

    [Header("Debug")]
    [SerializeField] private bool enableDebugLogs = false;

    private HashSet<Vector2Int> currentWallPositions = new HashSet<Vector2Int>();
    private void Awake()
    {
        if (objectSpawner == null)
        {
            objectSpawner = GetComponent<ObjectSpawner>();
            if (objectSpawner == null)
            {
                GameObject spawnerGO = new GameObject("Object Spawner");
                spawnerGO.transform.SetParent(transform);
                objectSpawner = spawnerGO.AddComponent<ObjectSpawner>();
            }
        }
    }

    //generate objects for the current floor layout
    public void GenerateObjects(HashSet<Vector2Int> floorPositions, HashSet<Vector2Int> wallPositions)
    {
        if (placementSettings == null)
        {
            Debug.LogWarning("ProceduralObjectGenerator: No placement settings assigned!");
            return;
        }
        if (placementSettings.objectTypes.Count == 0)
        {
            Debug.LogWarning("ProceduralObjectGenerator: No object types configured in placement settings!");
            return;
        }
        ClearObjects();
        currentWallPositions = new HashSet<Vector2Int>(wallPositions);
        List<HashSet<Vector2Int>> rooms = PlacementAlgorithms.AnalyzeRoomStructure(floorPositions);
        if (enableDebugLogs)
        {
            Debug.Log($"ProceduralObjectGenerator: Found {rooms.Count} separate room areas");
        }

        foreach (HashSet<Vector2Int> room in rooms)
        {
            GenerateObjectsForRoom(room);
        }

        objectSpawner.CleanupObjectsOutsideFloor(floorPositions);

        if (enableDebugLogs)
        {
            Debug.Log($"ProceduralObjectGenerator: Spawned {objectSpawner.GetSpawnedObjectCount()} objects total");
        }
    }

    private void GenerateObjectsForRoom(HashSet<Vector2Int> roomFloorPositions)
    {
        int roomSize = roomFloorPositions.Count;

        if (roomSize < placementSettings.minRoomSizeForObjects)
        {
            if (enableDebugLogs)
            {
                Debug.Log($"ProceduralObjectGenerator: Skipping room of size {roomSize} (minimum: {placementSettings.minRoomSizeForObjects})");
            }
            return;
        }

        HashSet<Vector2Int> occupiedPositions = new HashSet<Vector2Int>();

        GenerateObjectsByCategory(roomFloorPositions, occupiedPositions, ObjectCategory.Furniture, roomSize);
        GenerateObjectsByCategory(roomFloorPositions, occupiedPositions, ObjectCategory.Interactive, roomSize);
        GenerateObjectsByCategory(roomFloorPositions, occupiedPositions, ObjectCategory.Decoration, roomSize);
        GenerateObjectsByCategory(roomFloorPositions, occupiedPositions, ObjectCategory.Obstacle, roomSize);
        GenerateObjectsByCategory(roomFloorPositions, occupiedPositions, ObjectCategory.Resource, roomSize);
    }
    private void GenerateObjectsByCategory(
        HashSet<Vector2Int> roomFloorPositions,
        HashSet<Vector2Int> occupiedPositions,
        ObjectCategory category,
        int roomSize)
    {
        List<ObjectPlacementData> categoryObjects = placementSettings.GetObjectsByCategory(category);

        if (categoryObjects.Count == 0) return;

        foreach (ObjectPlacementData objectData in categoryObjects)
        {
            if (roomSize < objectData.minRoomSize) continue;

            // find valid positions for this object type
            List<Vector2Int> validPositions = PlacementAlgorithms.FindValidPositions(
                roomFloorPositions,
                currentWallPositions,
                occupiedPositions,
                objectData,
                placementSettings
            );

            if (validPositions.Count == 0) continue;

            // select positions
            List<Vector2Int> selectedPositions = PlacementAlgorithms.SelectPlacementPositions(
                validPositions,
                objectData,
                placementSettings,
                roomSize
            );

            // spawn objects
            foreach (Vector2Int position in selectedPositions)
            {
                if (TryPlaceObject(objectData, position, roomFloorPositions, occupiedPositions))
                {
                    occupiedPositions.Add(position);

                    if (enableDebugLogs)
                    {
                        Debug.Log($"ProceduralObjectGenerator: Placed {objectData.objectName} at {position}");
                    }
                }
            }
        }
    }
    private bool TryPlaceObject(
        ObjectPlacementData objectData,
        Vector2Int position,
        HashSet<Vector2Int> roomFloorPositions,
        HashSet<Vector2Int> occupiedPositions)
    {
        int attempts = 0;
        Vector2Int currentPosition = position;

        while (attempts < placementSettings.maxAttemptsPerObject)
        {
            if (PlacementAlgorithms.IsValidPlacementPosition(
                currentPosition,
                roomFloorPositions,
                currentWallPositions,
                occupiedPositions,
                objectData,
                placementSettings))
            {
                GameObject spawnedObject = objectSpawner.SpawnObject(objectData, currentPosition);
                return spawnedObject != null;
            }

            currentPosition = GetNearbyPosition(position, attempts + 1);
            if (!roomFloorPositions.Contains(currentPosition))
            {
                List<Vector2Int> roomPositions = new List<Vector2Int>(roomFloorPositions);
                if (roomPositions.Count > 0)
                {
                    currentPosition = roomPositions[Random.Range(0, roomPositions.Count)];
                }
            }

            attempts++;
        }

        if (enableDebugLogs)
        {
            Debug.LogWarning($"ProceduralObjectGenerator: Failed to place {objectData.objectName} after {placementSettings.maxAttemptsPerObject} attempts");
        }

        return false;
    }
    private Vector2Int GetNearbyPosition(Vector2Int originalPosition, int radius)
    {
        int x = Random.Range(-radius, radius + 1);
        int y = Random.Range(-radius, radius + 1);
        return originalPosition + new Vector2Int(x, y);
    }
    public void ClearObjects()
    {
        if (objectSpawner != null)
        {
            objectSpawner.ClearAllObjects();
        }
    }
    public void SetPlacementSettings(ObjectPlacementSettings settings)
    {
        placementSettings = settings;
    }
    public ObjectPlacementSettings GetPlacementSettings()
    {
        return placementSettings;
    }
}
