using Unity.VisualScripting;
using UnityEngine;

public static class ProgressManager
{
    private const string ProgressKey = "PlayerProgress";
    private const string GameFinishedKey = "GameFinished";

    public static void SaveProgress(int levelIndex)
    {
        PlayerPrefs.SetInt(ProgressKey, levelIndex);
        PlayerPrefs.Save();
    }
    public static int LoadProgress()
    {
        return PlayerPrefs.GetInt(ProgressKey, 0);
    }

    public static void ResetProgress()
    {
        PlayerPrefs.DeleteKey(ProgressKey);
        PlayerPrefs.Save();
    }

    public static void MarkGameFinished()
    {
        PlayerPrefs.SetInt(GameFinishedKey, 1);
        PlayerPrefs.Save();
    }

    public static bool IsGameFinished()
    {
        return PlayerPrefs.GetInt(GameFinishedKey, 0) == 1;
    }
}
