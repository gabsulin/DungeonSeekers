using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerObj : MonoBehaviour
{
    /*public enum PlayerState
    {
        idle,
        move,
        attack,
        death,
        stun
    }

    public PlayerState _playerState = PlayerState.idle;
    public SPUM_Prefabs _prefabs;
    private void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.localPosition.y * 0.01f);

        switch (_playerState)
        {
            case PlayerState.idle:
                break;

            case PlayerState.move:
                break;

            case PlayerState.attack:
                break;

            case PlayerState.stun:
                break;
        }
    }

    public void SetState(PlayerState newState)
    {
        if (_playerState != newState)
        {
            _playerState = newState;
        }
    }*/

    public enum PlayerState
    {
        idle,
        move,
        attack,
        death,
        stun
    }
    public PlayerState _playerState = PlayerState.idle;


    public SPUM_Prefabs _prefabs;
    public float _charMS;
    public Vector3 _goalPos;
    //public GameObject goalPos;
    //public GameObject notGoalPos;

    void Start()
    {

    }
    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.localPosition.y * 0.01f);
        switch (_playerState)
        {
            case PlayerState.idle:
                break;

            case PlayerState.move:
                DoMove();
                break;
        }
    }

    void DoMove()
    {
        Vector3 _dirVec = _goalPos - transform.position;
        Vector3 _disVec = (Vector2)_goalPos - (Vector2)transform.position;
        if (_disVec.sqrMagnitude < 0.1f)
        {
            _prefabs.PlayAnimation(0);
            _playerState = PlayerState.idle;
            return;
        }
        Vector3 _dirMVec = _dirVec.normalized;
        transform.position += (_dirMVec * _charMS * Time.deltaTime);


        if (_dirMVec.x > 0) _prefabs.transform.localScale = new Vector3(-1, 1, 1);
        else if (_dirMVec.x < 0) _prefabs.transform.localScale = new Vector3(1, 1, 1);
    }

    public void SetMovePos(Vector2 pos)
    {
        _goalPos = pos;

        _playerState = PlayerState.move;
        _prefabs.PlayAnimation(1);
    }
}

