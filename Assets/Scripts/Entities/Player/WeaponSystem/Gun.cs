using UnityEngine;

public class Gun : Weapon
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 10f;
    public bool isShotgun;

    [Range(0f, 100f)] public float accuracy = 100f;

    protected override void Attack()
    {
        if (bulletPrefab == null || firePoint == null) return;

        if (!isShotgun)
        {
            float maxSpreadAngle = 20f;
            float inaccuracy = Mathf.Clamp01(1f - (accuracy / 100f));
            float spread = Random.Range(-maxSpreadAngle * inaccuracy, maxSpreadAngle * inaccuracy);

            Quaternion spreadRotation = Quaternion.Euler(0, 0, spread);
            Vector2 direction = spreadRotation * firePoint.right;

            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = direction.normalized * bulletSpeed;
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
