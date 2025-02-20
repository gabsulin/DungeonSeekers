using UnityEngine;

public class ActivateMM : MonoBehaviour
{
    [SerializeField] GameObject square;
    [SerializeField] Rigidbody2D player, enemy;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Instantiate(square);
        player.linearVelocity = Vector2.zero;
        player.angularVelocity = 0;
        enemy.linearVelocity = Vector2.zero;
        enemy.angularVelocity = 0;

    }
}
