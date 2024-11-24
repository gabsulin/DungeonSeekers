using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyAttack : MonoBehaviour
{
    EnemyMovement enemyDistance;
    PlayerObj enemy;
    HpSystem enemyHp;

    [SerializeField] public Rigidbody2D bulletPrefab;
    [SerializeField] public int damage;
    [SerializeField] public Transform bulletSpawnPoint;
    [SerializeField] public Transform player;

    void Start()
    {
        enemyDistance = GetComponent<EnemyMovement>();
        enemy = GetComponent<PlayerObj>();
        enemyHp = GetComponent<HpSystem>();

        StartCoroutine(ShootingRoutine());
    }

    void Update()
    {
        
    }

    IEnumerator ShootingRoutine()
    {
        while (enemyHp.health > 0)
        {
            Shoot();
            yield return new WaitForSeconds(0.8f);
        }
    }

    private void Shoot()
    {
        if (enemyDistance.distance <= 3)
        {
            var bullet = Instantiate(bulletPrefab, bulletSpawnPoint.transform.position, Quaternion.identity);
            Vector2 direction = (player.position - bulletSpawnPoint.position).normalized;
            bullet.AddForce(direction * 5, ForceMode2D.Impulse);
            Destroy(bullet.gameObject, 2);
        }
    }
}
