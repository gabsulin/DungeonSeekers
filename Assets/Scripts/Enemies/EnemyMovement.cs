using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.VersionControl;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private PlayerObj player;
    private EnemyObj enemy;
    [HideInInspector] public float distance;
    EnemyHpSystem enemyHp;
    PlayerHpSystem playerHp;

    private SPUM_Prefabs anim;
    void Start()
    {
        player = FindAnyObjectByType<PlayerObj>();
        enemy = GetComponent<EnemyObj>();
        anim = GetComponentInChildren<SPUM_Prefabs>();
        enemyHp = GetComponent<EnemyHpSystem>();
        playerHp = FindAnyObjectByType<PlayerHpSystem>();
    }

    void Update()
    {
        if(player != null)
        {
            distance = Vector2.Distance(transform.position, player.transform.position);
        }
        

        if (enemy != null && player != null)
        {
            if (enemyHp.currentHealth > 0)
            {
                if (distance > 2 && distance <= 8)
                {
                    Vector2 goalPos = player.transform.position;
                    enemy.SetMovePos(goalPos);
                    if (enemy._enemyState != EnemyObj.EnemyState.move)
                    {
                        enemy._enemyState = EnemyObj.EnemyState.move;
                        anim.PlayAnimation(1);
                    }
                }
                else
                {
                    if (enemy._enemyState != EnemyObj.EnemyState.idle)
                    {
                        enemy._enemyState = EnemyObj.EnemyState.idle;
                        anim.PlayAnimation(0);
                    }
                }

                if (distance <= 6 && playerHp.currentHp > 0)
                {
                    enemy._enemyState = EnemyObj.EnemyState.attack;
                    anim.PlayAnimation(6);
                }
            }
        }
    }
}
