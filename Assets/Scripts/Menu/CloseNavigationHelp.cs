using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseNavigationHelp : MonoBehaviour
{
    public GameObject gameobject;

    public PlayerControl playerControl;

    public void whenButtonClicked()
    {
        if (gameobject.activeInHierarchy == true)
        {
            playerControl.enabled = true;
            gameobject.SetActive(false);
        }
 
        //wtf
    }
}
