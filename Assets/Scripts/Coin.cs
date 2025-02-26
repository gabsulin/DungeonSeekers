using UnityEngine;
using DG.Tweening;

public class Coin : MonoBehaviour
{
    private Transform _target;
    private Rigidbody2D rb;
    private bool isMoving = false;

    public void SetTarget(Transform target)
    {
        this._target = target;
    }

    public void MoveToTarget()
    {
        if (_target == null || isMoving) return;

        isMoving = true;
        rb = GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            Debug.Log("rigidbody");
            rb.gravityScale = 0;
            rb.linearVelocity = Vector2.zero;

            Vector3 screenPos = Camera.main.ScreenToWorldPoint(transform.position);
            Vector3 targetPos = _target.position;

            transform.DOMove(targetPos, 3f).SetEase(Ease.InQuad).OnComplete(() => Destroy(gameObject));
        }
    }
}
