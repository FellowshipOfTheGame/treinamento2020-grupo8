using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameOverMenuScript : MonoBehaviour
{
    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }


    public void QuitGame()
    {
        Debug.Log("Exit");
        Application.Quit();
    }
}
