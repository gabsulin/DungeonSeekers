using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    public Transform objectParent;

    private List<GameObject> spawnedObjects = new List<GameObject>();
    private HashSet<Vector2Int> occupiedPositions = new HashSet<Vector2Int>();

    private void Awake()
    {
        if (objectParent == null)
        {
            GameObject parentGO = new GameObject("Spawned Objects");
            objectParent = parentGO.transform;
            objectParent.SetParent(transform);
        }
    }

    //spawns an object at the specified world position
    public GameObject SpawnObject(ObjectPlacementData objectData, Vector2Int gridPosition)
    {
        if (objectData == null || objectData.prefab == null)
        {
            Debug.LogWarning("Cannot spawn object: missing object data or prefab");
            return null;
        }

        Vector3 worldPosition = new Vector3(gridPosition.x, gridPosition.y, 0);
        GameObject spawnedObject = Instantiate(objectData.prefab, worldPosition, Quaternion.identity, objectParent);
        spawnedObject.name = $"{objectData.objectName}_ {gridPosition.x}_{gridPosition.y}";

        spawnedObjects.Add(spawnedObject);
        occupiedPositions.Add(gridPosition);

        ObjectInstance instance = spawnedObject.GetComponent<ObjectInstance>();
        if(instance == null) instance = spawnedObject.AddComponent<ObjectInstance>();

        instance.Initialize(objectData, gridPosition);

        return spawnedObject;
    }

    //spawns multiple objects at specified positions
    public List<GameObject> SpawnObjects(List<(ObjectPlacementData data, Vector2Int position)> objectsToSpawn)
    {
        List<GameObject> newObjects = new List<GameObject>();

        foreach (var (data, position) in objectsToSpawn)
        {
            GameObject obj = SpawnObject(data, position);
            if (obj != null)
            {
                newObjects.Add(obj);
            }
        }
        return newObjects;
    }

    public bool IsPositionOccupied(Vector2Int position)
    {
        return occupiedPositions.Contains(position);
    }

    //gets all spawned objects within a radius of the specified position
    public List<GameObject> GetObjectsInRadius(Vector2Int center, int radius)
    {
        List<GameObject> objectsInRadius = new List<GameObject>();

        foreach(GameObject obj in  spawnedObjects)
        {
            if(obj == null) continue;

            ObjectInstance instance = obj.GetComponent<ObjectInstance>();
            if(instance != null)
            {
                float distance = Vector2Int.Distance(center, instance.GridPosition);
                if (distance <= radius)
                {
                    objectsInRadius.Add(obj);
                }
            }
        }
        return objectsInRadius;
    }

    public void ClearAllObjects()
    {
        foreach (GameObject obj in spawnedObjects)
        {
            if (obj != null)
            {
                DestroyImmediate(obj);
            }
        }

        spawnedObjects.Clear();
        occupiedPositions.Clear();
    }

    //removes objects that are outside the specified floor area
    public void CleanupObjectsOutsideFloor(HashSet<Vector2Int> floorPositions)
    {
        List<GameObject> objectsToRemove = new List<GameObject>();
        foreach (GameObject obj in spawnedObjects)
        {
            if (obj == null) continue;

            ObjectInstance instance = obj.GetComponent<ObjectInstance>();
            if (instance != null && !floorPositions.Contains(instance.GridPosition))
            {
                objectsToRemove.Add(obj);
            }
        }
        foreach (GameObject obj in objectsToRemove)
        {
            RemoveObject(obj);
        }
    }

    public void RemoveObject(GameObject obj)
    {
        if(obj == null) return;

        ObjectInstance instance = obj.GetComponent<ObjectInstance>();
        if (instance != null)
        {
            occupiedPositions.Remove(instance.GridPosition);
        }

        spawnedObjects.Remove(obj);
        DestroyImmediate(obj);
    }

    public int GetSpawnedObjectCount()
    {
        spawnedObjects.RemoveAll(obj => obj == null);
        return spawnedObjects.Count;
    }
}

public class ObjectInstance : MonoBehaviour
{
    public ObjectPlacementData PlacementData { get; private set; }
    public Vector2Int GridPosition { get; private set; }

    public void Initialize(ObjectPlacementData data, Vector2Int gridPos)
    {
        PlacementData = data;
        GridPosition = gridPos;
    }
}
