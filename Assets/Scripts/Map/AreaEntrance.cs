using Goldmetal.UndeadSurvivor;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AreaEntrance : MonoBehaviour
{
    UIFade uiFade;
    PlayerHpSystem playerHp;
    AbilityHolder abilityHolder;
    [SerializeField] private string transitionName;
    [SerializeField] private string nextSceneToPreload;
    private void Start()
    {
        uiFade = FindFirstObjectByType<UIFade>();
        if(transitionName == SceneManagement.Instance.SceneTransitionName)
        {
            PlayerController.Instance.transform.position = this.transform.position;
            uiFade.FadeFromBlack();
            if(!ProgressManager.IsGameFinished())
            {
                ProgressManager.SaveProgress(SceneManager.GetActiveScene().buildIndex);
            }
            if (!string.IsNullOrEmpty(nextSceneToPreload) && !SceneManager.GetSceneByName(nextSceneToPreload).isLoaded)
            {
                StartCoroutine(PreloadNextRoom(nextSceneToPreload));
            }
        }

        playerHp = FindFirstObjectByType<PlayerHpSystem>();
        abilityHolder = FindFirstObjectByType<AbilityHolder>();
        if (playerHp != null && abilityHolder != null)
        {
            playerHp.AssignUIElements();
            playerHp.UpdateUI();
            abilityHolder.AssignBar();
            abilityHolder.UpdateBar();
            Debug.Log("Assigned");
        }
        else
        {
            Debug.Log("Couldn't assign");
        }
    }

    private IEnumerator PreloadNextRoom(string sceneName)
    {
        AsyncOperation loadOp = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        loadOp.allowSceneActivation = true;
        while(!loadOp.isDone)
        {
            yield return null;
        }
    }
}
