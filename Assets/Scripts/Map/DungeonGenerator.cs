using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    [SerializeField] AbstractDungeonGenerator generator;

    private void Awake()
    {
        generator = FindFirstObjectByType<AbstractDungeonGenerator>();
        generator.GenerateDungeon();
    }
}
