using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public PlayerObj player;
    public PlayerObj enemy;
    public float distance;
    HpSystem enemyHp;

    private SPUM_Prefabs anim;
    void Start()
    {
        enemy = GetComponent<PlayerObj>();
        anim = GetComponent<SPUM_Prefabs>();
        enemyHp = GetComponent<HpSystem>();
    }

    void Update()
    {
        distance = Vector2.Distance(transform.position, player.transform.position);

        if (enemy != null)
        {
            if (enemyHp.health > 0)
            {
                if (distance > 2 && distance < 4)
                {
                    Vector2 goalPos = player.transform.position;
                    enemy.SetMovePos(goalPos);
                    if (enemy._playerState != PlayerObj.PlayerState.move)
                    {
                        enemy._playerState = PlayerObj.PlayerState.move;
                        anim.PlayAnimation(1);
                    }
                }
                else
                {
                    if (enemy._playerState != PlayerObj.PlayerState.idle)
                    {
                        enemy._playerState = PlayerObj.PlayerState.idle;
                        anim.PlayAnimation(0);
                    }
                }

                if (distance <= 3)
                {
                    enemy._playerState = PlayerObj.PlayerState.attack;
                    anim.PlayAnimation(6);
                }
            }
            else // Enemy is dead
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
            }
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
