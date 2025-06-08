using UnityEngine;

[CreateAssetMenu]
public class DashAbility : Ability
{
    public float dashVelocity;

    public override void Activate(GameObject parent)
    {
        AbilityHolder abilityHolder = parent.GetComponent<AbilityHolder>();
        PlayerController player = parent.GetComponent<PlayerController>();
        Rigidbody2D rb = parent.GetComponent<Rigidbody2D>(); 
        PlayerHpSystem playerHp = parent.GetComponent<PlayerHpSystem>();

        Vector2 dashDirection;

        if (player.input != Vector2.zero)
        {
            dashDirection = player.input.normalized;
        }
        else
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 directionToMouse = (mouseWorldPos - parent.transform.position);
            dashDirection = directionToMouse.normalized;
        }

        rb.linearVelocity = dashDirection * dashVelocity;
        playerHp.isImmune = true;
        abilityHolder.isReset = false;
    }
}
