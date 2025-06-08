using UnityEditor;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Linq;

public class AssetEncryptor
{
    [MenuItem("Tools/Encrypt JSON")]
    public static void EncryptJson()
    {
        string path = "Assets/Data/mydata.json";
        byte[] data = Encoding.UTF8.GetBytes(File.ReadAllText(path));
        using Aes aes = Aes.Create();
        File.WriteAllBytes(path + ".enc", aes.Key.Concat(aes.IV).ToArray().Concat(data).ToArray());
        UnityEngine.Debug.Log("✅ JSON encrypted.");
    }
}
