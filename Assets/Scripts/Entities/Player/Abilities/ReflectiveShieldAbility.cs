using UnityEngine;

[CreateAssetMenu]
public class ReflectiveShieldAbility : Ability
{
    [SerializeField] GameObject shieldPrefab;
    GameObject shield;

    public override void Activate(GameObject parent)
    {
        AbilityHolder abilityHolder = parent.GetComponent<AbilityHolder>();
        PlayerHpSystem playerHp = parent.GetComponent<PlayerHpSystem>();

        playerHp.isImmune = true;

        if (shield == null)
        {
            shield = Instantiate(
                shieldPrefab,
                new Vector2(parent.transform.position.x - 0.03f, parent.transform.position.y + 0.5f),
                Quaternion.identity
            );
            shield.transform.SetParent(parent.transform);
        }
        else
        {
            shield.SetActive(true);
        }

        abilityHolder.isReset = false;
    }
}
