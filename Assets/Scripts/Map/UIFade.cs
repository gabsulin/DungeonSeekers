using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFade : MonoBehaviour
{
    [SerializeField] private Image fadeScreen;
    [SerializeField] private float fadeSpeed = 1f;

    private IEnumerator fadeRoutine; 

    public void FadeToBlack()
    {
        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);

        StartCoroutine(FadeRoutine(1));
    }

    public void FadeFromBlack()
    {
        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);

        fadeRoutine = FadeRoutine(0);
        StartCoroutine(fadeRoutine);
    }

    private IEnumerator FadeRoutine(float targetAlpha)
    {
        while (!Mathf.Approximately(fadeScreen.color.a, targetAlpha))
        {
            float alpha = Mathf.MoveTowards(fadeScreen.color.a, targetAlpha, fadeSpeed * Time.deltaTime);
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, alpha);
            yield return null;
        }
    }
}