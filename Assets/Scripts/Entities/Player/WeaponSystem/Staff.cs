using UnityEngine;

public class Staff : Weapon
{
    public Animator animator;
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float projectileSpeed = 8f;

    protected override void Attack()
    {
        if (projectilePrefab != null && firePoint != null)
        {
            GameObject proj = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = firePoint.right * projectileSpeed;
            }
        }
    }
}
