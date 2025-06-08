using System;
using TMPro;
using UnityEngine;

public class Stats : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI enemiesKilled;
    [SerializeField] TextMeshProUGUI roomsCleared;
    [SerializeField] TextMeshProUGUI coinsCollected;
    [SerializeField] TextMeshProUGUI timeSpentPlaying;
    [SerializeField] TextMeshProUGUI fastestRoomCleared;

    void Start()
    {
        enemiesKilled.text = "Enemies Killed: " + GameStats.Instance.enemiesKilled.ToString();
        roomsCleared.text = "Rooms Cleared: " + GameStats.Instance.roomsCleared.ToString();
        coinsCollected.text = "Coins Collected: " + GameStats.Instance.coinsPickedUp.ToString();
        TimeSpan timePlayedSpan = TimeSpan.FromSeconds(GameStats.Instance.timePlayed);
        timeSpentPlaying.text = "Time Played: " + timePlayedSpan.ToString(@"mm\:ss");

        if (GameStats.Instance.fastestRoomClearTime == Mathf.Infinity)
        {
            fastestRoomCleared.text = "Fastest Room Clear: N/A";
        }
        else
        {
            TimeSpan fastestClearSpan = TimeSpan.FromSeconds(GameStats.Instance.fastestRoomClearTime);
            fastestRoomCleared.text = "Fastest Room Clear: " + fastestClearSpan.ToString(@"mm\:ss");
        }
    }

}
