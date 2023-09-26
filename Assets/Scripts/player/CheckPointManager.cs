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
        if (other.gameObject.tag == "checkpoint"){
            LevelState.checkpointReached(checkpoint);
        }

        if (other.gameObject.tag == "damageObject"){
            LevelState.died(loseScreen);
        }

        if (other.gameObject.tag == "finish")
        {
            LevelState.endPointReached(winScreen);            
        }
    }

}
