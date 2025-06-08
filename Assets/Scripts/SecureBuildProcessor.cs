using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using System.Diagnostics;
using System.IO;
using UnityEngine;

public class SecureBuildProcessor : IPreprocessBuildWithReport
{
    public int callbackOrder => 0;
    public void OnPreprocessBuild(BuildReport report)
    {
        string hash = System.DateTime.UtcNow.Ticks.ToString();
        File.WriteAllText("Assets/Resources/build_hash.txt", hash);
        UnityEngine.Debug.Log($"🔐 Build hash: {hash}");
        Process.Start(new ProcessStartInfo("Tools/obfuscate.bat")
        {
            UseShellExecute = false,
            CreateNoWindow = true
        });
    }
}
