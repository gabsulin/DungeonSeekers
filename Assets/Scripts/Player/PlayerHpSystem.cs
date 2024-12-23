using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHpSystem : MonoBehaviour
{
    PlayerObj player;
    SPUM_Prefabs anim;

    [SerializeField] Image hpBar;
    [SerializeField] Image shieldsBar;
    [SerializeField] TMP_Text hpTMP;
    [SerializeField] TMP_Text shieldsTMP;

    [SerializeField] private float maxHp = 5;
    [SerializeField] private float maxShields = 5;
    [HideInInspector] public float currentHp;
    [HideInInspector] public float currentShields;

    private float wasntHit = 0f;
    private bool isRegeneratingShields = false;

    private float shieldRegenTime = 5f;
    void Start()
    {
        player = GetComponent<PlayerObj>();
        anim = GetComponentInChildren<SPUM_Prefabs>();
        currentHp = maxHp;
        currentShields = maxShields;
    }

    private void Update()
    {
        // Increment wasntHit every frame
        wasntHit += Time.deltaTime;

        // Trigger shield regeneration if conditions are met
        if (currentShields < maxShields && wasntHit >= shieldRegenTime && !isRegeneratingShields)
        {
            StartCoroutine(RegenerateShields());
        }
    }

    public void TakeHit(int damage)
    {
        wasntHit = 0;

        if (currentShields > 0)
        {
            currentShields -= damage;
            shieldsBar.fillAmount = currentShields / 5;
            shieldsTMP.text = $"{currentShields.ToString()}/{maxShields.ToString()}";
            if (currentShields <= 0)
            {
                currentShields = 0;
                shieldsTMP.text = $"{currentShields.ToString()}/{maxShields.ToString()}";
            }
        }
        else
        {
            currentHp -= damage;
            hpBar.fillAmount = currentHp / 5;
            hpTMP.text = $"{currentHp.ToString()}/{maxHp.ToString()}";
            if (currentHp <= 0)
            {
                currentHp = 0;
                hpTMP.text = $"{currentHp.ToString()}/{maxHp.ToString()}";
                Die();
            }
        }

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

        }
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

        while (currentShields < maxShields)
        {
            currentShields += 1;
            shieldsBar.fillAmount = currentShields / maxShields;
            shieldsTMP.text = $"{currentShields.ToString()}/{maxShields.ToString()}";

            yield return new WaitForSeconds(2);
        }

        currentShields = Mathf.Min(currentShields, maxShields);
        isRegeneratingShields = false;
    }
}
