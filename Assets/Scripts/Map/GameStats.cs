using UnityEngine;

public class GameStats : MonoBehaviour
{
    public static GameStats Instance;

    public int enemiesKilled = 0;
    public int roomsCleared = 0;
    public int coinsPickedUp = 0;
    public float timePlayed = 0;
    public float fastestRoomClearTime = Mathf.Infinity;

    private float roomStartTime;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        timePlayed += Time.deltaTime;
    }

    public void OnRoomStart()
    {
        roomStartTime = Time.time;
    }

    public void OnRoomCleared()
    {
        float clearTime = Time.time - roomStartTime;
        if(clearTime < fastestRoomClearTime)
            fastestRoomClearTime = clearTime;

        roomsCleared++;
    }

    public void AddEnemyKill()
    {
        enemiesKilled += 1;
    }

    public void AddCoins(int amount)
    {
        coinsPickedUp += amount;
    }

    public void ResetStats()
    {
        enemiesKilled = 0;
        roomsCleared = 0;
        coinsPickedUp = 0;
        timePlayed = 0f;
        fastestRoomClearTime = Mathf.Infinity;
    }
}
