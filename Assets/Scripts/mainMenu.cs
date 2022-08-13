using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour
{
    public void start()
    {
        SceneManager.LoadScene(1);
    }

    public void select(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    public void quitGame()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
}
