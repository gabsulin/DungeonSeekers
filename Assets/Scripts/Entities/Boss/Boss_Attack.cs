using UnityEngine;

public class Boss_Attack : MonoBehaviour
{
    private Transform aimTarget;
    public Transform bulletSpawnPoint;
    public Rigidbody2D bulletPrefab;
    private Animator anim;

    PlayerObj player;
    PlayerHpSystem playerHp;
    
    void Start()
    {
        player = FindFirstObjectByType<PlayerObj>();
        playerHp = FindFirstObjectByType<PlayerHpSystem>();
        anim = GetComponent<Animator>();
        if(player != null)
        {
            Transform aimTargetTransform = player.transform;
            aimTarget = aimTargetTransform.Find("4/AimTarget");
        }
    }

    void Update()
    {
        if(playerHp.isDead)
        {
            anim.ResetTrigger("Attack");
        }
    }

    public void Attack()
    {
        if(playerHp.currentHp > 0)
        {
            var fireball = Instantiate(bulletPrefab, bulletSpawnPoint.transform.position, Quaternion.identity);
            if (fireball != null)
            {
                Vector2 direction = ((Vector2)aimTarget.position - (Vector2)bulletSpawnPoint.position).normalized;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                fireball.transform.rotation = Quaternion.Euler(0, 0, angle + 180);
                fireball.AddForce(direction * 10, ForceMode2D.Impulse);
            }
        }
    }
}
