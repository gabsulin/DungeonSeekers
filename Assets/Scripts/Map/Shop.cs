using UnityEngine;
using System.Collections.Generic;

public class Shop : MonoBehaviour
{
    public List<GameObject> itemsToBuy; // Prefabs
    public List<Transform> spawnPoints;

    public float floatSpeed;
    public float floatHeight;

    private List<GameObject> spawnedItems = new List<GameObject>();
    private List<Vector2> originalPositions = new List<Vector2>();

    void Start()
    {
        foreach (Transform spawnPoint in spawnPoints)
        {
            int randomIndex = Random.Range(0, itemsToBuy.Count);
            GameObject itemPrefab = itemsToBuy[randomIndex];

            GameObject spawnedItem = Instantiate(itemPrefab, spawnPoint.position, Quaternion.identity);
            spawnedItems.Add(spawnedItem);
            originalPositions.Add(spawnPoint.position);
        }
    }

    void Update()
    {
        for (int i = 0; i < spawnedItems.Count; i++)
        {
            GameObject item = spawnedItems[i];
            Vector2 originalPos = originalPositions[i];

            if(item != null)
            {
                float offsetY = Mathf.Sin(Time.time * floatSpeed) * floatHeight;
                item.transform.position = new Vector2(originalPos.x, originalPos.y + offsetY);
            }
        }
    }
}
