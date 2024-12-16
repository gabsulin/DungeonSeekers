using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    int currentScene;
    private void Start()
    {
        currentScene = SceneManager.GetActiveScene().buildIndex;
        Debug.Log(currentScene);
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene(currentScene + 1);
        }
    }
}
