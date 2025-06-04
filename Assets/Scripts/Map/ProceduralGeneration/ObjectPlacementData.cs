using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class ObjectPlacementData
{
    [Header("Object Information")]
    public GameObject prefab;
    public string objectName;

    [Header("Placement Rules")]
    [Range(0f, 1f)]
    public float spawnChance;

    [Range(1, 10)]
    public int minDistanceFromWalls = 1;

    [Range(1, 5)]
    public int minDistanceBetweenObjects = 1;

    [Header("Placement Constraints")]
    public bool canPlaceNearWalls = false;
    public bool requiresFloorAccess = true;
    public bool blockMovement = true;

    [Header("Room Requirements")]
    [Range(0, 100)]
    public int minRoomSize = 5;

    [Range(0f, 1f)]
    public float maxDensityPerRoom = 0.3f;

    [Header("Object Category")]
    public ObjectCategory category = ObjectCategory.Decoration;

    [Header("Weighted Section")]
    [Range(1, 100)]
    public int weight = 1;
}

public enum ObjectCategory
{
    Furniture,
    Decoration,
    Interactive,
    Obstacle,
    Resource
}


