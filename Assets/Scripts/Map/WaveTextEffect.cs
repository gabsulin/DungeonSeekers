using System.Collections;
using TMPro;
using UnityEngine;

public class WaveTextEffect : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI waveText;
    [SerializeField] float displayTime = 0.5f;
    [SerializeField] AnimationCurve fadeCurve;
    void Start()
    {
        if(waveText != null)
            waveText.gameObject.SetActive(false);
    }

    public void DisplayWaveText(int waveIndex)
    {
        if (waveText == null) return;

        waveText.text = "WAVE " + waveIndex;
        waveText.gameObject.SetActive(true);
        StartCoroutine(FadeTextIn());
    }

    private IEnumerator FadeTextIn()
    {
        float elapsedTime = 0f;
        Color baseColor = waveText.color;
        waveText.color = new Color(baseColor.r, baseColor.g, baseColor.b, 0f);
        waveText.gameObject.SetActive(true);

        while (elapsedTime < displayTime)
        {
            float alpha = fadeCurve.Evaluate(elapsedTime / displayTime);
            waveText.color = new Color(baseColor.r, baseColor.g, baseColor.b, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(1f);
        StartCoroutine(FadeTextOut());
    }


    private IEnumerator FadeTextOut()
    {
        float elapsedTime = 0f;
        Color initialColor = waveText.color;
        float startAlpha = initialColor.a;

        while (elapsedTime < displayTime)
        {
            float alpha = Mathf.Lerp(startAlpha, 0f, fadeCurve.Evaluate(elapsedTime / displayTime));
            waveText.color = new Color(initialColor.r, initialColor.g, initialColor.b, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        waveText.color = new Color(initialColor.r, initialColor.g, initialColor.b, 0f);
        waveText.gameObject.SetActive(false);
    }

}
