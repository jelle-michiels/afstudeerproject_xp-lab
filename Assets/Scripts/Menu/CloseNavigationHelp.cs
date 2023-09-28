using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CloseNavigationHelp : MonoBehaviour
{
    public GameObject playerTips;

    public PlayerControl movement;

    public CountdownTimer countDownTimer;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Joystick1Button0) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            DisablePlayerTips();
        }
    }

    public void DisablePlayerTips()
    {
        if (playerTips.activeInHierarchy == true)
        {
            movement.enabled = true;
            playerTips.SetActive(false);
            countDownTimer.startTimer();
        }
    }
}
