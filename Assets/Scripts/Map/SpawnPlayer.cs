using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{
    PlayerMovement player;
    private void Awake()
    {
        player = FindFirstObjectByType<PlayerMovement>();
        player.transform.position = transform.position;
    }
}
