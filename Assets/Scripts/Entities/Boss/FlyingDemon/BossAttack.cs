using System.Collections;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    private Transform aimTarget;
    private PlayerObj player;
    public Transform bulletSpawnPoint;
    private Animator anim;
    public Rigidbody2D bulletPrefab;
    private PlayerHpSystem playerHp;
    [SerializeField] GameObject enemyPrefab;
    //[SerializeField] Transform enemySpawnPoint;


    bool hasSpawnedEnemies = false;
    int startAngle = 0, angleStep = 15, endAngle, currentAngle;


    void Start()
    {
        player = FindFirstObjectByType<PlayerObj>();
        playerHp = FindFirstObjectByType<PlayerHpSystem>();
        anim = GetComponent<Animator>();
        if (player != null)
        {
            Transform aimTargetTransform = player.transform;
            aimTarget = aimTargetTransform.Find("AimTarget");
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

            fireball.AddForce(direction * 10, ForceMode2D.Impulse);
        }
    }

    public void EnragedAttack()
    {
        if (!hasSpawnedEnemies)
        {
            for (int i = 0; i < 5; i++)
            {
                Instantiate(enemyPrefab, Vector2.zero, Quaternion.identity);
            }

            hasSpawnedEnemies = true;
        }
        StartCoroutine(EnragedShootingRoutine());
    }

    public bool CanSeePlayer()
    {
        RaycastHit2D[] hits = Physics2D.LinecastAll(transform.position, aimTarget.position);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Collision"))
            {
                return false;
            }
        }
        return true;
    }

    IEnumerator EnragedShootingRoutine()
    {
        startAngle = Random.Range(0, 360);
        currentAngle = startAngle;
        endAngle = startAngle + 360;

        while (currentAngle < endAngle)
        {
            float rad = currentAngle * Mathf.Deg2Rad;
            Vector2 direction = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
            var fireball = Instantiate(bulletPrefab, bulletSpawnPoint.transform.position, Quaternion.identity);

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            fireball.transform.rotation = Quaternion.Euler(0, 0, angle + 180);

            fireball.linearVelocity = direction * 5;

            currentAngle += angleStep;

        }

        yield return new WaitForSeconds(2);
    }


}
