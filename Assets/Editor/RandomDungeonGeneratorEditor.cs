using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AbstractDungeonGenerator), true)]
public class RandomDungeonGeneratorEditor : Editor
{
    AbstractDungeonGenerator generator;

    private void Awake()
    {
        generator = (AbstractDungeonGenerator)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if(GUILayout.Button("Create Dungeon"))
        {
            generator.GenerateDungeon();
        }
        if(GUILayout.Button("Clear Dungeon"))
        {
            generator.ClearDungeon();
        }
    }
}
