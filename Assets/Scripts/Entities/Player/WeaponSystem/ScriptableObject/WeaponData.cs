using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Scriptable Objects/WeaponData")]
public class WeaponData : ScriptableObject
{
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
}
