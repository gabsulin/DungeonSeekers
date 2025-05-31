using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

[CreateAssetMenu(fileName = "New Object Placement Settings", menuName = "Procedural Generation/Object Placement Settings")]
public class ObjectPlacementSettings : ScriptableObject
{
    [Header("General Settings")]
    [Range(0f, 1f)]
    public float globalObjectDensity = 0.2f;

    [Range(1, 20)]
    public int maxAttemptsPerObject = 10;

    [Header("Room Analysis")]
    [Range(3, 50)]
    public int minRoomSizeForObjects = 5;

    [Header("Object Types")]
    public List<ObjectPlacementData> objectTypes = new List<ObjectPlacementData>();

    [Header("Pathfinding Settings")]
    [Range(1, 5)]
    public int minPathWidth = 2;

    public bool ensureRoomConnectivity = true;

    [Header("Debug Settings")]
    public bool showDebugGizmos = false;
    public Color debugObjectColor = Color.green;
    public Color debugBlockedAreaColor = Color.red;

    //gets all object types filtered by category
    public List<ObjectPlacementData> GetObjectsByCategory(ObjectCategory category)
    {
        List<ObjectPlacementData> filtered = new List<ObjectPlacementData>();
        foreach (var obj in objectTypes)
        {
            if (obj.category == category) filtered.Add(obj);
        }
        return filtered;
    }

    //gets a weighted random object from the specified category
    public ObjectPlacementData GetWeightedRandomObject(ObjectCategory category)
    {
        var objects = GetObjectsByCategory(category);
        if (objects.Count == 0) return null;

        int totalWeight = 0;
        foreach (var obj in objects)
        {
            totalWeight += obj.weight;
        }

        int randomValue = Random.Range(0, totalWeight);
        int currentWeight = 0;

        foreach (var obj in objects)
        {
            currentWeight += obj.weight;
            if (randomValue < currentWeight)
            {
                return obj;
            }
        }
        return objects[objects.Count - 1];
    }

    //gets a weighted random object from all categories
    public ObjectPlacementData GetWeightedRandomObject()
    {
        if (objectTypes.Count == 0) return null;

        int totalWeight = 0;
        foreach (var obj in objectTypes)
        {
            totalWeight += obj.weight;
        }

        int randomValue = Random.Range(0, totalWeight);
        int currentWeight = 0;

        foreach (var obj in objectTypes)
        {
            currentWeight += obj.weight;
            if (randomValue < currentWeight)
            {
                return obj;
            }
        }
        return objectTypes[objectTypes.Count - 1];
    }
}
