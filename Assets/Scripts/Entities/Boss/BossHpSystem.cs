using UnityEngine;
using UnityEngine.UI;

public class BossHpSystem : MonoBehaviour
{
    [SerializeField] float maxHealth;
    [SerializeField] Color enragedColor;
    [SerializeField] bool isSpecialBoss;
    [SerializeField] Animator anim;

    [Header("Sound Settings")]
    [SerializeField] string soundName;
    [SerializeField] string musicName;
    [SerializeField] bool hasIntro;
    public float currentHealth;

    public Image hpBar;
    SpriteRenderer spriteRenderer;

    public bool isEnraged = false;
    public bool isDamagable = true;
    bool isHealing = false;
    public bool isDead = false;
    public bool hasPhaseTwo;
    private void Awake()
    {
        currentHealth = maxHealth;
    }
    void Start()
    {
        if (hpBar != null)
        {
            hpBar.fillAmount = maxHealth / maxHealth;
        }
        anim.GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        if (isHealing)
        {
            hpBar.fillAmount = currentHealth / maxHealth;
        }
    }
    public void TakeDamage(int damage, bool isMelee)
    {
        if (isEnraged && isSpecialBoss && !isMelee)
        {
            Debug.Log("Boss is only damagable by melee weapons!");
            return;
        }

        if (isDamagable)
        {
            currentHealth -= damage;
            if (currentHealth < 0) currentHealth = 0;
            anim.ResetTrigger("Attack");
            anim.SetTrigger("Hit");
            hpBar.fillAmount = currentHealth / maxHealth;

            if (currentHealth <= 0)
            {
                Die();
            }
            if (hasPhaseTwo && !isEnraged && currentHealth <= maxHealth / 2)
            {
                AudioManager.Instance.PlaySFX("Phase2");
                isEnraged = true;
                anim.SetBool("IsEnraged", true);
                spriteRenderer.color = enragedColor;
            }
        }
        else
        {
            return;
        }
    }
    public void Heal(int amount, float time)
    {
        if(anim.GetBool("IsImmune") == true)
        {
            currentHealth = Mathf.Lerp(currentHealth, currentHealth + amount, time);
            isHealing = true;
        }
    }
    public void StopHealing()
    {
        isHealing = false;
    }
    private void Die()
    {
        AudioManager.Instance.PlayMusic(musicName, hasIntro);
        AudioManager.Instance.PlaySFX(name);
        anim.SetBool("Die", true);
        isDead = true;
    }

    public void DestroyBoss()
    {
        Destroy(gameObject);
    }
}
