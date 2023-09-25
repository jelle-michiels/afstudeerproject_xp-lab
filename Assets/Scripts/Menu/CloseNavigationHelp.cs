using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CloseNavigationHelp : MonoBehaviour
{
    public GameObject gameobject;

    public PlayerControl playerControl;

    public CountdownTimer countDownTimer;

    public void whenButtonClicked()
    {
        if (gameobject.activeInHierarchy == true)
        {
            playerControl.enabled = true;
            gameobject.SetActive(false);
            countDownTimer.startTimer();
        }
 
        //wtf
    }
}
