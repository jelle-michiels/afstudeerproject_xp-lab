using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour
{
    public GameObject settingsMenuFirst, settingsMenuButton;

    public void PlayGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void PlayWorldBuilder()
    {
        SceneManager.LoadScene("Editor");
    }



    public void PlayTextureUpload()
    {
        SceneManager.LoadScene("TextureUpload");
        Debug.Log("TextureUpload");
    }

    public void QuitGame()
    {
        Debug.Log("quit");
        Application.Quit();
    }

    public void OpenSettingsMenu()
    {
        EventSystem.current.SetSelectedGameObject(settingsMenuFirst);
    }

    public void CloseSettingsMenu()
    {
        EventSystem.current.SetSelectedGameObject(settingsMenuButton);
    }

}
