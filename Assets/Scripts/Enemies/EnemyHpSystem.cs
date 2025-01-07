using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHpSystem : MonoBehaviour
{
    private MapFunctionality manager;

    EnemyObj enemy;
    SPUM_Prefabs anim;

    public int maxHealth = 100;
    public int currentHealth;

    public bool stunned;


    private void Awake()
    {
        currentHealth = maxHealth;
    }
    private void Start()
    {
        enemy = GetComponent<EnemyObj>();
        anim = GetComponentInChildren<SPUM_Prefabs>();
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
        if (enemy._enemyState != EnemyObj.EnemyState.death)
        {
            anim._anim.ResetTrigger("Attack");
            anim._anim.SetFloat("RunState", 0f);
            anim._anim.SetFloat("AttackState", 0f);
            anim._anim.SetFloat("SkillState", 0f);

            enemy._enemyState = EnemyObj.EnemyState.death;

            StartCoroutine(PlayDeathAnimation());
        }

        RemoveFromList();
        if (gameObject != null)
            Destroy(gameObject, 1);
    }

    public void Stun(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }

        StartCoroutine(StunReset());
    }

    private void RemoveFromList()
    {
        MapFunctionality manager = FindFirstObjectByType<MapFunctionality>();

        if (manager != null && manager.enemies.Contains(this))
        {
            manager.enemies.Remove(this);
        }
    }

    public void SetManager(MapFunctionality manager)
    {
        this.manager = manager;
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

    private IEnumerator StunReset()
    {
        stunned = true;
        anim._anim.speed = 0.75f;
        anim.PlayAnimation(3);
        yield return new WaitForSeconds(2);
        anim._anim.speed = 1f;
        stunned = false;
    }
}
