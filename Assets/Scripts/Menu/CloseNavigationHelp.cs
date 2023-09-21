using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseNavigationHelp : MonoBehaviour
{
    public GameObject gameobject;


    public void whenButtonClicked()
    {
        if (gameobject.activeInHierarchy == true)
        {
            gameobject.SetActive(false);
        }

        GameObject.Find("LoadCanvas").GetComponent<LevelLoader>().LoadLevel();
 
        //wtf
    }
}
