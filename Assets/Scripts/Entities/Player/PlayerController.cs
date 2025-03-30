using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    private PlayerObj playerObj;
    private Rigidbody2D rb;

    public float moveSpeed = 8f;
    private SPUM_Prefabs anim;

    private bool isAttacking = false;
    private float attackCooldown = 0.5f;
    private float attackTimer = 0f;

    private Vector2 lastMovementDirection = Vector2.right;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerObj = GetComponent<PlayerObj>();
        anim = GetComponentInChildren<SPUM_Prefabs>();
        
    }

    private void Update()
    {
        HandleMovement();
        HandleActions();
        HandleAttackCooldown();
    }

    private void HandleMovement()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        if (input.magnitude > 0 && !isAttacking)
        {
            playerObj._playerState = PlayerObj.PlayerState.move;
            transform.position += (Vector3)(input * moveSpeed * Time.deltaTime);
            lastMovementDirection = input;  
            FlipCharacter(input.x);
            anim.PlayAnimation(1);
        }
        else if (!isAttacking)
        {
            playerObj._playerState = PlayerObj.PlayerState.idle;
            FlipCharacter(lastMovementDirection.x);
            anim.PlayAnimation(0);
        }
    }

    private void HandleActions()
    {
        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {
            playerObj._playerState = PlayerObj.PlayerState.attack;
            isAttacking = true;
            attackTimer = attackCooldown;
            anim.PlayAnimation(4);
            AudioManager.Instance.PlaySFX("SwordAttack");
        }

        if (Input.GetMouseButtonDown(1))
        {
            playerObj._playerState = PlayerObj.PlayerState.stun;
            anim.PlayAnimation(7);
            AudioManager.Instance.PlaySFX("SwordSpecialAttack");
        }
    }

    private void HandleAttackCooldown()
    {
        if (isAttacking)
        {
            attackTimer -= Time.deltaTime;

            if (attackTimer <= 0f)
            {
                isAttacking = false;
                if (rb.linearVelocity.magnitude == 0)
                {
                    playerObj._playerState = PlayerObj.PlayerState.idle;
                    FlipCharacter(lastMovementDirection.x);
                    anim.PlayAnimation(0);
                }
            }
        }
    }

    private void FlipCharacter(float directionX)
    {
        if (directionX > 0)
            anim.transform.localScale = new Vector3(-1, 1, 1);
        else if (directionX < 0)
            anim.transform.localScale = new Vector3(1, 1, 1);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Colliding with: " + collision.gameObject.name);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("Triggering with: " + collider.gameObject.name);
    }
}
