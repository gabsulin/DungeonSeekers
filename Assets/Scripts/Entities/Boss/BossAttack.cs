using System.Collections;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    private Transform aimTarget;
    public Transform bulletSpawnPoint;
    private Animator anim;
    public Rigidbody2D bulletPrefab;
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] Transform enemySpawnPoint;

    PlayerObj player;
    PlayerHpSystem playerHp;
    BossMove bossMove;

    bool hasSpawnedEnemies = false;
    float startAngle = 0f, angleStep = 18f, endAngle = 360f, currentAngle;


    void Start()
    {
        player = FindFirstObjectByType<PlayerObj>();
        playerHp = FindFirstObjectByType<PlayerHpSystem>();
        anim = GetComponent<Animator>();
        bossMove = FindFirstObjectByType<BossMove>();
        if (player != null)
        {
            Transform aimTargetTransform = player.transform;
            aimTarget = aimTargetTransform.Find("4/AimTarget");
        }
    }

    void Update()
    {
        if (playerHp.isDead)
        {
            anim.ResetTrigger("Attack");
        }
    }

    public void Attack()
    {
        var fireball = Instantiate(bulletPrefab, bulletSpawnPoint.transform.position, Quaternion.identity);
        if (fireball != null)
        {
            Vector2 direction = ((Vector2)aimTarget.position - (Vector2)bulletSpawnPoint.position).normalized;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            fireball.transform.rotation = Quaternion.Euler(0, 0, angle + 180);

            fireball.AddForce(direction * 5, ForceMode2D.Impulse);

            Destroy(fireball, 3f);
        }
    }

    public void EnragedAttack()
    {
        if (!hasSpawnedEnemies)
        {
            for(int i = 0; i < 6;  i++)
            {
                Instantiate(enemyPrefab, enemySpawnPoint.position, Quaternion.identity);
            }
            
            hasSpawnedEnemies = true;
        }
        StartCoroutine(EnragedShootingRoutine());
    }

    IEnumerator EnragedShootingRoutine()
    {

        currentAngle = startAngle;

        while (currentAngle < endAngle)
        {
            float rad = currentAngle * Mathf.Deg2Rad;   //pøevedu si aktuální úhel na radiany
            Vector2 direction = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));    //Mathf.Cos(rad) vrátí hodnotu x a Mathf.Sin(rad) vrátí hodnotu y Vektoru direction.
            var fireball = Instantiate(bulletPrefab, bulletSpawnPoint.transform.position, Quaternion.identity);

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            fireball.transform.rotation = Quaternion.Euler(0, 0, angle + 180);

            fireball.linearVelocity = direction * 5;
            Destroy(fireball, 3f);

            currentAngle += angleStep;

        }

        yield return new WaitForSeconds(2);
    }


}
