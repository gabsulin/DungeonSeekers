using UnityEngine;

public class CanvasHpShields : MonoBehaviour
{
    PlayerHpSystem playerHp;
    AbilityHolder abilityHolder;
    void Start()
    {
        playerHp = FindFirstObjectByType<PlayerHpSystem>();
        abilityHolder = FindFirstObjectByType<AbilityHolder>();
        if(playerHp != null && abilityHolder != null)
        {
            playerHp.AssignUIElements();
            playerHp.UpdateUI();
            abilityHolder.AssignBar();
            abilityHolder.UpdateBar();
            Debug.Log("Assigned");
        } else
        {
            Debug.Log("Couldn't assign");
        }
    }
}
