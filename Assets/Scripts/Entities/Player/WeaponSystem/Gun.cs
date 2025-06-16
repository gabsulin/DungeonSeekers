using static WeaponData;
using UnityEngine;
using System.Collections;

public class Gun : Weapon
{
    public string bulletPoolTag = "Bullet";
    public Transform firePoint;
    public float bulletSpeed = 10f;
    public bool isShotgun;

    [Range(0f, 100f)] public float accuracy = 100f;

    private bool isBursting = false;
    public override void UpdateWeapon()
    {
        base.UpdateWeapon();

        switch (data.fireMode)
        {
            case FireMode.SemiAuto:
                if (Input.GetMouseButtonDown(0)) TryAttack(true);
                break;
            case FireMode.FullAuto:
                if (Input.GetMouseButton(0)) TryAttack(true);
                break;
            case FireMode.Burst:
                if (Input.GetMouseButtonDown(0))
                {
                    StartCoroutine(FireBurst());
                }
                break;
        }
    }

    protected override void Attack()
    {
        (AudioManager.Instance)?.PlaySFX("GunShot");
        if (!isShotgun)
            FireSingleShot();
        else
            FireShotgun();
    }

    private void FireSingleShot()
    {
        float maxSpreadAngle = 50f;
        float inaccuracy = Mathf.Clamp01(1f - (accuracy / 100f));
        float spread = Random.Range(-maxSpreadAngle * inaccuracy, maxSpreadAngle * inaccuracy);

        Quaternion spreadRotation = Quaternion.Euler(0, 0, spread);
        Vector2 direction = spreadRotation * firePoint.right;

        GameObject bullet = ObjectPooler.Instance.SpawnFromPool(bulletPoolTag, firePoint.position, Quaternion.identity);
        
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null) bulletScript.Fire(direction);
    }

    private void FireShotgun()
    {
        int pelletCount = 8;
        float spreadAngle = 30f;

        for (int i = 0; i < pelletCount; i++)
        {
            float angleStep = spreadAngle / (pelletCount - 1);
            float angleOffset = -spreadAngle / 2f + angleStep * i;

            Quaternion rotation = Quaternion.Euler(0, 0, angleOffset);
            Vector2 direction = rotation * firePoint.right;

            GameObject bullet = ObjectPooler.Instance.SpawnFromPool(bulletPoolTag, firePoint.position, Quaternion.identity);
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            if (bulletScript != null) bulletScript.Fire(direction);
        }
    }

    private IEnumerator FireBurst()
    {
        isBursting = true;

        for (int i = 0; i < data.burstCount; i++)
        {
            TryAttack(true);
            yield return new WaitForSeconds(data.burstDelay);
        }

        isBursting = false;
    }
}
