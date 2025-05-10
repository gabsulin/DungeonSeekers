using UnityEngine;

public class Ability : ScriptableObject
{
    public new string name;
    public float coolDownTime;
    public float activeTime;

    public virtual void Activate(GameObject parent) {}
}
