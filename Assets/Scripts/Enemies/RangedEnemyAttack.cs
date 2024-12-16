using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RangedEnemyAttack : MonoBehaviour
{
    EnemyMovement enemyDistance;
    PlayerObj enemy;
    EnemyHpSystem enemyHp;
    PlayerHpSystem playerHp;
    SPUM_Prefabs anim;

    [SerializeField] public Rigidbody2D bulletPrefab;
    GameObject bullets;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] public Transform player;
    [SerializeField] private Transform aimTarget;

    void Start()
    {
        enemyDistance = GetComponent<EnemyMovement>();
        enemy = GetComponent<PlayerObj>();
        enemyHp = GetComponent<EnemyHpSystem>();
        playerHp = FindAnyObjectByType<PlayerHpSystem>();
        anim = FindAnyObjectByType<SPUM_Prefabs>();

        StartCoroutine(ShootingRoutine());
    }

    void Update()
    {

    }

    IEnumerator ShootingRoutine()
    {
        while (enemyHp.currentHealth > 0)
        {
            if (playerHp.currentHp > 0 &&CanSeePlayer())
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
            if(CanSeePlayer())
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
