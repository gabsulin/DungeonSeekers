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
    private Transform aimTarget;

    void Start()
    {
        enemyDistance = GetComponent<EnemyMovement>();
        enemyHp = GetComponent<EnemyHpSystem>();
        playerHp = FindFirstObjectByType<PlayerHpSystem>();

        if (enemyHp != null)
        {
            Debug.Log("EnemyHpSystem found: currentHealth = " + enemyHp.currentHealth);
        }
        else
        {
            Debug.LogError("EnemyHpSystem component not found!");
        }

        if (playerHp != null)
        {
            Transform aimTargetTransform = playerHp.transform;
            aimTarget = aimTargetTransform.Find("4/UnitRoot/Root/AimTarget");
        }
        StartCoroutine(ShootingRoutine());

    }

    IEnumerator ShootingRoutine()
    {
        while (enemyHp.currentHealth > 0)
        {
            if (playerHp.currentHp > 0 && CanSeePlayer())
            {
                Shoot();
            }
            yield return new WaitForSeconds(0.75f);
        }
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
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                return true;
            }
        }
        return false;
    }


    private void Shoot()
    {
        if (enemyDistance.distance <= 6)
        {
            if (CanSeePlayer())
            {
                var bullet = Instantiate(bulletPrefab, bulletSpawnPoint.transform.position, Quaternion.identity);
                if (bullet != null)
                {
                    Vector2 direction = ((Vector2)aimTarget.position - (Vector2)bulletSpawnPoint.position).normalized;
                    bullet.AddForce(direction * 5, ForceMode2D.Impulse);
                    Destroy(bullet.gameObject, 2);
                }
            }
        }
    }
}
