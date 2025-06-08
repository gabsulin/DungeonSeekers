using System.Collections;
using UnityEngine.Networking;
using UnityEngine;

public class LicenseValidator : MonoBehaviour
{
    public string licenseKey = "Rls-E9Q-DDJ";
    public string serverURL = "http://localhost:5000/api/validate";

    IEnumerator Start()
    {
        string hwid = SystemInfo.deviceUniqueIdentifier;
        string url = $"{serverURL}?key={licenseKey}&hwid={hwid}";
        UnityWebRequest r = UnityWebRequest.Get(url);
        yield return r.SendWebRequest();

        if(r.result != UnityWebRequest.Result.Success || r.downloadHandler.text != "OK")
        {
            Debug.LogError("❌ Licence neplatná/nelze ověřit.");
            Application.Quit();
        }
        Debug.Log("❌ Licence neplatná/nelze ověřit.");
    }

}
