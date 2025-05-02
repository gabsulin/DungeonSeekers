using UnityEngine;
using UnityEngine.UI;

public class BossHpSystem : MonoBehaviour
{
    [SerializeField] float maxHealth;
    [SerializeField] Color enragedColor;
    [SerializeField] bool isSpecialBoss;
    public float currentHealth;

    public Image hpBar;
    Animator anim;
    SpriteRenderer spriteRenderer;
    bool isEnraged = false;
    private void Awake()
    {
        currentHealth = maxHealth;
    }
    void Start()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (hpBar != null)
        {
            hpBar.fillAmount = maxHealth / maxHealth;
        }
    }
    public void TakeDamage(int damage, bool isMelee)
    {
        if (isEnraged && isSpecialBoss && !isMelee)
        {
            Debug.Log("Boss is only damagable by melee weapons!");
            return;
        }

        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;
        anim.ResetTrigger("Attack");
        anim.SetTrigger("Hit");
        hpBar.fillAmount = currentHealth / maxHealth;

        if (currentHealth <= 0)
        {
            Die();
        }
        if (!isEnraged && currentHealth <= maxHealth / 2)
        {
            isEnraged = true;
            anim.SetBool("IsEnraged", true);
            spriteRenderer.color = enragedColor;
        }
    }

    private void Die()
    {
        anim.SetBool("Die", true);
    }

    public void DestroyBoss()
    {
        Destroy(gameObject);
    }
}
