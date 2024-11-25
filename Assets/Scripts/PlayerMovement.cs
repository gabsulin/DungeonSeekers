using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    public Tilemap collisionTilemap;
    public Vector3 startPos;
    public PlayerObj player;
    //public GameObject goalPos;
    private SPUM_Prefabs anim;
    //Vector2 offset = new Vector2(0, 0.2f);
    void Start()
    {
        player = GetComponent<PlayerObj>();
        anim = GetComponent<SPUM_Prefabs>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            /*Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = goalPos.transform.position.z;
            goalPos.transform.position = mousePosition;*/

            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null && hit.collider.gameObject.layer == LayerMask.NameToLayer("Walking"))
            {
                if (player != null)
                {
                    Vector2 goalPos = hit.point;
                    if (IsPathClear(goalPos))
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

                anim.PlayAnimation(4);
            }
        }

    }

    private bool IsPathClear(Vector2 goalPos)
    {
        Vector3 direction = goalPos - (Vector2)transform.position;
        float distance = direction.magnitude;
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction.normalized, distance);

        foreach (var hit in hits)
        {
            if (hit.collider != null && hit.collider.gameObject.layer == LayerMask.NameToLayer("Collision"))
                return false;
        }

        return true;
    }
}
