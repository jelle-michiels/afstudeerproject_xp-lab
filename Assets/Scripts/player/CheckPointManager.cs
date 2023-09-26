using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class CheckPointManager : MonoBehaviour

{
    
    public GameObject winScreen;

    public GameObject loseScreen;

    public GameObject checkpoint;

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "CheckPoint"){
            LevelState.enableCheckpointScreen(checkpoint);
        }

        if (other.gameObject.tag == "Damage"){
            Debug.Log("died");
            LevelState.died(loseScreen);
        }

        if (other.gameObject.tag == "finish"){
            Debug.Log("finish");
            LevelState.endPointReached(winScreen);            
        }
    }

}
