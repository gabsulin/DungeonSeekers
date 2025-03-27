using UnityEngine;

public class CanvasHpShields : MonoBehaviour
{
    PlayerHpSystem playerHp;
    void Start()
    {
        playerHp = FindFirstObjectByType<PlayerHpSystem>();
        if(playerHp != null )
        {
            playerHp.AssignUIElements();
            playerHp.UpdateUI();
            Debug.Log("Assigned");
        } else
        {
            Debug.Log("Couldn't assign");
        }
    }
}
