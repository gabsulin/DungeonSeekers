using UnityEngine;

[CreateAssetMenu]
public class DashAbility : Ability
{
    public float dashVelocity;

    public override void Activate(GameObject parent)
    {
        PlayerController player = parent.GetComponent<PlayerController>();
        Rigidbody2D rb = parent.GetComponent<Rigidbody2D>(); 
        PlayerHpSystem playerHp = parent.GetComponent<PlayerHpSystem>();

        rb.linearVelocity = player.input.normalized * dashVelocity;
        playerHp.isImmune = true;
    }
}
