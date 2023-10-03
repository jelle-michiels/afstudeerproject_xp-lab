using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class EscapeMenu : MonoBehaviour
{
    /*public GameObject optionsMenu;
    public GameObject infoMenu;*/

    public static bool isPaused = false;

    public GameObject escapeMenu, playerTipsMenu, optionsMenu;
    
    public GameObject escapeMenuFirst, optionsMenuFirst, optionMenuButton;

    public PlayerControl playerControl;

    void Start()
    {
        if (escapeMenu.activeSelf)
        {
            isPaused = true;
        } else
        {
            isPaused = false;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Joystick1Button7))
        {
            if (optionsMenu.activeSelf)
            {
                optionsMenu.SetActive(false);
                escapeMenu.SetActive(true);
                EventSystem.current.SetSelectedGameObject(optionMenuButton);

            }
            else if (!playerTipsMenu.activeSelf) // If options menu is not open, then handle the escape behavior
            {
                OnEscape(isPaused);
                if (isPaused)
                {
                    Resume();
                }
                else
                {
                    Pause();
                }
            }
        }
    }


    void OnEscape(bool shown)
    {
        escapeMenu.SetActive(shown ? false : true);
        EventSystem.current.SetSelectedGameObject(shown ? null : escapeMenuFirst);

    }

    public void Resume()
    {
        OnEscape(isPaused);
        Time.timeScale = 1f;
        isPaused = false;
        playerControl.enabled = true;
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        isPaused = true;
        playerControl.enabled = false;
    }

    public void LoadHomeScreen()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    public void OpenOptionMenu()
    {
        EventSystem.current.SetSelectedGameObject(optionsMenuFirst);
    }

    public void CloseOptionMenu()
    {
        EventSystem.current.SetSelectedGameObject(optionMenuButton);
    }
}
