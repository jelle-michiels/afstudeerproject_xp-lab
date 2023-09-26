using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LevelState
{
    public static void endPointReached(GameObject screen){
        screen.SetActive(true);
        Debug.Log("endpoint reached");
    }

    public static void checkpointReached(GameObject checkpoint){
        checkpoint.SetActive(false);
        Debug.Log("checkpoint reached");
    }

    public static void died(GameObject screen){
        screen.SetActive(true);
    }
}
