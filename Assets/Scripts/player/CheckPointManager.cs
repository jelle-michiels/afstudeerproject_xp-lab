using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CheckPointManager : MonoBehaviour
{

    public GameObject winScreen;
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "finish")
        {
            LevelState.endPointReached(winScreen);
            print("collision"); 
            
        }
        if (other.gameObject.tag == "checkpoint"){
            
        }
    }

}
