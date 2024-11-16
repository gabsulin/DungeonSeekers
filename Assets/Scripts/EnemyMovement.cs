using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public PlayerObj player;
    public PlayerObj enemy;
    public float distance;

    private SPUM_Prefabs anim;
    void Start()
    {
        enemy = GetComponent<PlayerObj>();
        anim = GetComponent<SPUM_Prefabs>();
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector2.Distance(transform.position, player.transform.position);

        if (enemy != null)
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

            if(distance <= 3)
            {
                enemy._playerState= PlayerObj.PlayerState.attack;
                anim.PlayAnimation(6);
            }
        }

    }
}
