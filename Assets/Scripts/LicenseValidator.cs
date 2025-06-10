using System.Collections;
using UnityEngine.Networking;
using UnityEngine;

public class LicenseValidator : MonoBehaviour
{
    public string serverURL = "https://gabsulin.pythonanywhere.com/api/validate";
    public bool IsValid { get; private set; }

    public IEnumerator ValidateKey(string licenseKey)
    {
        string hwid = SystemInfo.deviceUniqueIdentifier;
        string url = $"{serverURL}?key={licenseKey}&hwid={hwid}";
        UnityWebRequest r = UnityWebRequest.Get(url);
        yield return r.SendWebRequest();

        if (r.result == UnityWebRequest.Result.Success && r.downloadHandler.text == "OK")
        {
            IsValid = true;
        }
        else
        {
            IsValid = false;
        }
    }
}

