using System.Collections;
using UnityEngine;
using TMPro;

public class EnemyDamage : MonoBehaviour
{
    private PlayerObj player;
    [SerializeField] private ParticleSystem boomParticles;
    [SerializeField] public int damage;

    [Header("Damage Text Settings")]
    [SerializeField] private GameObject damageTextPrefab;
    [SerializeField] private float textSpawnRadius = 0.5f;
    [SerializeField] private Transform uiCanvasTransform;
    [SerializeField] private Camera mainCamera;

    private bool hasHitEnemy = false;

    void Start()
    {
        player = FindFirstObjectByType<PlayerObj>();

        if (mainCamera == null)
        {
            mainCamera = Camera.main;
            if (mainCamera == null) Debug.LogError("Main Camera not found!");
        }

        if (uiCanvasTransform == null)
        {
            Canvas canvas = FindFirstObjectByType<Canvas>();
            if (canvas != null) uiCanvasTransform = canvas.transform;
            if (uiCanvasTransform == null) Debug.LogError("UI Canvas Transform not assigned!");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HandleCollision(collision.collider, collision.contacts[0].point);
        PlayerController.Instance.isAttacking = false;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        HandleCollision(collider, collider.transform.position);
    }

    private void HandleCollision(Collider2D collider, Vector3 hitPosition)
    {
        if (hasHitEnemy) return;

        Weapon weapon = PlayerController.Instance?.GetCurrentWeapon();
        bool isMelee = weapon is Melee;
        var state = player._playerState;

        if ((collider.CompareTag("Enemy") && state == PlayerObj.PlayerState.attack) ||
            (collider.CompareTag("MiniBoss") && state == PlayerObj.PlayerState.attack))
        {
            int actualDamageDealt = damage;
            Vector3 enemyCenterPosition = collider.bounds.center;

            if (collider.CompareTag("MiniBoss"))
            {
                var bossHp = collider.GetComponent<BossHpSystem>();
                if (bossHp != null)
                {
                    hasHitEnemy = true;
                    bossHp.TakeDamage(actualDamageDealt, isMelee);
                    ShowDamageNumber(actualDamageDealt, enemyCenterPosition);
                    StartCoroutine(ResetHitFlag());
                }
            }
            else
            {
                if (TryDealDamageToEnemy(collider, actualDamageDealt))
                {
                    hasHitEnemy = true;
                    ShowDamageNumber(actualDamageDealt, enemyCenterPosition);
                    if (gameObject.activeSelf)
                        StartCoroutine(ResetHitFlag());
                }
            }
        }
    }

    private bool TryDealDamageToEnemy(Component target, int damageToDeal)
    {
        var spumEnemyHp = target.GetComponent<EnemyHpSystem>();
        if (spumEnemyHp != null)
        {
            spumEnemyHp.TakeDamage(damageToDeal);
            return true;
        }

        var enemyHp = target.GetComponent<EnemyHealth>();
        if (enemyHp != null)
        {
            enemyHp.TakeDamage(damageToDeal);
            return true;
        }

        return false;
    }

    private void ShowDamageNumber(int damageAmount, Vector3 enemyWorldPosition)
    {
        float randomX = Random.Range(-textSpawnRadius, textSpawnRadius);
        float randomY = Random.Range(-textSpawnRadius, textSpawnRadius);
        Vector3 textWorldPosition = enemyWorldPosition + new Vector3(randomX, randomY, 0f);
        Vector2 screenPosition = mainCamera.WorldToScreenPoint(textWorldPosition);

        GameObject damageTextInstance = Instantiate(damageTextPrefab, uiCanvasTransform);
        RectTransform canvasRect = uiCanvasTransform.GetComponent<RectTransform>();
        RectTransform damageRect = damageTextInstance.GetComponent<RectTransform>();

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPosition, null, out Vector2 localPoint))
        {
            damageRect.anchoredPosition = localPoint;
        }

        DamageText animator = damageTextInstance.GetComponent<DamageText>();
        if (animator != null)
        {
            animator.SetText(damageAmount.ToString());
        }
        else
        {
            TextMeshProUGUI tmpText = damageTextInstance.GetComponentInChildren<TextMeshProUGUI>();
            if (tmpText != null)
            {
                tmpText.text = damageAmount.ToString();
            }
        }
    }

    private IEnumerator ResetHitFlag()
    {
        yield return new WaitForSeconds(0.35f);
        hasHitEnemy = false;
    }

    public void ResetHit()
    {
        hasHitEnemy = false;
    }
}
