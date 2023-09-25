using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CheckPointManager : MonoBehaviour
{
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "finish")
        {
            //GameObject.Find("ScoreCanvas").GetComponent<CountdownTimer>().gameFinished = true;
            levelState.FinishReached();
            print("collision"); 
            
        }
        if (other.gameObject.tag == "checkpoint"){
            
        }
    }

}
