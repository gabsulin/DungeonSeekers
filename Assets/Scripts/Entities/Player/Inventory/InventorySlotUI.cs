using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{
    public Image weaponSprite;

    public void LoadWeapon(WeaponData data)
    {
        weaponSprite.sprite = data.inventorySprite;
        weaponSprite.SetNativeSize();
        weaponSprite.rectTransform.localScale = data.spriteScale;
        weaponSprite.rectTransform.anchoredPosition = data.spriteOffset;
    }
}
