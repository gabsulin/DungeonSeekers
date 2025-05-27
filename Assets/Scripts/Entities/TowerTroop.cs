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
        FindClosestEnemy();
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
    private void FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        Transform closestEnemy = null;
        float minDistanceSquared = MathF.Pow(attackRange, 2);

        Vector2 currentPosition = transform.position;

        foreach (GameObject enemyObject in enemies)
        {
            Vector2 directionToEnemy = (Vector2)enemyObject.transform.position - currentPosition;
            float distanceSquaredToEnemy = directionToEnemy.sqrMagnitude;

            if (distanceSquaredToEnemy < minDistanceSquared)
            {
                minDistanceSquared = distanceSquaredToEnemy;
                closestEnemy = enemyObject.transform;
            }
        }
        currentTarget = closestEnemy;
        Debug.Log(currentTarget);
    }
    private void ShootAtTarget()
    {
        if (currentTarget != null && currentTarget.GetComponent<EnemyHpSystem>().currentHealth > 0)
        {
            if (Time.time > nextFireTime)
            {
                animator.SetTrigger("Attack");
                nextFireTime = Time.time + fireCooldown;
            } else
            {
                animator.ResetTrigger("Attack");
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