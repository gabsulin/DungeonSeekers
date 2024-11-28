using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public PlayerObj player;
    public PlayerObj enemy;
    public float distance;
    EnemyHpSystem enemyHp;
    PlayerHpSystem playerHp;

    private SPUM_Prefabs anim;
    void Start()
    {
        enemy = GetComponent<PlayerObj>();
        anim = GetComponent<SPUM_Prefabs>();
        enemyHp = GetComponent<EnemyHpSystem>();
        playerHp = FindAnyObjectByType<PlayerHpSystem>();
    }

    void Update()
    {
        distance = Vector2.Distance(transform.position, player.transform.position);

        if (enemy != null)
        {
            if (enemyHp.currentHealth > 0)
            {
                if (distance > 2 && distance <= 8)
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

                if (distance <= 6 && playerHp.currentHp > 0)
                {
                    enemy._playerState = PlayerObj.PlayerState.attack;
                    anim.PlayAnimation(6);
                }
            }
        }
    }
}
