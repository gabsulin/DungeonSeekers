using System.Collections;
using UnityEngine;

public class MechGolem : MonoBehaviour
{
    [Header("Attack Components")]
    [SerializeField] private GameObject laserPrefab;
    [SerializeField] private Transform laserSpawnPoint;
    [SerializeField] private Rigidbody2D missilePrefab;
    [SerializeField] private Transform missileSpawnPoint;

    [Header("Dash Settings")]
    [SerializeField] private float dashSpeed = 10f;
    [SerializeField] private float maxDashDuration = 5f;
    [SerializeField] private LayerMask bounceLayers;
    [SerializeField] private float bounceCooldown = 0.1f;

    [Header("Laser Settings")]
    [SerializeField] private float laserDuration = 2f;

    [SerializeField] GameObject winScreen;

    private Rigidbody2D rb;
    private Transform player;
    private Animator animator;
    private BossHpSystem bossHp;
    private PlayerHpSystem playerHp;
    private CameraShake shake;
    private GameObject laser;

    private Vector2 dashDirection;
    private bool isDashing = false;
    private float dashDuration = 0f;
    private float lastBounceTime = -1f;

    private float attackCooldown = 0f;

    private bool isCastingLaser = false;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        animator = GetComponent<Animator>();
        bossHp = GetComponent<BossHpSystem>();
        playerHp = FindFirstObjectByType<PlayerHpSystem>();
        shake = GetComponent<CameraShake>();

        dashDuration = animator.GetFloat("DashDuration");
        attackCooldown = animator.GetFloat("AttackCooldown");

