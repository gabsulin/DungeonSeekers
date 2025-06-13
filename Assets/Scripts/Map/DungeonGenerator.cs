using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    [SerializeField] AbstractDungeonGenerator generator;

    private void Awake()
    {
        generator.GenerateDungeon();
    }
}
