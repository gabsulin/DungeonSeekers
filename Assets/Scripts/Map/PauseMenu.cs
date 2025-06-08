using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject canvas;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject settings;
    [SerializeField] GameObject actionButtons;
    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;

    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void Settings()
    {
        actionButtons.SetActive(false);
        settings.SetActive(true);
    }

    public void GoBack()
    {
        actionButtons.SetActive(true);
        settings.SetActive(false);
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        //Save progress
        SceneManager.LoadScene("MainMenu");
    }
}
