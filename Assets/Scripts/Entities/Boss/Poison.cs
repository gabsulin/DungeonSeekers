using UnityEngine;

public class Poison : MonoBehaviour
{
    public float damageInterval = 0.25f;
    public int damageAmount = 1;
    private float damageTimer = 0f;
    private GameObject player;

    void Update()
    {
        if (player != null)
        {
            damageTimer += Time.deltaTime;
            if (damageTimer >= damageInterval)
            {
                damageTimer = 0f;
                DealDamage();
            }
        }
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
            Debug.Log("damage");
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
