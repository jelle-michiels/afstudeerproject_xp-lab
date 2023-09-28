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

    public GameObject dmgScreen;
    private IEnumerator checkpointDelay()
    {
        yield return new WaitForSeconds(0.5f);
        LevelState.disableCheckpointscreen(checkpoint);
    }

    private IEnumerator damageDelay()
    {
        yield return new WaitForSeconds(0.5f);
        LevelState.damagescreenOff(dmgScreen);
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "CheckPoint"){
            LevelState.enableCheckpointScreen(checkpoint);
            StartCoroutine(checkpointDelay());
            Debug.Log("test");
        }

        if (other.gameObject.tag == "Damage" || other.gameObject.tag == "WrongDoor"){
            if (GameObject.Find("Player").GetComponent<HealthState>().damaged()){
                LevelState.died(loseScreen);
                Debug.Log("damage");
            } else {
                LevelState.damageTaken(dmgScreen);
                StartCoroutine(damageDelay());
                Debug.Log("dead");
            };
        }

        if (other.gameObject.tag == "finish"){
            Debug.Log("finish");
            LevelState.endPointReached(winScreen);            
        }
    }
}
