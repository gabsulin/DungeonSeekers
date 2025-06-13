using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField] GameObject menu;
    [SerializeField] GameObject options;
    [SerializeField] MMEnemyObj player;
    [SerializeField] Transform goalPos, fadePos;
    [SerializeField] Animator pedestal;
    [SerializeField] Animator gate;
    UIFade uiFade;

    float waitToLoadTime = 1.5f;
    private void Start()
    {
        uiFade = FindFirstObjectByType<UIFade>();
    }
    private void Update()
    {
        if (Vector2.Distance(player.transform.position, fadePos.position) < 0.1f)
        {
            uiFade.FadeToBlack();
            StartCoroutine(LoadSceneRoutine());
        }
    }
    public void Play()
    {
        SceneManager.LoadScene(2);
        AudioManager.Instance.PlaySFX("UIButton");
    }
    private IEnumerator LoadSceneRoutine()
    {
        yield return new WaitForSeconds(waitToLoadTime);
        var uiFade = FindFirstObjectByType<UIFade>();
        Destroy(uiFade);
        SceneManager.LoadScene(2);
    }

    public void StartGame()
    {
        pedestal.SetBool("Slide", true);
        gate.SetBool("IsOpen", true);
        player.SetMovePos(goalPos.position);
    }
    public void ContinueGame()
    {
        if (ProgressManager.IsGameFinished())
        {
            Debug.Log("Game is finished. No progress to load.");
            return;
        }

        int savedLevel = ProgressManager.LoadProgress();

        if (savedLevel < SceneManager.sceneCountInBuildSettings)
        {
            AudioManager.Instance.PlaySFX("UIButton");
            SceneManager.LoadScene(savedLevel);
        }
        else
        {
            Debug.LogWarning("Saved level index is out of range. Loading default.");
            SceneManager.LoadScene(1);
        }
    }
    public void OpenSettings()
    {
        menu.SetActive(false);
        options.SetActive(true);
        AudioManager.Instance.PlaySFX("UIButton");
    }

    public void Back()
    {
        options.SetActive(false);
        menu.SetActive(true);
        AudioManager.Instance.PlaySFX("UIButton");
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Quit");
        AudioManager.Instance.PlaySFX("UIButton");
    }
}
