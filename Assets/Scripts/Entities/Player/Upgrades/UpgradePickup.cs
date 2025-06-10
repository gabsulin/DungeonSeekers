using UnityEngine;

public class UpgradePickup : MonoBehaviour, IInteractable
{
    public UpgradeType upgradeType;
    public float value;

    public void Interact()
    {
        Upgrade upgrade = new Upgrade
        {
            upgradeType = upgradeType,
            value = value
        };

        UpgradeManager.Instance.ApplyUpgradeToCurrentWeapon(upgrade);

        Destroy(gameObject);
    }
}
