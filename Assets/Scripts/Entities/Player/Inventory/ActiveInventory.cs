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
            GameObject newWeaponPrefab = weaponPrefabs[activeSlotIndex];

            Transform existingWeapon = weaponParent.Find(newWeaponPrefab.name);
            if (existingWeapon != null)
            {
                currentWeaponInstance = existingWeapon.gameObject;
                currentWeaponInstance.SetActive(true);
            }
            else
            {
                currentWeaponInstance = Instantiate(newWeaponPrefab, weaponParent);
                currentWeaponInstance.name = newWeaponPrefab.name;
                currentWeaponInstance.transform.localPosition = Vector3.zero;
                currentWeaponInstance.transform.localRotation = Quaternion.identity;
            }
        }
    }

    public void PickUpWeapon(Transform weapon)
    {
        if (weaponParent != null)
        {
            if (!weaponPrefabs.Contains(weapon.gameObject))
            {
                weapon.SetParent(weaponParent);

                weapon.localPosition = Vector3.zero;
                weapon.localRotation = Quaternion.identity;
                weapon.localScale = Vector3.one;
                weaponPrefabs.Add(weapon.gameObject);

                for (int i = 0; i < inventorySlots.Length; i++)
                {
                    Transform itemTransform = inventorySlots[i].transform.Find("Item");
                    if (itemTransform != null)
                    {
                        Image itemImage = itemTransform.GetComponent<Image>();
                        if (itemImage != null && !itemTransform.gameObject.activeSelf)
                        {
                            itemImage.sprite = weapon.GetComponent<SpriteRenderer>().sprite;
                            itemTransform.gameObject.SetActive(true);
                            weaponSprites.Insert(i, itemImage.sprite);
                            break;
                        }
                    }
                }

                activeSlotIndex = weaponPrefabs.Count - 1;
                UpdateActiveSlot();
                UpdateWeapon();
            }
        }
    }

    /*public void PickUpWeapon(GameObject weapon)
    {
        if (!weaponPrefabs.Contains(weapon))
        {
            weaponPrefabs.Add(weapon);

            for (int i = 0; i < inventorySlots.Length; i++)
            {
                Transform itemTransform = inventorySlots[i].transform.Find("Item");
                if (itemTransform != null)
                {
                    Image itemImage = itemTransform.GetComponent<Image>();
                    if (itemImage != null && !itemTransform.gameObject.activeSelf)
                    {
                        itemImage.sprite = weapon.GetComponent<SpriteRenderer>().sprite;
                        itemTransform.gameObject.SetActive(true);
                        weaponSprites.Insert(i, itemImage.sprite);
                        break;
                    }
                }
            }

            activeSlotIndex = weaponPrefabs.Count - 1;
            UpdateActiveSlot();
            UpdateWeapon();

        }
    }

    public void DropWeapon(GameObject weapon)
    {
        int weaponIndex = weaponPrefabs.IndexOf(weapon);
        if (weaponIndex >= 0 && weaponIndex < weaponPrefabs.Count)
        {
            weaponPrefabs.RemoveAt(weaponIndex);
            weaponSprites.RemoveAt(weaponIndex);

            for (int i = weaponIndex; i < inventorySlots.Length; i++)
            {
                Transform itemTransform = inventorySlots[i].transform.Find("Item");
                if (itemTransform != null)
                {
                    Image itemImage = itemTransform.GetComponent<Image>();
                    if (i < weaponSprites.Count)
                    {
                        itemImage.sprite = weaponSprites[i];
                        itemTransform.gameObject.SetActive(true);
                    }
                    else
                    {
                        itemTransform.gameObject.SetActive(false);
                    }
                }
            }
                UpdateActiveSlot();
                UpdateWeapon();
        }
    }*/
}