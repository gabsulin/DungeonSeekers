using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField] GameObject menu;
    [SerializeField] GameObject options;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Play()
    {
        SceneManager.LoadScene(2);
    }

    public void OpenSettings()
    {
        menu.SetActive(false);
        options.SetActive(true);
    }

    public void Back()
    {
        options.SetActive(false);
        menu.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
}
