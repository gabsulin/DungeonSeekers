using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterSelect : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private GameObject canvas;
    [SerializeField] private TMP_Text characterNameText, healthText, shieldsText, speedText, description;
    [SerializeField] private Button actionButton;
    [SerializeField] private TMP_Text actionButtonText;
    [SerializeField] private Animator descAnim, statsAnim, buttonsAnim;
    [SerializeField] private Image coin;

    private CharacterSelectObj selectedCharacter;
    private float zoom;
    private float velocity = 0;
    private float smoothTime = 0.35f;
    private float minZoom = 1.5f;
    private float maxZoom = 6;
    private bool isZooming = false;
    //private bool isZoomed = false;
    private Vector3 targetPosition;
    private Vector3 originalPosition = new Vector3(0, 0, -10);
    private Vector3 velocityPos = Vector3.zero;
    private RaycastHit2D hit;

    private void Start()
    {
        zoom = _camera.orthographicSize;
        originalPosition = _camera.transform.position;

        //UIFade.Instance.FadeFromBlack();
    }

    private void Update()
    {
        CheckForSelection();
    }

    private void CheckForSelection()
    {
        if (Input.GetMouseButtonDown(0) && !isZooming /*&& !isZoomed*/)
        {
            hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null)
            {
                CharacterSelectObj character = hit.collider.GetComponent<CharacterSelectObj>();
                if (character != null)
                {
                    SelectCharacter(character);
                }
            }
        }


        if (isZooming)
        {
            _camera.orthographicSize = Mathf.SmoothDamp(_camera.orthographicSize, zoom, ref velocity, smoothTime);
            _camera.transform.position = Vector3.SmoothDamp(_camera.transform.position, targetPosition, ref velocityPos, smoothTime);
            canvas.gameObject.SetActive(true);

            if (Mathf.Abs(_camera.orthographicSize - zoom) < 0.01f && Vector3.Distance( _camera.transform.position, targetPosition) < 0.01f)
            {
                _camera.orthographicSize = zoom;
                _camera.transform.position = targetPosition;
                isZooming = false;
                //isZoomed = true;
            }
        }
    }

    private void SelectCharacter(CharacterSelectObj character)
    {
        selectedCharacter = character;

        CharacterData data = selectedCharacter.characterData;
        characterNameText.text = data.characterName;
        description.text = data.description;
        healthText.text = data.health.ToString();
        shieldsText.text = data.shields.ToString();
        speedText.text = data.speed.ToString();

        actionButtonText.text = data.isUnlocked ? "Select" : "      " + data.price;
        coin.gameObject.SetActive(!data.isUnlocked);

        actionButton.onClick.RemoveAllListeners();
        actionButton.onClick.AddListener(() => HandleCharacterSelection(data));

        ZoomToCharacter(character.transform.position);
    }

    private void ZoomToCharacter(Vector3 characterPosition)
    {
        isZooming = true;
        zoom = minZoom;
        targetPosition = characterPosition;
        targetPosition.y += 0.25f;
        targetPosition.z = _camera.transform.position.z;
        canvas.gameObject.SetActive(false);
    }

    public void ExitZoom()
    {
        isZooming = true;
        //isZoomed = false;
        zoom = maxZoom;
        targetPosition = originalPosition;
        descAnim.SetBool("IsGoingBack", true);
        statsAnim.SetBool("IsGoingBack", true);
        buttonsAnim.SetBool("IsGoingBack", true);
    }

    public void HandleCharacterSelection(CharacterData data)
    {
        if (data.isUnlocked)
        {
            PlayCharacter(data);
        }
        else
        {
            BuyCharacter(data);
        }
    }

    private void PlayCharacter(CharacterData data)
    {
        Debug.Log("Selected: " + data.characterName);

        var player = selectedCharacter.AddComponent<PlayerController>();

        player.moveSpeed = data.speed;

        PlayerHpSystem hpSystem = selectedCharacter.GetComponent<PlayerHpSystem>();
        if (hpSystem == null) hpSystem = selectedCharacter.AddComponent<PlayerHpSystem>();
        hpSystem.enabled = true;
        hpSystem.characterData = data;

        CapsuleCollider2D collider = selectedCharacter.GetComponent<CapsuleCollider2D>();
        collider.direction = CapsuleDirection2D.Horizontal;
        collider.offset = new Vector2(0, 0.1f);
        collider.size = new Vector2(0.5f, 0.4f);

        AbilityHolder abilityHolder = selectedCharacter.GetComponent<AbilityHolder>();
        if (abilityHolder != null) abilityHolder.enabled = true;

        Transform cameras = selectedCharacter.transform.Find("Cameras");
        cameras.gameObject.SetActive(true);

        selectedCharacter.tag = "Player";

        //SceneManager.sceneLoaded += OnSceneLoaded;

        SceneManager.LoadScene("1_1");
        GameStats.Instance.ResetStats();
    }

    /*private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayerHpSystem hpSystem = FindFirstObjectByType<PlayerHpSystem>();

        if (hpSystem != null)
        {
            hpSystem.AssignUIElements();
            hpSystem.UpdateUI();
        }
        else
        {
            Debug.LogError("PlayerHpSystem not found in the scene!");
        }

        //SceneManager.sceneLoaded -= OnSceneLoaded;
    }*/

    private void BuyCharacter(CharacterData data)
    {
        if (CoinManager.instance.CanAfford(data.price))
        {
            CoinManager.instance.Buy(data.price);
            data.isUnlocked = true;
            actionButtonText.text = "Select";
            coin.gameObject.SetActive(false);
            Debug.Log("Bought: " + data.characterName);
            AudioManager.Instance.PlaySFX("BuySell");
        }
        else
        {
            Debug.Log("Not enough coins!");
        }
    }
}

