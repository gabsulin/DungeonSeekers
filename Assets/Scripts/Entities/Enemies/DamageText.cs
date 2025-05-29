using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    public float popUpDuration = 0.3f;
    public float totalLifetime = 1.2f;
    public float riseDistance = 60f;
    public float fallDistance = 30f;
    public float horizontalDrift = 40f;
    public float fadeOutTime = 1f;

    private TextMeshProUGUI damageText;
    private Color startColor;
    private float elapsedTime = 0f;
    private RectTransform rectTransform;

    private Vector2 screenStartPosition;
    private float direction;

    private void Awake()
    {
        damageText = GetComponent<TextMeshProUGUI>();
        rectTransform = GetComponent<RectTransform>();

        if (damageText != null)
            startColor = damageText.color;

        // Random drift left or right
        direction = Random.value > 0.5f ? 1f : -1f;

        Destroy(gameObject, totalLifetime);
    }

    public void SetText(string text)
    {
        if (damageText != null)
            damageText.text = text;
    }

    // ✅ Called from EnemyDamage to initialize position correctly
    public void SetPosition(Vector2 screenPosition)
    {
        screenStartPosition = screenPosition;
        if (rectTransform != null)
            rectTransform.position = screenStartPosition;
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;

        float t = elapsedTime / totalLifetime;
        Vector2 newPosition = screenStartPosition;

        if (elapsedTime < popUpDuration)
        {
            float popT = elapsedTime / popUpDuration;
            newPosition.y += Mathf.Lerp(0, riseDistance, EaseOutCubic(popT));
        }
        else
        {
            float fallT = (elapsedTime - popUpDuration) / (totalLifetime - popUpDuration);
            newPosition.y += riseDistance - Mathf.Lerp(0, fallDistance, EaseInCubic(fallT));
            newPosition.x += direction * Mathf.Lerp(0, horizontalDrift, EaseOutCubic(fallT));
        }

        if (rectTransform != null)
            rectTransform.position = newPosition;

        if (damageText != null)
        {
            float alpha = Mathf.Lerp(startColor.a, 0f, elapsedTime / fadeOutTime);
            Color faded = startColor;
            faded.a = alpha;
            damageText.color = faded;
        }
    }

    private float EaseOutCubic(float t) => 1f - Mathf.Pow(1f - t, 3);
    private float EaseInCubic(float t) => t * t * t;
}
