using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHpSystem : MonoBehaviour
{
    PlayerObj player;
    SPUM_Prefabs anim;
    public CharacterData characterData;

    [SerializeField] Image hpBar;
    [SerializeField] Image shieldsBar;
    [SerializeField] TMP_Text hpTMP;
    [SerializeField] TMP_Text shieldsTMP;

    [HideInInspector] public float currentHp;
    [HideInInspector] public float currentShields;

    private float wasntHit = 0f;
    private bool isRegeneratingShields = false;

    private float shieldRegenTime = 5f;

    public bool isDead;

    private void Awake()
    {
        currentHp = characterData.health;
        currentShields = characterData.shields;

        isDead = false;
    }

    public void AssignUIElements()
    {
        GameObject canvas = GameObject.Find("Canvas");

        if (canvas != null)
        {
            hpBar = canvas.transform.Find("HealthBar/Health").GetComponent<Image>();
            hpTMP = canvas.transform.Find("HealthBar/HpAmount").GetComponent<TMP_Text>();
            shieldsBar = canvas.transform.Find("Shieldbar/Shields").GetComponent<Image>();
            shieldsTMP = canvas.transform.Find("Shieldbar/ShieldsAmount").GetComponent<TMP_Text>();
        }
        else
        {
            Debug.LogError("Canvas not found! Make sure your Canvas is named correctly.");
        }
    }

    public void UpdateUI()
    {
        if (hpBar != null && shieldsBar != null && hpTMP != null && shieldsTMP != null)
        {
            hpBar.fillAmount = currentHp / characterData.health;
            hpTMP.text = $"{currentHp}/{characterData.health}";
            shieldsBar.fillAmount = currentShields / characterData.shields;
            shieldsTMP.text = $"{currentShields}/{characterData.shields}";
        }
        else
        {
            Debug.LogError("UI Elements are not assigned in PlayerHpSystem!");
        }
    }
    void Start()
    {
        player = GetComponent<PlayerObj>();
        anim = GetComponentInChildren<SPUM_Prefabs>();
    }

    private void Update()
    {
        wasntHit += Time.deltaTime;

        if (currentShields < characterData.shields && wasntHit >= shieldRegenTime && !isRegeneratingShields && !isDead)
        {
            StartCoroutine(RegenerateShields());
        }
    }

    public void TakeHit(int damage)
    {
        wasntHit = 0;

        if (currentShields > 0)
        {
            int overflowDmg = damage - (int)currentShields;

            currentShields -= damage;

            if(currentShields < 0) currentShields = 0;

            shieldsBar.fillAmount = currentShields / characterData.shields;
            shieldsTMP.text = $"{currentShields.ToString()}/{characterData.shields.ToString()}";

            if(overflowDmg > 0)
            {
                currentHp -= overflowDmg;

                hpBar.fillAmount = currentHp / characterData.health;
                hpTMP.text = $"{currentHp.ToString()}/{characterData.health.ToString()}";

                if(currentHp <= 0)
                {
                    currentHp = 0;
                    hpTMP.text = $"{currentHp.ToString()}/{characterData.health.ToString()}";
                    Die();
                }
            }
        }
        else
        {
            currentHp -= damage;
            hpBar.fillAmount = currentHp / characterData.health;
            hpTMP.text = $"{currentHp.ToString()}/{characterData.health.ToString()}";
            if (currentHp <= 0)
            {
                currentHp = 0;
                hpTMP.text = $"{currentHp.ToString()}/{characterData.health.ToString()}";
                Die();
            }
        }
        //Debug.Log(wasntHit);
    }

    private void Die()
    {
        if (player._playerState != PlayerObj.PlayerState.death)
        {
            anim._anim.ResetTrigger("Attack");
            anim._anim.SetFloat("RunState", 0f);
            anim._anim.SetFloat("AttackState", 0f);
            anim._anim.SetFloat("SkillState", 0f);

            player._playerState = PlayerObj.PlayerState.death;

            StartCoroutine(PlayDeathAnimation());

            anim._anim.SetTrigger("Die");
            anim._anim.SetBool("EditChk", anim.EditChk);
        }
        isDead = true;

        /*
         deathScreen.SetActive(true);
         */
    }

    private IEnumerator PlayDeathAnimation()
    {
        anim._anim.speed = 1;
        anim.PlayAnimation(2);
        float animationLength = anim._anim.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(animationLength / 2f / anim._anim.speed);
        anim._anim.speed = 1;
        yield return new WaitForSeconds(animationLength / 2f);
    }

    private IEnumerator RegenerateShields()
    {
        isRegeneratingShields = true;

        while (currentShields < characterData.shields && !isDead && wasntHit >= shieldRegenTime)
        {
            currentShields += 1;
            shieldsBar.fillAmount = currentShields / characterData.shields;
            shieldsTMP.text = $"{currentShields.ToString()}/{characterData.shields.ToString()}";

            yield return new WaitForSeconds(2);
        }

        currentShields = Mathf.Min(currentShields, characterData.shields);
        isRegeneratingShields = false;
    }
}