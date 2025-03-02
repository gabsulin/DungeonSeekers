using UnityEngine;
using System.Collections.Generic;
public class Shop : MonoBehaviour
{
    public List<GameObject> itemsToBuy;
    public List<Transform> spawnPoints;
    void Start()
    {
        foreach (Transform spawnPoint in spawnPoints)
        {
            int randomItem = Random.Range(0, itemsToBuy.Count);

            GameObject itemToBuy = itemsToBuy[randomItem];
            Instantiate(itemToBuy, spawnPoint.position, Quaternion.identity);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
