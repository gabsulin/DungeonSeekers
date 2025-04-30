using UnityEngine;

public class Melee : Weapon
{
    protected override void Attack()
    {
        Debug.Log("attack");
        AudioManager.Instance.PlaySFX("SwordAttack");
    }
}

