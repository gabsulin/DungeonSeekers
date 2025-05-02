using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public WeaponData data;

    [SerializeField] public bool allowHoldToFire = false;

    protected float attackTimer = 0f;
    public bool IsAttacking => attackTimer > 0;

    public virtual void UpdateWeapon()
    {
        if (attackTimer > 0f)
            attackTimer -= Time.deltaTime;
    }

    public void TryAttack(bool trigger)
    {
        if (attackTimer <= 0f && trigger)
        {
            Attack();
            attackTimer = data.attackCooldown;
        }
    }

    protected abstract void Attack();
}

