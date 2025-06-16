using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{
    PlayerController player;
    private void Start()
    {
        player = FindFirstObjectByType<PlayerController>();
        player.transform.position = transform.position;
        (AudioManager.Instance)?.PlayMusic("Battle", false);
    }
}
