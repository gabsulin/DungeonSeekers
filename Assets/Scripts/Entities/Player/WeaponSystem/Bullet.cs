using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifetime = 2.5f;
    private EnemyDamage damageScript;

    private Rigidbody2D rb;
    private float timer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        damageScript = GetComponent<EnemyDamage>();
    }

    private void OnEnable()
    {
        timer = lifetime;
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            Deactivate();
        }
    }

    public void Fire(Vector2 direction)
    {
        rb.linearVelocity = direction.normalized * speed;
    }

    public void Deactivate()
    {
        rb.linearVelocity = Vector2.zero;
        ObjectPooler.Instance.ReturnToPool("Bullet", gameObject);
    }
}
