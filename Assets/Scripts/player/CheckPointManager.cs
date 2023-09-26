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


    private IEnumerator checkpointDelay()
    {
        yield return new WaitForSeconds(0.5f);
        LevelState.disableCheckpointscreen(checkpoint);
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "CheckPoint"){
            LevelState.enableCheckpointScreen(checkpoint);
            StartCoroutine(checkpointDelay());
            Debug.Log("test");
            
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
