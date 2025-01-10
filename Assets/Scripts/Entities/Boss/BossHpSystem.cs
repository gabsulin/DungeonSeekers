using UnityEngine;
using UnityEngine.UI;

public class BossHpSystem : MonoBehaviour
{
    float maxHealth = 200;
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

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        anim.SetTrigger("Hit");
        hpBar.fillAmount = currentHealth / maxHealth;

        if (currentHealth <= 0)
        {
            Die();
        }
        if(currentHealth <= maxHealth / 2 )
        {
            anim.SetBool("IsEnraged", true);
            Debug.Log("enraged");
        }
    }

    private void Die()
    {
        Debug.Log("Die");
        anim.SetBool("Die", true);
        Destroy(gameObject, 1f);
    }
}
