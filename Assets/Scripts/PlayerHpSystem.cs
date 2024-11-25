using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHpSystem : MonoBehaviour
{
    PlayerObj player;
    SPUM_Prefabs anim;

    [SerializeField] Image hpBar;
    [SerializeField] Image shieldsBar;
    [SerializeField] private float maxHp = 5;
    private float currentHp;
    void Start()
    {
        player = GetComponent<PlayerObj>();
        anim = GetComponent<SPUM_Prefabs>();
        currentHp = maxHp;
    }

    public void TakeHit(int damage)
    {
        currentHp -= damage;
        hpBar.fillAmount = currentHp / 5;
        Debug.Log(currentHp);
        Debug.Log(hpBar.fillAmount);
        if (currentHp <= 0)
        {
            Die();
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
}
