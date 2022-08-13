using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class menuCheatScript : MonoBehaviour
{   
    void Update()
    {
        if(Input.GetKey(KeyCode.RightBracket)) {
            if (SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings-1) SceneManager.LoadScene(0);
            else SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
        }
        if(Input.GetKey(KeyCode.LeftBracket)) {
            if (SceneManager.GetActiveScene().buildIndex == 0) SceneManager.LoadScene(SceneManager.sceneCountInBuildSettings-1);
            else SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex-1);
        }
    }
}
