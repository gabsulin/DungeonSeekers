using UnityEngine;

public abstract class AbstractDungeonGenerator : MonoBehaviour
{
    [Header("Base Generation")]
    [SerializeField] protected TilemapVisualizer tilemapVisualizer = null;
    [SerializeField] protected Vector2Int startPosition = Vector2Int.zero;

    [Header("Object Generation")]
    [SerializeField] protected ProceduralObjectGenerator objectGenerator = null;
    [SerializeField] protected bool enableObjectGeneration = true;

    [Header("Settings")]
    [SerializeField] protected ObjectPlacementSettings objectPlacementSettings = null;

    protected virtual void Awake()
    {
        if (tilemapVisualizer == null)
        {
            tilemapVisualizer = FindFirstObjectByType<TilemapVisualizer>();
        }

        if (objectGenerator == null)
        {
            objectGenerator = GetComponent<ProceduralObjectGenerator>();
            if (objectGenerator == null)
            {
                objectGenerator = gameObject.AddComponent<ProceduralObjectGenerator>();
            }
        }

        if (objectPlacementSettings != null && objectGenerator != null)
        {
            objectGenerator.SetPlacementSettings(objectPlacementSettings);
        }
    }

    public void GenerateDungeon()
    {
        if (tilemapVisualizer != null)
        {
            tilemapVisualizer.Clear();
        }

        if (objectGenerator != null)
        {
            objectGenerator.ClearObjects();
        }

        RunProceduralGeneration();
    }
    public void GenerateRoomOnly()
    {
        bool wasObjectGenerationEnabled = enableObjectGeneration;
        enableObjectGeneration = false;

        GenerateDungeon();

        enableObjectGeneration = wasObjectGenerationEnabled;
    }
    public void RegenerateObjectsOnly()
    {
        if (objectGenerator != null && enableObjectGeneration)
        {
            // This would require storing the current floor and wall positions
            // For now, we'll regenerate the entire dungeon
            GenerateDungeon();
        }
    }
    public void SetObjectGenerationEnabled(bool enabled)
    {
        enableObjectGeneration = enabled;
    }
    public void SetObjectPlacementSettings(ObjectPlacementSettings settings)
    {
        objectPlacementSettings = settings;
        if (objectGenerator != null)
        {
            objectGenerator.SetPlacementSettings(settings);
        }
    }
    public ObjectPlacementSettings GetObjectPlacementSettings()
    {
        return objectPlacementSettings;
    }
    public ProceduralObjectGenerator GetObjectGenerator()
    {
        return objectGenerator;
    }

    protected abstract void RunProceduralGeneration();

    protected virtual void OnValidate()
    {
        if (objectPlacementSettings != null && objectGenerator != null)
        {
            objectGenerator.SetPlacementSettings(objectPlacementSettings);
        }
    }
}
