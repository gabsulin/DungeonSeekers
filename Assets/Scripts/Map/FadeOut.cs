using UnityEngine;

public class FadeOut : MonoBehaviour
{
    UIFade uiFade;
    void Start()
    {
        uiFade = GetComponent<UIFade>();
        uiFade.FadeFromBlack();
    }
}
