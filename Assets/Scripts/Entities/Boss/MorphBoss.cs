using UnityEngine;
public class MorphBoss : MonoBehaviour
{
    public float attackRange = 15f;
    public float attackCooldown = 2f;

    Transform player;
    PlayerHpSystem playerHp;
    Rigidbody2D rb;
    Animator anim;
    CircleCollider2D circleCollider;
    float lastAttackTime;

    [SerializeField] GameObject poisonAttack;
    GameObject poisonAttackSpawn;
    BossFlip bossFlip;

    [Header("Dash Settings")]
    [SerializeField] private float dashSpeed = 12f;
    [SerializeField] private float dashDuration = 0.4f;
    private bool isDashing = false;
    private float dashTimeRemaining;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerHp = FindFirstObjectByType<PlayerHpSystem>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        poisonAttackSpawn = GameObject.Find("PoisonSpawn");
        bossFlip = GetComponent<BossFlip>();
        circleCollider = GetComponent<CircleCollider2D>();
    }

    void Update()
    {
        if (isDashing)
        {
            dashTimeRemaining -= Time.deltaTime;
            if (dashTimeRemaining <= 0f)
            {
                StopDash();
            }
        }
        else
        {
            float distance = Vector2.Distance(player.position, rb.position);
            if (distance <= attackRange && Time.time >= lastAttackTime + attackCooldown)
            {
                ChooseAttack();
                lastAttackTime = Time.time;
            }
        }
    }

    void ChooseAttack()
    {
        float rand = Random.value;

        if (rand < 0.6f)
        {
            AttackDash();
        }
        else if (rand < 0.9f)
        {
            AttackPoison();
        }
        else
        {
            AttackSpikes();
        }
    }

    public void AttackPoison()
    {
        if (player == null || poisonAttack == null || poisonAttackSpawn == null) return;

        anim.SetTrigger("PoisonAttack");

        var poison = Instantiate(poisonAttack, poisonAttackSpawn.transform.position, Quaternion.identity);
        Rigidbody2D poisonRb = poison.GetComponent<Rigidbody2D>();

        if (poisonRb != null)
        {
            Vector2 direction = ((Vector2)player.position - (Vector2)poisonAttackSpawn.transform.position).normalized;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            poison.transform.rotation = Quaternion.Euler(0, 0, angle);

            poisonRb.AddForce(direction * 1.5f, ForceMode2D.Impulse);
        }
        Destroy(poison, 1);

        //apply poison effect to player
    }

    void AttackSpikes()
    {
        anim.SetTrigger("SpikesAttack");
    }

    public void EnableSpikesCollider()
    {
        //circleCollider.enabled = true;
    }

    public void DisableSpikesCollider()
    {
        //circleCollider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            playerHp.TakeHit(5);
        }
    }

    void AttackDash()
    {
        anim.SetTrigger("DashAttack");
        StartDash();
    }

    void StartDash()
    {
        isDashing = true;
        dashTimeRemaining = dashDuration;

        if (player != null)
        {
            Vector2 direction = ((Vector2)player.position - rb.position).normalized;
            rb.linearVelocity = direction * dashSpeed;
        }
    }


    void StopDash()
    {
        isDashing = false;
        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        Rigidbody2D playerRB = player.GetComponent<Rigidbody2D>();
        playerRB.linearVelocity = Vector2.zero;
    }

}


