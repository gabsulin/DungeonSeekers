using System;
using UnityEngine;

public class TowerTroop : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Rigidbody2D arrowPrefab;
    [SerializeField] Transform arrowSpawnpoint;

    [Header("Attack Parameters")]
    [SerializeField] float attackRange = 10f;
    [SerializeField] float fireRate = 1f;
    [SerializeField] float arrowSpeed = 10f;
    [SerializeField] string enemyTag = "Enemy";
    [SerializeField] string bossTag = "MiniBoss";

    private Transform currentTarget;
    private Animator animator;
    private float fireCooldown;
    private float nextFireTime;
    private float initialScaleX;
    void Start()
    {
        animator = GetComponent<Animator>();

        fireCooldown = 1f / fireRate;
        nextFireTime = Time.time;
    }
    void Update()
    {
        FindTarget();
        UpdateFacingDirection();
        ShootAtTarget();
    }
    private void UpdateFacingDirection()
    {
        if (currentTarget != null)
        {
            if (currentTarget.position.x < transform.position.x)
            {
                transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
            }
            else if (currentTarget.position.x > transform.position.x)
            {
                transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
            }
        }
    }
    private void FindTarget()
    {
        Transform newTarget = null;
        Vector2 currentPosition = transform.position;

        GameObject[] bosses = GameObject.FindGameObjectsWithTag(bossTag);
        Transform closestBossInRange = null;
        float minBossDistanceSquared = Mathf.Pow(attackRange, 2);

        foreach (GameObject bossObject in bosses)
        {
            BossHpSystem bossHp = bossObject.GetComponent<BossHpSystem>();
            if (bossHp != null && bossHp.currentHealth > 0)
            {
                Vector2 directionToBoss = (Vector2)bossObject.transform.position - currentPosition;
                float distanceSquaredToBoss = directionToBoss.sqrMagnitude;

                if (distanceSquaredToBoss < minBossDistanceSquared)
                {
                    minBossDistanceSquared = distanceSquaredToBoss;
                    closestBossInRange = bossObject.transform;
                }
            }
        }
        if (closestBossInRange != null)
        {
            newTarget = closestBossInRange;
        }
        else
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
            Transform closestEnemyInRange = null;
            float minEnemyDistanceSquared = Mathf.Pow(attackRange, 2);

            foreach (GameObject enemyObject in enemies)
            {
                bool enemyHasHealth = false;
                EnemyHpSystem enemyHpSys = enemyObject.GetComponent<EnemyHpSystem>();
                if (enemyHpSys != null && enemyHpSys.currentHealth > 0)
                {
                    enemyHasHealth = true;
                }
                else
                {
                    EnemyHealth enemyHealthCom = enemyObject.GetComponent<EnemyHealth>();
                    if (enemyHealthCom != null && enemyHealthCom.currentHealth > 0)
                    {
                        enemyHasHealth = true;
                    }
                }
                if (enemyHasHealth)
                {
                    Vector2 directionToEnemy = (Vector2)enemyObject.transform.position - currentPosition;
                    float distanceSquaredToEnemy = directionToEnemy.sqrMagnitude;

                    if (distanceSquaredToEnemy < minEnemyDistanceSquared)
                    {
                        minEnemyDistanceSquared = distanceSquaredToEnemy;
                        closestEnemyInRange = enemyObject.transform;
                    }
                }
            }
            newTarget = closestEnemyInRange;
        }
        currentTarget = newTarget;
    }
    private void ShootAtTarget()
    {
        if (currentTarget != null)
        {
            bool targetHasHealth = false;

            BossHpSystem bossHp = currentTarget.GetComponent<BossHpSystem>();
            if (bossHp != null && bossHp.currentHealth > 0)
            {
                targetHasHealth = true;
            }
            if (!targetHasHealth)
            {
                EnemyHpSystem hpSystem = currentTarget.GetComponent<EnemyHpSystem>();
                if (hpSystem != null && hpSystem.currentHealth > 0)
                {
                    targetHasHealth = true;
                }
            }
            if (!targetHasHealth)
            {
                EnemyHealth healthSystem = currentTarget.GetComponent<EnemyHealth>();
                if (healthSystem != null && healthSystem.currentHealth > 0)
                {
                    targetHasHealth = true;
                }
            }
            if (targetHasHealth)
            {
                if (Time.time > nextFireTime)
                {
                    animator.SetTrigger("Attack");
                    nextFireTime = Time.time + fireCooldown;
                }
                else
                {
                    animator.ResetTrigger("Attack");
                }
            }
        }
    }
    public void AttackEnemy()
    {
        if (arrowPrefab != null)
        {
            Rigidbody2D arrow = Instantiate(arrowPrefab, arrowSpawnpoint.position, Quaternion.identity);
            Vector2 direction = (currentTarget.position - arrowSpawnpoint.position).normalized;
            arrow.AddForce(direction * arrowSpeed, ForceMode2D.Impulse);

            float angle = MathF.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            arrow.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}