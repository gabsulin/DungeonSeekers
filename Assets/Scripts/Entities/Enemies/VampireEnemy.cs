using UnityEngine;

public class VampireEnemy : Enemy
{
    [Header("Vampire Settings")]
    [SerializeField] Transform centerOfAttack;
    [SerializeField] float attackRadius;

    CameraShake shake;
    protected override void Start()
    {
        attackCooldown = animator.GetFloat("AttackCooldown");
        player = FindFirstObjectByType<PlayerController>().transform;
        shake = GetComponent<CameraShake>();
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        attackCooldown += Time.deltaTime;
        animator.SetFloat("AttackCooldown", attackCooldown);
    }
    public override void Attack()
    {
        currentState = EnemyState.Attack;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(centerOfAttack.position, attackRadius);
        foreach (Collider2D collider in colliders)
        {
            if(collider.CompareTag("Player"))
            {
                var playerHp = collider.GetComponent<PlayerHpSystem>();
                if (playerHp != null) playerHp.TakeHit(1);
                shake.StartShake(force: 0.2f);
            }
        }
    }
    public override void OnCollisionEnter2D(Collision2D collision) { }

    protected override void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(centerOfAttack.position, attackRadius);
    }
}

