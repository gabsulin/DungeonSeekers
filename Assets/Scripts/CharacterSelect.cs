
using UnityEngine;

using UnityEngine;

public class CharacterSelect : MonoBehaviour
{
    CharacterInfo character;

    [SerializeField] Camera camera;
    [SerializeField] Canvas canvas;

    float zoom;
    float velocity = 0;
    float smoothTime = 0.35f;
    float minZoom = 1;
    float maxZoom = 6;
    bool isZooming = false;

    Vector3 targetPosition;
    Vector3 originalPosition;
    Vector3 velocityPos = Vector3.zero;

    RaycastHit2D hit;

    public bool isCharacterUnlocked = false;
    void Start()
    {
        zoom = camera.orthographicSize;
        originalPosition = camera.transform.position;
    }

    void Update()
    {
        CheckForSelection();
    }

    private void CheckForSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null && hit.collider.CompareTag("Character"))
            {
                ZoomToCharacter(hit.collider.transform.position);
            }            
        }

        if (isZooming)
        {
            camera.orthographicSize = Mathf.SmoothDamp(camera.orthographicSize, zoom, ref velocity, smoothTime);
            camera.transform.position = Vector3.SmoothDamp(camera.transform.position, targetPosition, ref velocityPos, smoothTime);

            if (Mathf.Abs(camera.orthographicSize - zoom) < 0.01f && Vector3.Distance(camera.transform.position, targetPosition) < 0.01f)
            {
                camera.orthographicSize = zoom;
                camera.transform.position = targetPosition;
                isZooming = false;
                canvas.gameObject.SetActive(zoom == minZoom);
            }
        }
    }

    public void ZoomToCharacter(Vector3 characterPosition)
    {
        originalPosition = camera.transform.position;
        isZooming = true;
        zoom = minZoom;
        targetPosition = characterPosition;
        targetPosition.z = camera.transform.position.z;
        canvas.gameObject.SetActive(false);
    }

    public void ExitZoom()
    {
        isZooming = true;
        zoom = maxZoom;
        targetPosition = originalPosition;
        canvas.gameObject.SetActive(false);
    }

    public void SelectCharacter()
    {
        int price = 20;
        if (isCharacterUnlocked)
        {
            SelectCharacter();
            Debug.Log("Selected");
        }
        else
        {
            BuyCharacter(price);
            Debug.Log("Bought");
        }
    }

    public void BuyCharacter(int price)
    {
        CoinManager.instance.Buy(price);
    }
}

