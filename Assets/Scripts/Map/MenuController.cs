using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField] GameObject menu;
    [SerializeField] GameObject options;

    public void Play()
    {
        SceneManager.LoadScene(2);
        AudioManager.Instance.PlaySFX("UIButton");
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
