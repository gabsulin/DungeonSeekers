using Unity.VisualScripting;
using UnityEngine;

public class Gun : Weapon
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 10f;
    public bool isShotgun;

    protected override void Attack()
    {
        if (bulletPrefab == null || firePoint == null) return;

        if (!isShotgun)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = firePoint.right * bulletSpeed;
            }
        }
        else
        {
            int pelletCount = 8;
            float spreadAngle = 30f;

            for (int i = 0; i < pelletCount; i++)
            {
                float angleStep = spreadAngle / (pelletCount - 1);
                float angleOffset = -spreadAngle / 2f + angleStep * i;

                Quaternion rotation = Quaternion.Euler(0, 0, angleOffset);
                Vector2 direction = rotation * firePoint.right;

                GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.linearVelocity = direction.normalized * bulletSpeed;
                }
            }
        }

        // AudioManager.Instance.PlaySFX("GunShot");
    }

}