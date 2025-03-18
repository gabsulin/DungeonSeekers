using UnityEngine;

public class WeaponPickup : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        ActiveInventory playerInventory = FindFirstObjectByType<ActiveInventory>();
        if (playerInventory != null)
        {
            playerInventory.PickUpWeapon(gameObject);
        }
    }
}
