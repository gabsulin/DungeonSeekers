using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyAttack : MonoBehaviour
{
    EnemyMovement enemyMovement;
    EnemyHpSystem enemyHp;
    PlayerHpSystem playerHp;

    [SerializeField] public Rigidbody2D bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private string soundName;
    [SerializeField] private float bulletSpeed = 10f;

    float cooldown = 0.75f;
    Transform aimTarget;

    Coroutine shootingRoutine;

    private void Awake()
    {
        shootingRoutine = null;
    }

    void Start()
    {
        enemyMovement = GetComponent<EnemyMovement>();
        enemyHp = GetComponent<EnemyHpSystem>();

        playerHp = FindFirstObjectByType<PlayerHpSystem>();
        Transform aimTargetParent = playerHp.transform;
        aimTarget = aimTargetParent.Find("AimTarget");
    }

    private void Update()
    {
        cooldown -= Time.deltaTime;
        if (enemyHp == null || playerHp == null || aimTarget == null || enemyMovement == null)
        {
            return;
        }

        if (aimTarget != null)
        {
            if (enemyMovement.distance <= enemyMovement.attackThreshold && enemyHp.currentHealth > 0 && playerHp.currentHp > 0)
            {
                StartShooting();
            }
            else
            {
                StopShooting();
            }
        }
        else
        {
            StopShooting();
        }
    }

    public void StartShooting()
    {
        if (shootingRoutine == null && enemyHp.currentHealth > 0 && playerHp.currentHp > 0)
        {
            try
            {
                shootingRoutine = StartCoroutine(ShootingRoutine());
            }
            catch (System.Exception e)
            {
                Debug.LogError($"RangedEnemyAttack: EXCEPTION caught while starting ShootingRoutine: {e.Message}\n{e.StackTrace}", this.gameObject);
            }
        }
    }

    public void StopShooting()
    {
        if (shootingRoutine != null)
        {
            StopCoroutine(shootingRoutine);
            shootingRoutine = null;
        }
    }

    public IEnumerator ShootingRoutine()
    {
        while (enemyHp != null && enemyHp.currentHealth > 0)
        {
            if (playerHp != null && playerHp.currentHp > 0 && aimTarget != null && CanSeePlayer() && !enemyHp.stunned)
            {
                Shoot();
            }
            else if (playerHp == null || playerHp.currentHp <= 0 || aimTarget == null)
            {
                break;
            }
            yield return new WaitForSeconds(0.75f);
        }
        shootingRoutine = null;
    }

    public bool CanSeePlayer()
    {
        if (aimTarget == null)
        {
            return false;
        }
        if (bulletSpawnPoint == null)
        {
            RaycastHit2D[] hitsFromTransform = Physics2D.LinecastAll(transform.position, aimTarget.position, LayerMask.GetMask("Collision"));
            foreach (RaycastHit2D hit in hitsFromTransform)
            {
                if (hit.transform != aimTarget.root && hit.transform != transform.root)
                {
                    return false;
                }
            }
            return true;
        }

        RaycastHit2D[] hits = Physics2D.LinecastAll(bulletSpawnPoint.position, aimTarget.position, LayerMask.GetMask("Collision"));

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Collision"))
            {
                if (hit.transform != aimTarget.root)
                {
                    return false;
                }
            }
        }
        return true;
    }

    private void Shoot()
    {
        if(cooldown <= 0)
        {
            var bulletInstance = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);

            if (bulletInstance != null)
            {
                Vector2 direction = ((Vector2)aimTarget.position - (Vector2)bulletSpawnPoint.position).normalized;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                bulletInstance.transform.rotation = Quaternion.Euler(0, 0, angle);
                bulletInstance.AddForce(direction * bulletSpeed, ForceMode2D.Impulse);
                cooldown = 0.75f;
                if (AudioManager.Instance != null && !string.IsNullOrEmpty(soundName))
                {
                    AudioManager.Instance.PlaySFX(soundName);
                }
            }
        }
    }
}