        StartCoroutine(InitialAttackDelay());
    }

    private void Update()
    {
        attackCooldown += Time.deltaTime;
        animator.SetFloat("AttackCooldown", attackCooldown);

        //mozna protoze je pred nim prekazka tak se nedokaze rozhodnout co ma delat kdyz ma dashoavt


        if (attackCooldown > 1f && !isDashing && !isCastingLaser && !playerHp.isDead)
        {
            if (!bossHp.isEnraged)
            {
                ChooseAttack();
            }
            else
            {
                if (animator.GetBool("IsImmune") == false) ChooseEnragedAttack();
            }
        }

        if (isDashing)
        {
            dashDuration += Time.deltaTime;
            animator.SetFloat("DashDuration", dashDuration);

            if (dashDuration >= maxDashDuration && !playerHp.isDead)
            {
                StopDash();
            }
        }

        if (bossHp.isDead)
        {
            StopDash();
            StartCoroutine(StopLaserAfterDuration(laser, 0.1f));
            /*var canvas = GameObject.Find("Canvas");
            Instantiate(winScreen, canvas.transform);
            Destroy(GameObject.Find("Player"));
            var persistance = GameObject.Find("Persistence");
            var inventory = persistance.transform.Find("InventoryCanvas(Clone)");
            Destroy(inventory.gameObject);*/
        }
    }

    private void FixedUpdate()
    {
        if (isDashing && !isCastingLaser)
        {
            rb.linearVelocity = dashDirection * dashSpeed;
        }
    }

    private IEnumerator InitialAttackDelay()
    {
        yield return new WaitForSeconds(1f);
        StartDashAttack();
    }

    public void ChooseAttack()
    {
        if (isCastingLaser) return;

        if (!bossHp.isDead)
        {
            float rand = Random.value;
            animator.SetTrigger("Attack");

            if (rand < 0.5f)
            {
                StartLaserAttack();
                (AudioManager.Instance)?.PlaySFX("Laser");
            }
            else
            {
                StartDashAttack();
                (AudioManager.Instance)?.PlaySFX("MechCharging");
            }
        }
    }

    public void ChooseEnragedAttack()
    {
        if (!bossHp.isDead)
        {
            float rand = Random.value;
            if (rand < 0.45f)
            {
                animator.SetTrigger("Attack");
                animator.SetTrigger("MissleAttack");
            }
            else if (rand < 0.9f)
            {
                animator.SetTrigger("Attack");
                StartLaserAttack();
                (AudioManager.Instance)?.PlaySFX("Laser");
            }
            else
            {
                ActivateImmunity();
            }
        }
    }

    public void StartDashAttack()
    {
        isDashing = true;
        dashDuration = 0f;
        SetDashDirectionToPlayer();
    }

    private void StopDash()
    {
        (AudioManager.Instance)?.sfxSource.Stop();
        rb.linearVelocity = Vector2.zero;
        isDashing = false;
        animator.SetFloat("DashDuration", maxDashDuration + 1f);
        animator.ResetTrigger("Attack");
        attackCooldown = 0f;
    }

    private void SetDashDirectionToPlayer()
    {
        if (player == null) return;

        dashDirection = (player.position - transform.position).normalized;
    }
    private void SetDashDirectionFromPlayer()
    {
        if (player == null) return;

        Vector2 baseDirection = (transform.position - player.position).normalized;

        float angleOffset = Random.Range(-60, 60);

        float angleRad = angleOffset * Mathf.Deg2Rad;
        float cos = Mathf.Cos(angleRad);
        float sin = Mathf.Sin(angleRad);

        Vector2 rotateDirection = new Vector2(
            baseDirection.x * cos - baseDirection.y * sin,
            baseDirection.x * sin + baseDirection.y * cos
        ).normalized;

        dashDirection = rotateDirection;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isDashing) return;

        rb.linearVelocity = Vector2.zero;

        if (collision.gameObject.CompareTag("Player"))
        {
            if (Time.time - lastBounceTime > bounceCooldown)
            {
                PlayerHpSystem playerHp = collision.gameObject.GetComponent<PlayerHpSystem>();
                Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
                playerHp.TakeHit(1);
                playerRb.linearVelocity = Vector2.zero;
                lastBounceTime = Time.time;
                SetDashDirectionFromPlayer();
                if (isDashing) shake.StartShake(force: 0.25f);
            }
        }
        else if ((bounceLayers.value & (1 << collision.gameObject.layer)) != 0)
        {
            if (Time.time - lastBounceTime > bounceCooldown)
            {
                lastBounceTime = Time.time;
                SetDashDirectionToPlayer();
                if (isDashing) shake.StartShake(force: 0.25f);
            }
        }
    }

    private void StartLaserAttack()
    {
        laser = Instantiate(laserPrefab, laserSpawnPoint.position, Quaternion.identity);
        LaserRotate rotator = laser.GetComponent<LaserRotate>();

        if (rotator != null && !isCastingLaser)
        {
            isCastingLaser = true;
            rotator.StartRotating();
            animator.SetBool("LaserComplete", false);
            StartCoroutine(StopLaserAfterDuration(laser, laserDuration));
        }
    }

    private IEnumerator StopLaserAfterDuration(GameObject laser, float duration)
    {
        yield return new WaitForSeconds(duration);

        if (laser != null)
        {
            LaserRotate rotator = laser.GetComponent<LaserRotate>();
            if (rotator != null)
            {
                rotator.StopRotating();
            }
        }

        isCastingLaser = false;
        animator.SetBool("LaserComplete", true);
        attackCooldown = 0f;

        if (laser != null) Destroy(laser);
    }

    public void LaunchMissile()
    {
       // (AudioManager.Instance)?.PlaySFX("MechMissle");
        Instantiate(missilePrefab, missileSpawnPoint.position, Quaternion.identity);
        attackCooldown = 0f;
    }

    public void ActivateImmunity()
    {
        animator.SetBool("IsImmune", true);
        bossHp.isDamagable = false;
        attackCooldown = 0f;
        animator.ResetTrigger("Attack");
        animator.ResetTrigger("MissleAttack");
        StartCoroutine(DisableImmunityAfterDelay());
    }

    private IEnumerator DisableImmunityAfterDelay()
    {
        float duration = Random.Range(2f, 5f);
        bossHp.Heal(10, duration);
        yield return new WaitForSeconds(duration);

        bossHp.StopHealing();
        bossHp.isDamagable = true;
        animator.SetBool("IsImmune", false);
    }
}
