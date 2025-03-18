using UnityEngine;

public class Spikes : MonoBehaviour
{
    bool canDamage = false;
    float damageCooldown = 1f;
    float lastDamageTime = 0f;

    public void EnableDamage()
    {
        canDamage = true;
    }

    public void DisableDamage()
    {
        canDamage = false;
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (canDamage && collision.CompareTag("Player") && Time.time > lastDamageTime + damageCooldown)
        {
            collision.GetComponent<PlayerHpSystem>().TakeHit(1);
            lastDamageTime = Time.time;
        }
    }
}
