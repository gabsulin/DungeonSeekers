using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHpSystem : MonoBehaviour
{
    PlayerObj enemy;
    SPUM_Prefabs anim;

    public int maxHealth = 100;
    public int currentHealth;

    private void Start()
    {
        enemy = GetComponent<PlayerObj>();
        anim = GetComponent<SPUM_Prefabs>();
        currentHealth = maxHealth;
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (enemy._playerState != PlayerObj.PlayerState.death)
        {
            anim._anim.ResetTrigger("Attack");
            anim._anim.SetFloat("RunState", 0f);
            anim._anim.SetFloat("AttackState", 0f);
            anim._anim.SetFloat("SkillState", 0f);

            enemy._playerState = PlayerObj.PlayerState.death;

            StartCoroutine(PlayDeathAnimation());

        }

        Destroy(gameObject, 1);
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
