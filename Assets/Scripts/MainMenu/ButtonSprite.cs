using UnityEngine;
using UnityEngine.UI;

public class ButtonSprite : MonoBehaviour
{
    public Sprite defaultSprite;
    public Sprite toggledSprite;
    private Image buttonImage;
    private bool isToggled = false;

    private void Start()
    {
        buttonImage = GetComponent<Image>();
        GetComponent<Button>().onClick.AddListener(ToggleSprite);
    }

    private void ToggleSprite()
    {
        isToggled = !isToggled;
        buttonImage.sprite = isToggled ? toggledSprite : defaultSprite;
    }
}

