using UnityEngine;

public class EnemyObj : MonoBehaviour
{
    public enum EnemyState
    {
        idle,
        move,
        attack,
        death,
        stun
    }
    public EnemyState _enemyState = EnemyState.idle;


    public SPUM_Prefabs _prefabs;
    public float _charMS; // move speed
    public Vector3 _goalPos;
    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.localPosition.y * 0.01f);
        switch (_enemyState)
        {
            case EnemyState.idle:
                break;

            case EnemyState.move:
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
            _enemyState = EnemyState.idle;
            return;
        }
        Vector3 _dirMVec = _dirVec.normalized;
        transform.position += (_dirMVec * _charMS * Time.deltaTime);


        //if (_dirMVec.x > 0) _prefabs.transform.localScale = new Vector3(-1, 1, 1);
        //else if (_dirMVec.x < 0) _prefabs.transform.localScale = new Vector3(1, 1, 1);
    }

    public void SetMovePos(Vector2 pos)
    {
        _goalPos = pos;
        _enemyState = EnemyState.move;
        _prefabs.PlayAnimation(1);
    }
}
