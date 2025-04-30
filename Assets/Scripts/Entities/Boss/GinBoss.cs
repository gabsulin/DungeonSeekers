using UnityEngine;

public class GinBoss : MonoBehaviour
{
    Transform aimTarget;
    PlayerObj player;
    Animator anim;
    PlayerHpSystem playerHp;

    [SerializeField] float attackRange;
    [SerializeField] Rigidbody2D magicAttack;
    [SerializeField] Transform magicSpawnPoint;

    float cooldown;
    void Start()
    {
        player = FindFirstObjectByType<PlayerObj>();
        anim = GetComponent<Animator>();
        playerHp = FindFirstObjectByType<PlayerHpSystem>();
        if (player != null)
        {
            Transform aimTargetTransform = player.transform;
            aimTarget = aimTargetTransform.Find("4/AimTarget");
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

    /*
    enraged attack - only damagable with a (special) melee weapon
    his attack will no longer pull the player towards the attack, it will only do the explosion part so its harder for the player to get to the boss without being killed
    the attack will also be delayed. so from 1 second for attack to probably 2 seconds
     */
}
