using UnityEngine;

public class PlantEnemy : Enemy
{
    CameraShake shake;
    protected override void Start()
    {
        attackCooldown = animator.GetFloat("AttackCooldown");
        player = FindFirstObjectByType<PlayerController>().transform;
        shake = GetComponent<CameraShake>();
    }
    private void Update()
    {
        attackCooldown += Time.deltaTime;
        animator.SetFloat("AttackCooldown", attackCooldown);
    }
    public override void Attack()
    {
        currentState = EnemyState.Attack;
    }
    public override void OnCollisionEnter2D(Collision2D collision)
    {
        if (currentState == EnemyState.Attack && collision.gameObject.CompareTag("Player") && (attackHitbox != null || collision.collider.IsTouching(attackHitbox)))
        {
            PlayerHpSystem playerHp = collision.collider.GetComponent<PlayerHpSystem>();
            playerHp.TakeHit(damage);
            currentState = EnemyState.Idle;
            attackCooldown = 0f;
            shake.StartShake(force: 0.1f);
            player.GetComponent<Rigidbody2D>().linearVelocity = Vector3.zero;
            ExecuteIdleState();
            DeactivateAttackHitbox();
        }
    }

    protected override void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 1f);
    }
}
