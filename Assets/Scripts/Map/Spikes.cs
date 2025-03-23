using UnityEngine;

public class Spikes : MonoBehaviour
{
    public float damageInterval = 0.75f;
    public int damageAmount = 1;
    private bool isDamageEnabled = false;
    private float damageTimer = 0f;
    private GameObject player;

    void Update()
    {
        if (isDamageEnabled && player != null)
        {
            damageTimer += Time.deltaTime;
            if (damageTimer >= damageInterval)
            {
                damageTimer = 0f;
                DealDamage();
            }
        }
    }

    public void EnableDamage()
    {
        isDamageEnabled = true;
    }

    public void DisableDamage()
    {
        isDamageEnabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = collision.gameObject;
        }
    }

    private void DealDamage()
    {
        if (player != null)
        {
            player.GetComponent<PlayerHpSystem>().TakeHit(damageAmount);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = null;
        }
    }
}