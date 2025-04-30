using System.Collections.Generic;
using UnityEngine;

public class GinMagic : MonoBehaviour
{
    PlayerHpSystem playerHp;
    Transform player;
    public float pullSpeed;
    public float pullDuration;
    public float pullRadius;
    public float explosionForce;
    public float explosionRadius;

    public float maxTravelDistance = 3f;
    private Dictionary<Collider2D, Vector2> playerInitialPositions = new Dictionary<Collider2D, Vector2>();

    bool isPulling = false;
    float pullTimer = 0f;

    private void Start()
    {
        playerHp = FindFirstObjectByType<PlayerHpSystem>();
        player = FindFirstObjectByType<PlayerController>().transform;
    }

    private void Update()
    {
        float distnace = Vector2.Distance(transform.position, player.position);
        if (isPulling && player != null && distnace < pullRadius)
        {
            pullTimer = Time.deltaTime;
            if (pullTimer < pullDuration)
            {
                Vector2 direction = (transform.position - player.position).normalized;

                float randomOffsetX = Random.Range(-0.1f, 0.1f);
                float randomOffsetY = Random.Range(-0.1f, 0.1f);
                Vector2 offset = new Vector2(randomOffsetX, randomOffsetY);
                direction += offset;

                direction = direction.normalized;
                player.position += (Vector3)direction * pullSpeed * Time.deltaTime;
            }
            else
            {
                isPulling = false;
                pullTimer = 0f;
            }
        }
    }
    public void ActivatePull()
    {
        isPulling = true;
        pullTimer = 0f;
    }

    public void Explode()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                float distance = Vector2.Distance(hit.transform.position, transform.position);
                float distanceRatio = Mathf.Clamp01(1 - (distance / explosionRadius));
                int damage = Mathf.RoundToInt(distanceRatio * 8);

                Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    Vector2 forceDirection = (hit.transform.position - transform.position).normalized;
                    rb.AddForce(forceDirection * explosionForce, ForceMode2D.Impulse); //udelat to aby se boss porad hybal a nikdy nestal

                    if (!playerInitialPositions.ContainsKey(hit))
                    {
                        playerInitialPositions.Add(hit, hit.transform.position);
                    }
                }

                //playerHp.TakeHit(damage);
            }
        }
    }

    void FixedUpdate()
    {
        foreach (var player in playerInitialPositions)
        {
            if (player.Key != null)
            {
                float distanceTraveled = Vector2.Distance(player.Value, player.Key.transform.position);
                if (distanceTraveled > maxTravelDistance)
                {
                    Rigidbody2D rb = player.Key.GetComponent<Rigidbody2D>();
                    if (rb != null)
                    {
                        rb.linearVelocity = Vector2.zero;
                    }
                }
            }
        }
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, pullRadius);
    }
}