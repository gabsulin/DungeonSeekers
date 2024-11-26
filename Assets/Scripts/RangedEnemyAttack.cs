using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyAttack : MonoBehaviour
{
    EnemyMovement enemyDistance;
    PlayerObj enemy;
    EnemyHpSystem enemyHp;
    PlayerHpSystem playerHp;

    [SerializeField] float offsetAngle = 50f;
    [SerializeField] public Rigidbody2D bulletPrefab;
    [SerializeField] public Transform bulletSpawnPoint;
    [SerializeField] public Transform player;

    void Start()
    {
        enemyDistance = GetComponent<EnemyMovement>();
        enemy = GetComponent<PlayerObj>();
        enemyHp = GetComponent<EnemyHpSystem>();
        playerHp = FindAnyObjectByType<PlayerHpSystem>();

        StartCoroutine(ShootingRoutine());
    }

    void Update()
    {
        
    }

    IEnumerator ShootingRoutine()
    {
        while (enemyHp.currentHealth > 0 && playerHp.currentHp > 0)
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
            Vector2 offsetDirection = RotateVector2(direction, offsetAngle);
            bullet.AddForce(offsetDirection * 5, ForceMode2D.Impulse);
            Destroy(bullet.gameObject, 2);
        }
    }

    private Vector2 RotateVector2(Vector2 vector, float angleDegrees)
    {
        float angleRadians = angleDegrees * Mathf.Deg2Rad;
        float cos = Mathf.Cos(angleRadians);
        float sin = Mathf.Sin(angleRadians);

        return new Vector2(
            vector.x * cos - vector.y * sin,
            vector.x * sin + vector.y * cos
        );
    }
}
