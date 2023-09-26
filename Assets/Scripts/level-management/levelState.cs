using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LevelState
{
    public static void endPointReached(GameObject screen){
        screen.SetActive(true);
    }

    public static void checkpointReached(GameObject checkpoint){
        checkpoint.SetActive(true);
        new WaitForSeconds(2);
        checkpoint.SetActive(false);
     }

    public static void died(GameObject screen){
        screen.SetActive(true);
    }
}
