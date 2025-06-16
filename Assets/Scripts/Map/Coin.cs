using UnityEngine;
using DG.Tweening;

public class Coin : MonoBehaviour
{
    Transform _target;
    private Rigidbody2D rb;
    private bool isMoving = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        _target = CoinManager.instance.target;
    }
    public void SetTarget(Transform target)
    {
        this._target = target;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isMoving)
        {
            MoveToTarget();
            CoinManager.instance.AddCoin(1);
            (AudioManager.Instance)?.PlaySFX("Coin");
        }
    }

    public void MoveToTarget()
    {
        gameObject.GetComponent<CircleCollider2D>().enabled = false;
        if (_target == null || isMoving) return;

        isMoving = true;
        rb = GetComponent<Rigidbody2D>();

        if (rb != null && _target != null)
        {
            rb.gravityScale = 0;
            rb.linearVelocity = Vector2.zero;

            Vector3 screenPos = Camera.main.ScreenToWorldPoint(transform.position);
            Vector3 targetPos = _target.position;

            transform.DOMove(targetPos, 3f).SetEase(Ease.InQuad).OnComplete(() => Destroy(gameObject));
        }
    }

    public void StartFloatingAnimation()
    {
        float floatDistance = 0.2f;
        float floatDuration = 0.5f;

        transform.DOMoveY(transform.position.y - floatDistance, floatDuration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }
}
