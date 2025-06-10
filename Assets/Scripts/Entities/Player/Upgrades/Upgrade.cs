using UnityEngine;

public enum UpgradeType
{
    FireRate,
    Accuracy,
    MovementSpeed,
    MaxHealth,
    MaxShields,
    CooldownReduction,
    ShieldRecharge
}

[System.Serializable]
public class Upgrade
{
    public UpgradeType upgradeType;
    public float value;

    public void ApplyTo(Weapon weapon)
    {
        if (weapon == null) return;

        switch (upgradeType)
        {
            case UpgradeType.FireRate:
            case UpgradeType.CooldownReduction:
                weapon.data.attackCooldown *= 1f - value;
                break;
            case UpgradeType.Accuracy:
                if (weapon is Gun gun)
                {
                    gun.accuracy += value;
                }
                break;
        }
    }

    public void ApplyToPlayer()
    {
        var player = PlayerController.Instance;
        var hpSystem = player.GetComponent<PlayerHpSystem>();

        switch (upgradeType)
        {
            case UpgradeType.MovementSpeed:
                player.moveSpeed += value;
                break;
            case UpgradeType.MaxHealth:
                hpSystem.maxHp += value;
                hpSystem.currentHp += value;
                hpSystem.UpdateUI();
                break;
            case UpgradeType.MaxShields:
                hpSystem.maxShields += value;
                hpSystem.currentShields += value;
                hpSystem.UpdateUI();
                break;
            case UpgradeType.ShieldRecharge:
                hpSystem.ApplyShieldRechargeUpgrade(value);
                break;
        }
    }

}
