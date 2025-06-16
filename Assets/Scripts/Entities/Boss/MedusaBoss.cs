using Unity.VisualScripting;
using UnityEngine;

public class MedusaBoss : MonoBehaviour
{
    Transform aimTarget;
    PlayerController player;
    Animator anim;
    PlayerHpSystem playerHp;

    [SerializeField] GameObject stoneObj;
    [SerializeField] Rigidbody2D snakeProjectile;
    [SerializeField] float attackRange;
    [SerializeField] Transform projectileSpawnPoint;

    float cooldown;
    void Start()
    {
        player = FindFirstObjectByType<PlayerController>();
        anim = GetComponent<Animator>();
        playerHp = FindFirstObjectByType<PlayerHpSystem>();
        if (player != null)
        {
            Transform aimTargetTransform = player.transform;
            aimTarget = aimTargetTransform.Find("AimTarget");
        }
        cooldown = anim.GetFloat("AttackCooldown");
    }

    void Update()
    {
        if (playerHp.isDead)
        {
            anim.ResetTrigger("Attack");
        }
        cooldown += Time.deltaTime;
        anim.SetFloat("AttackCooldown", cooldown);
    }
    public void ChooseAttack()
    {
        float rand = Random.value;

        if(rand < 0.8)
        {
            Attack();
            (AudioManager.Instance)?.PlaySFX("MedusaAttack");
        } else
        {
            Petrify();
            (AudioManager.Instance)?.PlaySFX("Petrification");
        }
    }
    private void Attack()
    {
        var projectile = Instantiate(snakeProjectile, projectileSpawnPoint.position, Quaternion.identity);

        if (projectile != null)
        {
            Vector2 direction = ((Vector2)aimTarget.position - (Vector2)projectileSpawnPoint.position).normalized;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            projectile.transform.rotation = Quaternion.Euler(0, 0, angle + 180);

            projectile.AddForce(direction * 8, ForceMode2D.Impulse);
        }
        cooldown = 0;
    }
    private void Petrify()
    {
        var stone = Instantiate(stoneObj, player.transform.position, Quaternion.identity);

        playerHp.TakeHit(1);

        cooldown = 0;
    }
}
