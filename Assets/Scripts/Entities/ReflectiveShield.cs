using UnityEngine;

public class ReflectiveShield : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyBullet"))
        {
            Rigidbody2D projRb = collision.GetComponent<Rigidbody2D>();
            if (projRb != null)
            {
                Vector2 incomingVelocity = projRb.linearVelocity;
                Vector2 normal = (collision.transform.position - transform.position).normalized;
                Vector2 reflectedVelocity = Vector2.Reflect(incomingVelocity, normal);

                projRb.linearVelocity = reflectedVelocity;

                float angle = Mathf.Atan2(reflectedVelocity.y, reflectedVelocity.x) * Mathf.Rad2Deg;
                collision.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
        }
    }
}
