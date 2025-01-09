using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RangedEnemyAttack : MonoBehaviour
{
    EnemyMovement enemyDistance;
    EnemyHpSystem enemyHp;
    PlayerHpSystem playerHp;

    [SerializeField] public Rigidbody2D bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;
    Transform aimTarget;

    Coroutine shootingRoutine;

    private void Awake()
    {
        shootingRoutine = null;
    }
    void Start()
    {
        enemyDistance = GetComponent<EnemyMovement>();
        enemyHp = GetComponent<EnemyHpSystem>();
        playerHp = FindFirstObjectByType<PlayerHpSystem>();

        if (playerHp != null)
        {
            Transform aimTargetTransform = playerHp.transform;
            aimTarget = aimTargetTransform.Find("4/AimTarget");
        }
    }

    private void Update()
    {
        if(aimTarget != null)
        {
            StartShooting();
        }
    }

    public void StartShooting()
    {
        if (shootingRoutine == null && enemyHp.currentHealth > 0 && playerHp.currentHp > 0)
        {
            shootingRoutine = StartCoroutine(ShootingRoutine());
        }
    }

    public void StopShooting()
    {
        if(shootingRoutine != null)
        {
            StopCoroutine(shootingRoutine);
            shootingRoutine = null;
        }
    }

    public IEnumerator ShootingRoutine()
    {
        StopShooting();

        while (enemyHp.currentHealth > 0)
        {
            if (playerHp.currentHp > 0 && CanSeePlayer() && enemyHp.stunned == false)
            {
                Shoot();
            }
            yield return new WaitForSeconds(0.75f);
        }
        shootingRoutine = null;
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


    private void Shoot()
    {
        if (enemyDistance.distance <= 8)
        {
            var bullet = Instantiate(bulletPrefab, bulletSpawnPoint.transform.position, Quaternion.identity);
            if (bullet != null)
            {
                Vector2 direction = ((Vector2)aimTarget.transform.position - (Vector2)bulletSpawnPoint.position).normalized;
                bullet.AddForce(direction * 5, ForceMode2D.Impulse);
                Destroy(bullet.gameObject, 2);
            }
        }
    }
}
