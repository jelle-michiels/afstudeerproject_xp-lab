using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LevelState
{
    public static void endPointReached(GameObject screen){
        screen.SetActive(true);
    }

    public static void enableCheckpointScreen(GameObject screen)
    {
        screen.SetActive(true);
    }

    public static void disableCheckpointscreen(GameObject checkpoint){
        checkpoint.SetActive(false);
        Debug.Log("disabled");
     }

    public static void died(GameObject screen){
        screen.SetActive(true);
    }
}
