using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class tutorialMenu : MonoBehaviour
{
    public GameObject tutorialScreen;

    void Start() {
        pause();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            quitToMenu();
        }
        if (Input.GetKeyDown(KeyCode.F)) {
            resume();
            tutorialScreen.SetActive(false);
        }
    }
    
    public void resume()
    {
        Time.timeScale = 1f;
    }

    public void pause()
    {
        Time.timeScale = 0f;
    }

    public void quitToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
        Debug.Log("Quit");
    }
}
