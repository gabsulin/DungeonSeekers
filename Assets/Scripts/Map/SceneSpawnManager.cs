using UnityEngine;

public class SceneSpawnManager : MonoBehaviour
{
    PlayerController player;
    [SerializeField] Transform playerSpawn;
    void Start()
    {
        player = FindFirstObjectByType<PlayerController>();
        player.transform.position = transform.position;
    }
}
