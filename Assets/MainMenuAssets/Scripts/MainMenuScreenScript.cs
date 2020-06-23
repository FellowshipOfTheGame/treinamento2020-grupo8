using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScreenScript : MonoBehaviour
{
    private Animator transition;


    void Start()
    {
        transition = GetComponent<Animator>();
    }

    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }


    IEnumerator LoadLevel(int levelIndex)
    {
        //Play animation
        transition.SetTrigger("startTransition");
        //Wait
        yield return new WaitForSeconds(1.5f);
        //Load scene
        SceneManager.LoadScene(levelIndex);
    }


    public void QuitGame()
    {
        Debug.Log("Exit");
        Application.Quit();
    }
}
