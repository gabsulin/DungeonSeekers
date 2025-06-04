using UnityEngine;
using UnityEngine.SceneManagement;

public class AreaEntrance : MonoBehaviour
{
    [SerializeField] private string transitionName;
    PlayerObj player;

    private void Awake()
    {
        player = FindFirstObjectByType<PlayerObj>();
    }
    private void Start()
    {
        if(transitionName == SceneManagement.Instance.SceneTransitionName)
        {
            PlayerController.Instance.transform.position = this.transform.position;
            Debug.Log("scena");
            //player.SetMovePos(this.transform.position)
            UIFade.Instance.FadeFromBlack();
            if(!ProgressManager.IsGameFinished())
            {
                ProgressManager.SaveProgress(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
}
