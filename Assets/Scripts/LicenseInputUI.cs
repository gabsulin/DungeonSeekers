using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LicenseInputUI : MonoBehaviour
{
    public TMP_InputField licenseInputField;
    public Button submitButton;
    public TextMeshProUGUI messageText;
    public LicenseValidator licenseValidator;

    private void Start()
    {
        submitButton.onClick.AddListener(OnSubmit);
        messageText.text = "";

        /*string savedKey = PlayerPrefs.GetString("LicenseKey", "");
        if (!string.IsNullOrEmpty(savedKey))
        {
            licenseInputField.text = savedKey;
            StartCoroutine(ValidateKey(savedKey));
        }*/
    }

    public void OnSubmit()
    {
        string key = licenseInputField.text.Trim();
        if(string.IsNullOrEmpty(key))
        {
            messageText.text = "Please, enter a license key.";
            return;
        }
        PlayerPrefs.SetString("LicenseKey", key);
        PlayerPrefs.Save();

        messageText.text = "Ověřuji klíč...";
        StartCoroutine(ValidateKey(key));
    }

    IEnumerator ValidateKey(string key)
    {
        yield return licenseValidator.ValidateKey(key);

        if (licenseValidator.IsValid)
        {
            messageText.text = "✅ Licence ověřena!";
            SceneManager.LoadScene(1);
        }
        else
        {
            messageText.text = "❌ Licence neplatná.";
        }
    }
}
