using UnityEngine;

public class PlayerController : MonoBehaviour
{
    PlayerObj playerObj;
    Rigidbody2D rb;

    public float moveSpeed;
    float speedX, speedY;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerObj = GetComponent<PlayerObj>();
    }
    private void Update()
    {
        Move();
    }

    private void Move()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (input.magnitude > 1)
        {
            input = input.normalized;
        }

        rb.linearVelocity = input * moveSpeed;
        playerObj._playerState = PlayerObj.PlayerState.move;
    }
}