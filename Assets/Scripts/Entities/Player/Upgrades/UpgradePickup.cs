using UnityEngine;

public class UpgradePickup : MonoBehaviour, IInteractable
{
    public UpgradeType upgradeType;
    public float value;
    public int price;
    public void Interact()
    {
        Upgrade upgrade = new Upgrade
        {
            upgradeType = upgradeType,
            value = value
        };

        CoinManager.instance.Buy(price);
        (AudioManager.Instance)?.PlaySFX("Upgrade");
        UpgradeManager.Instance.ApplyUpgradeToCurrentWeapon(upgrade);

        Destroy(gameObject);
    }
}
