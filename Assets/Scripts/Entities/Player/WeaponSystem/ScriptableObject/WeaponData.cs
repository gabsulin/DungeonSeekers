using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Scriptable Objects/WeaponData")]
public class WeaponData : ScriptableObject
{
    [Header("Combat Settings")]
    public float attackCooldown = 1f;
    public int burstCount;
    public float burstDelay;
    public enum FireMode
    {
        SemiAuto,
        FullAuto,
        Burst
    }
    public FireMode fireMode;
    public Sound attackSound;

    [Header("Inventory Visuals")]
    public Sprite inventorySprite;
    public Vector2 spriteScale = Vector2.one;
    public Vector2 spriteOffset = Vector2.zero;
}
