using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ActiveInventory : MonoBehaviour
{
    public GameObject[] inventorySlots;
    public List<GameObject> weaponPrefabs = new List<GameObject>();
    public List<Sprite> weaponSprites = new List<Sprite>();
    public Transform weaponParent;

    private GameObject currentWeaponInstance;
    private int activeSlotIndex = 0;

    private void Start()
    {
        weaponParent = GameObject.Find("P_WeaponP").transform;
        if (weaponParent.childCount > 0)
        {
            GameObject defaultWeapon = weaponParent.GetChild(0).gameObject;
            weaponSprites.Add(defaultWeapon.GetComponent<SpriteRenderer>().sprite);
            weaponPrefabs.Add(defaultWeapon);
            currentWeaponInstance = defaultWeapon;

            Transform itemTransform = inventorySlots[0].transform.Find("Item");
            UpdateSprites(defaultWeapon, itemTransform);
        }

        UpdateActiveSlot();
        UpdateWeapon();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) SwitchSlots(0);
        else if (Input.GetKeyDown(KeyCode.Alpha2)) SwitchSlots(1);
        else if (Input.GetKeyDown(KeyCode.Alpha3)) SwitchSlots(2);
        else if (Input.GetKeyDown(KeyCode.Alpha4)) SwitchSlots(3);
        else if (Input.GetKeyDown(KeyCode.Alpha5)) SwitchSlots(4);

        if (Input.GetKeyDown(KeyCode.Q)) DropWeapon();
    }

    void SwitchSlots(int newSlotIndex)
    {
        if (newSlotIndex < 0 || newSlotIndex >= weaponPrefabs.Count) return;

        if (newSlotIndex == activeSlotIndex || weaponPrefabs[newSlotIndex] == null) return;

        activeSlotIndex = newSlotIndex;
        UpdateActiveSlot();
        UpdateWeapon();
    }

    void UpdateActiveSlot()
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            Transform activeTransform = inventorySlots[i].transform.Find("Active");
            if (activeTransform != null)
            {
                activeTransform.gameObject.SetActive(i == activeSlotIndex);
            }

            if (i < weaponSprites.Count)
            {
                Transform weaponIcon = inventorySlots[i].transform.Find("Item");
                if (weaponIcon != null)
                {
                    Image weaponIconImage = weaponIcon.GetComponent<Image>();
                    if (weaponIconImage != null)
                    {
                        weaponIconImage.sprite = weaponSprites[i];
                    }
                }
            }
        }
    }

    private void UpdateWeapon()
    {
        if (currentWeaponInstance != null)
        {
            currentWeaponInstance.SetActive(false);
        }
        if (activeSlotIndex >= 0 && activeSlotIndex < weaponPrefabs.Count)
        {
            currentWeaponInstance = weaponPrefabs[activeSlotIndex];

            if (currentWeaponInstance != null)
            {
                currentWeaponInstance.SetActive(true);
            }
        }
    }
    private static void UpdateSprites(GameObject newWeapon, Transform itemTransform)
    {
        RectTransform imageRect = itemTransform.GetComponent<RectTransform>();
        if (imageRect != null)
        {
            float width = 50f;
            float height = 50f;
            float rotation = 0f;

            if (newWeapon.GetComponent<Gun>())
            {
                width = 55f;
                height = 35f;
                rotation = 0f;
            }
            else if (newWeapon.GetComponent<Melee>())
            {
                width = 35f;
                height = 65f;
                rotation = -45f;
            }

            imageRect.sizeDelta = new Vector2(width, height);
            imageRect.rotation = Quaternion.Euler(0, 0, rotation);
        }
    }
    public void PickUpWeapon(GameObject weapon)
    {
        if (weaponParent == null || weapon == null) return;

        if (weaponPrefabs.Any(w => w.name == weapon.name)) return;

        if (currentWeaponInstance != null)
        {
            currentWeaponInstance.SetActive(false);
        }

        GameObject newWeapon = Instantiate(weapon, weaponParent);
        newWeapon.name = weapon.name;
        newWeapon.transform.localPosition = Vector3.zero;
        newWeapon.transform.localRotation = Quaternion.identity;
        newWeapon.SetActive(false);

        weaponPrefabs.Add(newWeapon);

        SpriteRenderer spriteRenderer = weapon.GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            weaponSprites.Add(spriteRenderer.sprite);
        }

        int newIndex = weaponPrefabs.Count - 1;

        if (newIndex >= 0 && newIndex < inventorySlots.Length)
        {
            Transform itemTransform = inventorySlots[newIndex].transform.Find("Item");

            if (itemTransform != null)
            {
                UpdateSprites(newWeapon, itemTransform);

                Image itemImage = itemTransform.GetComponent<Image>();
                if (itemImage != null)
                {
                    itemImage.sprite = weaponSprites[newIndex];
                    itemTransform.gameObject.SetActive(true);
                }
            }
        }


        activeSlotIndex = weaponPrefabs.Count - 1;
        UpdateActiveSlot();
        UpdateWeapon();

        newWeapon.tag = "Melee";
        newWeapon.layer = 9;
        Destroy(weapon);
    }

    public void DropWeapon()
    {
        if (activeSlotIndex < 0 || activeSlotIndex >= weaponPrefabs.Count || weaponPrefabs[activeSlotIndex] == null)
            return;

        GameObject weaponToDrop = weaponPrefabs[activeSlotIndex];

        GameObject droppedWeapon = Instantiate(weaponToDrop, PlayerController.Instance.transform.position, Quaternion.identity);

        droppedWeapon.name = weaponToDrop.name;
        droppedWeapon.SetActive(true);
        droppedWeapon.tag = "Interactable";
        droppedWeapon.layer = 8;
        if (droppedWeapon.GetComponent<WeaponPickup>() == null)
        {
            droppedWeapon.AddComponent<WeaponPickup>();
        }

        Collider2D droppedCollider = droppedWeapon.GetComponent<Collider2D>();
        if (droppedCollider != null)
        {
            droppedCollider.isTrigger = true;
        }

        weaponPrefabs.RemoveAt(activeSlotIndex);
        weaponSprites.RemoveAt(activeSlotIndex);

        Destroy(weaponToDrop);

        if (activeSlotIndex >= weaponPrefabs.Count && weaponPrefabs.Count > 0)
        {
            activeSlotIndex = weaponPrefabs.Count - 1;
        }
        else if (weaponPrefabs.Count == 0)
        {
            activeSlotIndex = 0;
        }

        for (int i = 0; i < inventorySlots.Length; i++)
        {
            Transform itemTransform = inventorySlots[i].transform.Find("Item");
            if (itemTransform != null)
            {
                itemTransform.gameObject.SetActive(false);
            }
        }

        for (int i = 0; i < weaponSprites.Count && i < inventorySlots.Length; i++)
        {
            Transform itemTransform = inventorySlots[i].transform.Find("Item");
            if (itemTransform != null)
            {
                UpdateSprites(weaponPrefabs[i], itemTransform);

                Image itemImage = itemTransform.GetComponent<Image>();
                if (itemImage != null)
                {
                    itemImage.sprite = weaponSprites[i];
                    itemTransform.gameObject.SetActive(true);
                }
            }
        }

        UpdateActiveSlot();
        UpdateWeapon();
    }
}