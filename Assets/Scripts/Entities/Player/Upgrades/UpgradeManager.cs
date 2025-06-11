using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance;

    private List<Upgrade> activeUpgrades = new();

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    void Start()
    {
        if (activeUpgrades.Count > 0)
        {
            Debug.Log("Aktivní upgrady pøi startu:");
            foreach (Upgrade upgrade in activeUpgrades)
            {
                Debug.Log($"{upgrade.upgradeType} | Hodnota: {upgrade.value}");
            }
        }
        else
        {
            Debug.Log("Žádné aktivní upgrady pøi startu.");
        }
    }
    public void ApplyUpgradeToCurrentWeapon(Upgrade upgrade)
    {
        Weapon currentWeapon = PlayerController.Instance.GetCurrentWeapon();
        if (currentWeapon != null)
        {
            upgrade.ApplyTo(currentWeapon);
        }

        upgrade.ApplyToPlayer();

        activeUpgrades.Add(upgrade);
    }

    public void ResetUpgrades()
    {
        activeUpgrades.Clear();
        var abilityHolder = PlayerController.Instance.GetComponent<AbilityHolder>();
        if(abilityHolder != null && abilityHolder.ability != null)
        {
            abilityHolder.ability.ResetValues();
        } 
        Debug.Log("Upgrady resetovány.");
    }

    public List<Upgrade> GetActiveUpgrades()
    {
        return new List<Upgrade>(activeUpgrades);
    }
}
