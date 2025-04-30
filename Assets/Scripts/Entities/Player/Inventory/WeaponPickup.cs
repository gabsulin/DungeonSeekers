using UnityEngine;

public class WeaponPickup : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        ActiveInventory playerInventory = FindFirstObjectByType<ActiveInventory>();
        if (playerInventory != null)
        {
            gameObject.tag = "Untagged";
            gameObject.layer = 9;
            playerInventory.PickUpWeapon(gameObject);
        }
    }
}
