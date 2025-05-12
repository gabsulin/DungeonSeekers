using UnityEngine;

[CreateAssetMenu]
public class ImmuneAbility : Ability
{
    public override void Activate(GameObject parent)
    {
        PlayerHpSystem player = parent.GetComponent<PlayerHpSystem>();
        player.isImmune = true;
        player.transform.position = Vector2.zero;
    }
}
