public enum UpgradeType
{
    FireRate,
    Accuracy,
    MovementSpeed,
    MaxHealth,
    MaxShields,
    Damage,
    CooldownReduction,
    ShieldRecharge
}
public class Upgrade
{
    public UpgradeType upgradeType;
    public float value;

    public void Apply()
    {
        switch (upgradeType)
        {
            case UpgradeType.FireRate:
                //firecooldown *= 1f - value
                break;
            case UpgradeType.Accuracy:

                break;
            case UpgradeType.MovementSpeed:

                break;
            case UpgradeType.MaxHealth:

                break;
            case UpgradeType.MaxShields:

                break;
            case UpgradeType.Damage:

                break;
            case UpgradeType.CooldownReduction:

                break;
            case UpgradeType.ShieldRecharge:

                break;
        }
    }
}
