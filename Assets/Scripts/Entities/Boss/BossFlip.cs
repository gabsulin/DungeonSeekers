using UnityEngine;

public class BossFlip : MonoBehaviour
{
    public Transform player;

    private bool isFlipped = false;

    private void Start()
    {
        player = FindFirstObjectByType<PlayerObj>().transform;
    }

    public void LookAtPlayer()
    {
        if (transform.position.x > player.position.x && isFlipped)
        {
            Flip();
        }
        else if (transform.position.x < player.position.x && !isFlipped)
        {
            Flip();
        }
    }

    private void Flip()
    {
        Vector3 flippedScale = transform.localScale;
        flippedScale.x *= -1f;
        transform.localScale = flippedScale;

        isFlipped = !isFlipped;
    }
}