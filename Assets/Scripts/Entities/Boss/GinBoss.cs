using UnityEngine;

public class GinBoss : MonoBehaviour
{
    Transform aimTarget;
    PlayerController player;
    Animator anim;
    PlayerHpSystem playerHp;

    [SerializeField] float attackRange;
    [SerializeField] Rigidbody2D magicAttack;
    [SerializeField] Rigidbody2D enragedMagicAttack;
    [SerializeField] Transform magicSpawnPoint;
    [SerializeField] Transform enragedMagicSpawnPoint;
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
    public void Attack()
    {
        var magic = Instantiate(magicAttack, magicSpawnPoint.position, Quaternion.identity);
        if (magic != null)
        {
            Vector2 direction = ((Vector2)aimTarget.position - (Vector2)magicSpawnPoint.position).normalized;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            magic.transform.rotation = Quaternion.Euler(0, 0, angle + 180);

            magic.AddForce(direction * 5, ForceMode2D.Force);
        }
        cooldown = 0;
    }
    public void EnragedAttack()
    {
        Vector2 direction = ((Vector2)aimTarget.position - (Vector2)magicSpawnPoint.position).normalized;

        float distance = Vector2.Distance(aimTarget.position, enragedMagicSpawnPoint.position);
        float maxDistance = 10f;
        float clampedOffset = Mathf.Clamp((distance / maxDistance) - 0.5f, -0.5f, 0.5f);

        Vector3 spawnPos = enragedMagicSpawnPoint.position + new Vector3(clampedOffset, 0, 0);

        var enragedMagic = Instantiate(enragedMagicAttack, spawnPos, Quaternion.identity);
        if (enragedMagic != null)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            enragedMagic.transform.rotation = Quaternion.Euler(0, 0, angle + 180);

            enragedMagic.AddForce(direction * 5, ForceMode2D.Force);
        }

        cooldown = 0;
    }


    /*
    enraged attack - only damagable with a (special) melee weapon
     */
}
