using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuAnimationScript : MonoBehaviour
{
    [SerializeField] private Animator mainMenuAnim;
    [SerializeField] private Animator optionsMenuAnim;

    private void Start()
    {
        mainMenuAnim.SetBool("isActive", true);
        mainMenuAnim.SetBool("isOut", false);

        optionsMenuAnim.SetBool("isActive", false);
        optionsMenuAnim.SetBool("isOut", true);
    }


    public void OpenOptionsMenu()
    {
        mainMenuAnim.SetBool("isActive", false);
        mainMenuAnim.SetBool("isOut", true);

        optionsMenuAnim.SetBool("isActive", true);
        optionsMenuAnim.SetBool("isOut", false);
    }


    public void CloseOptionsMenu()
    {
        mainMenuAnim.SetBool("isActive", true);
        mainMenuAnim.SetBool("isOut", false);

        optionsMenuAnim.SetBool("isActive", false);
        optionsMenuAnim.SetBool("isOut", true);
    }
}
