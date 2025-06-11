using UnityEngine;

public class Melee : Weapon
{
    public override void UpdateWeapon()
    {
        base.UpdateWeapon();

        if (data.fireMode == WeaponData.FireMode.SemiAuto)
        {
            if(Input.GetMouseButtonDown(0)) TryAttack(true);
        }
    }
    protected override void Attack()
    {
        Debug.Log("attack");
        AudioManager.Instance.PlaySFX("SwordAttack");
    }
}

