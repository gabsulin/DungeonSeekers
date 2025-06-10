using UnityEngine;

public class SceneSpawnManager : MonoBehaviour
{
    PlayerController player;
    [SerializeField] Transform playerSpawn;
    [SerializeField] GameObject inventoryCanvas;
    void Start()
    {
        player = FindFirstObjectByType<PlayerController>();
        player.transform.position = transform.position;

        var persistence = GameObject.Find("Persistence");
        Instantiate(inventoryCanvas, persistence.transform);
    }
}
