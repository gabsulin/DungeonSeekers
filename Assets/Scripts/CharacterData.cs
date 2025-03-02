using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "Scriptable Objects/CharacterData")]
public class CharacterData : ScriptableObject
{
    public string description;
    public string characterName;
    public float health;
    public float shields;
    public bool isUnlocked;
    public int price;
    public float speed;
}
