using UnityEngine;
using System.IO;
using System.Security.Cryptography;

public class AssetLoader : MonoBehaviour
{
    void Start()
    {
        byte[] all = File.ReadAllBytes(Application.dataPath + "/Data/mydata.json.enc");
        byte[] key = all[..32], iv = all[32..48], data = all[48..];
        using Aes aes = Aes.Create(); aes.Key = key; aes.IV = iv;
        using var ms = new MemoryStream(data);
        using var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read);
        using var sr = new StreamReader(cs);
        Debug.Log("📄 JSON data: " + sr.ReadToEnd());
    }
}
