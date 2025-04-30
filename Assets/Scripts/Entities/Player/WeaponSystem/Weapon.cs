using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public WeaponData data;

    [SerializeField] protected bool allowHoldToFire = false;

    protected float attackTimer = 0f;
    public bool IsAttacking => attackTimer > 0;

    public virtual void UpdateWeapon()
    {
        if (attackTimer > 0f)
            attackTimer -= Time.deltaTime;
    }

    public void TryAttack(bool isButtonHeld)
    {
        if (attackTimer <= 0f)
        {
            if (allowHoldToFire && isButtonHeld || !allowHoldToFire && isButtonHeld)
            {
                Attack();
                attackTimer = data.attackCooldown;
            }
        }
    }

    protected abstract void Attack();
}

