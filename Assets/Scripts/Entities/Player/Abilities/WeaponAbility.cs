using UnityEngine;

[CreateAssetMenu]
public class WeaponAbility : Ability
{
    public override void Activate(GameObject parent)
    {
        AbilityHolder abilityHolder = parent.GetComponent<AbilityHolder>();
        PlayerController player = parent.GetComponent<PlayerController>();
        Weapon currentWeapon = player.GetCurrentWeapon();
        if (currentWeapon is Melee)
        {
            Melee melee = (Melee)currentWeapon;
            var enemyDamage = melee.GetComponent<EnemyDamage>();
            enemyDamage.damage *= 2;
        }
        else if (currentWeapon is Gun)
        {
            currentWeapon.data.attackCooldown *= 0.5f;
            Gun gun = (Gun)currentWeapon;
            gun.accuracy *= 2f;
            if(gun.accuracy >= 100) gun.accuracy = 100;
        }
        abilityHolder.isReset = false;
    }
}
