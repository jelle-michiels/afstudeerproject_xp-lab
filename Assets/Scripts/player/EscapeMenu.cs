using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscapeMenu : MonoBehaviour
{
    /*public GameObject optionsMenu;
    public GameObject infoMenu;*/

    public static bool isPaused = false;

    public GameObject escapeMenu, playerTipsMenu;

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

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Joystick1Button7))
        {
            if (!playerTipsMenu.activeSelf)
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
            /*optionsMenu.SetActive(!optionsMenu.activeSelf);*/
        }
        
    }

    void OnEscape(bool shown)
    {
        escapeMenu.SetActive(shown ? false : true);
    }

    public void Resume()
    {
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

    public void LoadMenu()
    {
        Debug.Log("Loading menu...");
    }

    public void LoadHomeScreen()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }
}
