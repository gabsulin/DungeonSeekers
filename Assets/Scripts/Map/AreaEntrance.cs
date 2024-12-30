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
            PlayerObj.Instance.transform.position = this.transform.position;
            player.SetMovePos(this.transform.position);
            UIFade.Instance.FadeFromBlack();
        }
    }
}
