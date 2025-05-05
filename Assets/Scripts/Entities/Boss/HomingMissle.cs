using UnityEngine;

public class HomingMissle : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float rotateSpeed;
    [SerializeField] float lifeTime;

    int health = 2;

    Transform target;
    Rigidbody2D rb;
    PlayerHpSystem playerHpSystem;
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();

        Destroy(gameObject, lifeTime);
    }

    private void FixedUpdate()
    {
        if (target == null) return;

        Vector2 direction = (Vector2)target.position - rb.position;
        direction.Normalize();

        float rotateAmount = Vector3.Cross(direction, transform.right).z;

        rb.angularVelocity = -rotateAmount * rotateSpeed;
        rb.linearVelocity = transform.right * speed;
    }
    void TakeHit(int damage)
    {
        health -= damage;
        if(health < 0) health = 0;
        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            TakeHit(1);
        } else if (collision.CompareTag("Melee"))
        {
            TakeHit(2);
        }
    }
}
