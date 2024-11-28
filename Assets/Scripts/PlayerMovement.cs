using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    public Vector3 startPos;
    public PlayerObj player;
    private SPUM_Prefabs anim;
    private PlayerHpSystem playerHp;
    
    void Start()
    {
        player = GetComponent<PlayerObj>();
        playerHp = GetComponent<PlayerHpSystem>();
        anim = GetComponent<SPUM_Prefabs>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null && hit.collider.gameObject.layer == LayerMask.NameToLayer("Walking"))
            {
                if (player != null && playerHp.currentHp > 0)
                {
                    Vector2 goalPos = hit.point;
                    if (IsPathClear())
                    {
                        player.SetMovePos(goalPos);
                    }

                }
            }
        }
        if (Input.GetButtonDown("Jump"))
        {
            player._playerState = PlayerObj.PlayerState.attack;
            if (player._playerState == PlayerObj.PlayerState.attack)
            {
                anim._anim.SetFloat("RunState", 0f);
                anim.PlayAnimation(4);
            }
        }

    }
    private bool IsPathClear()
    {
        RaycastHit2D raycastHit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        RaycastHit2D[] linecastHits = Physics2D.LinecastAll(player.transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));

        foreach(RaycastHit2D hit in linecastHits)
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Collision"))
            {
                return false;
            }
        }
        return true;
    }
}
