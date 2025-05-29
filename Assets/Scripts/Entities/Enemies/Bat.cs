using UnityEngine;

public class Bat : MonoBehaviour
{
    [Header("Components")]
    Animator animator;

    [Header("Fly Settings")]
    [SerializeField] float speed = 7f;
    [SerializeField] float flightDistanceMin = 8f;
    [SerializeField] float flightDistanceMax = 15f;
    [SerializeField] float randomAngleSpread = 120f;

    [Header("Collisions")]
    [SerializeField] LayerMask collisionLayerMask;
    [SerializeField] int maxRepositionAttempts = 15;

    private Transform playerTransform;
    private Vector2 targetPosition;
    private bool isFlying = false;
    private VampireEnemy originalVampire;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            playerTransform = playerObject.transform;
            ChooseDestinationAwayFromPlayer();
        }
        else
        {
            Debug.LogError("Hr�� nebyl nalezen! Netop�r polet� n�hodn�m sm�rem s kontrolou koliz� cesty.");
            ChooseRandomDestinationFallback();
        }
    }

    public void SetOriginalVampire(VampireEnemy vampire)
    {
        originalVampire = vampire;
    }

    void ChooseDestinationAwayFromPlayer()
    {
        Vector2 baseDirectionFromPlayer = Vector2.zero;
        if (playerTransform != null)
        {
            baseDirectionFromPlayer = ((Vector2)transform.position - (Vector2)playerTransform.position).normalized;
        }

        if (baseDirectionFromPlayer == Vector2.zero)
        {
            baseDirectionFromPlayer = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
            if (baseDirectionFromPlayer == Vector2.zero) baseDirectionFromPlayer = Vector2.right;
        }

        for (int i = 0; i < maxRepositionAttempts; i++)
        {
            float angleOffset = Random.Range(-randomAngleSpread / 2, randomAngleSpread / 2);
            Vector2 finalDirection = Quaternion.Euler(0, 0, angleOffset) * baseDirectionFromPlayer;
            finalDirection.Normalize();

            float distance = Random.Range(flightDistanceMin, flightDistanceMax);
            Vector2 candidatePosition = (Vector2)transform.position + finalDirection * distance;

            RaycastHit2D hit = Physics2D.Linecast(transform.position, candidatePosition, collisionLayerMask);

            if (hit.collider == null)
            {
                targetPosition = candidatePosition;
                isFlying = true;
                Debug.DrawLine(transform.position, targetPosition, Color.green, 3f);
                return;
            }
        }

        Debug.LogWarning($"Netop�r nena�el volnou CESTU k c�li po {maxRepositionAttempts} pokusech. Prov�d� nouzov� kr�tk� let.");
        targetPosition = (Vector2)transform.position + baseDirectionFromPlayer * (flightDistanceMin / 2);
        isFlying = true;
        Debug.DrawLine(transform.position, targetPosition, Color.red, 3f);
    }

    void ChooseRandomDestinationFallback()
    {
        for (int i = 0; i < maxRepositionAttempts; i++)
        {
            Vector2 randomDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
            if (randomDirection == Vector2.zero) randomDirection = Vector2.right;

            float distance = Random.Range(flightDistanceMin, flightDistanceMax);
            Vector2 candidatePosition = (Vector2)transform.position + randomDirection * distance;

            RaycastHit2D hit = Physics2D.Linecast(transform.position, candidatePosition, collisionLayerMask);
            if (hit.collider == null)
            {
                targetPosition = candidatePosition;
                isFlying = true;
                Debug.LogWarning("Netop�r (hr�� nenalezen) vol� n�hradn� c�l, voln� cesta nalezena.");
                Debug.DrawLine(transform.position, targetPosition, Color.cyan, 3f);
                return;
            }
        }

        Debug.LogWarning($"Netop�r (hr�� nenalezen) nena�el volnou CESTU po {maxRepositionAttempts} pokusech. Prov�d� nouzov� kr�tk� let n�hodn�m sm�rem.");
        Vector2 emergencyRandomDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        if (emergencyRandomDirection == Vector2.zero) emergencyRandomDirection = Vector2.right;
        targetPosition = (Vector2)transform.position + emergencyRandomDirection * (flightDistanceMin / 2);
        isFlying = true;
        Debug.DrawLine(transform.position, targetPosition, Color.magenta, 3f);
    }

    void Update()
    {
        if (!isFlying) return;

        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if (spriteRenderer != null)
        {
            float directionToTargetX = targetPosition.x - transform.position.x;
            if (directionToTargetX > 0.05f)
                spriteRenderer.flipX = false;
            else if (directionToTargetX < -0.05f)
                spriteRenderer.flipX = true;
        }
        if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
        {
            isFlying = false;
            animator.SetTrigger("Revive");
        }
    }

    public void ReviveVampire()
    {
        if (originalVampire != null)
        {
            originalVampire.Reactivate(transform.position);
        }
        else
        {
            Debug.LogError("Chyba: Netop�r nem� referenci na p�vodn�ho up�ra!");
        }
        Destroy(gameObject);
    }
}