using UnityEngine;

public class WeaponPickup : MonoBehaviour, IInteractable
{
    public SpriteRenderer weaponSprite;

    private void Awake()
    {
        weaponSprite = GetComponent<SpriteRenderer>();
    }
    public void Interact()
    {
        ActiveInventory playerInventory = FindFirstObjectByType<ActiveInventory>();
        if (playerInventory != null)
        {
            playerInventory.PickUpWeapon(gameObject.transform);
            gameObject.SetActive(true);
        }
    }
}
