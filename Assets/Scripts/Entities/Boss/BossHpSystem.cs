using UnityEngine;
using UnityEngine.UI;

public class BossHpSystem : MonoBehaviour
{
    [SerializeField] float maxHealth;
    public float currentHealth;

    public Image hpBar;
    Animator anim;
    private void Awake()
    {
        currentHealth = maxHealth;
    }
    void Start()
    {
        anim = GetComponent<Animator>();
        if(hpBar != null )
        {
            hpBar.fillAmount = maxHealth / maxHealth;
        }
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if( currentHealth < 0 ) currentHealth = 0;
        anim.ResetTrigger("Attack");
        anim.SetTrigger("Hit");
        hpBar.fillAmount = currentHealth / maxHealth;

        if (currentHealth <= 0)
        {
            Die();
        }
        if(currentHealth <= maxHealth / 2 )
        {
            anim.SetBool("IsEnraged", true);
        }
    }

    private void Die()
    {
        Debug.Log("deda");
        anim.SetBool("Die", true);
    }
}
