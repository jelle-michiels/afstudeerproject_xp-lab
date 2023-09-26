using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LevelState
{
    public static void endPointReached(GameObject screen){
        screen.SetActive(true);
    }

    private static IEnumerator checkpointDelay(GameObject screen)
    {
        yield return new WaitForSeconds(3f); // Adjust the delay time as needed
        disableCheckpointscreen(screen);
    }

    public static void enableCheckpointScreen(GameObject screen)
    {
        screen.SetActive(true);
        checkpointDelay(screen);
    }

    public static void disableCheckpointscreen(GameObject checkpoint){
        checkpoint.SetActive(false);
     }

    public static void died(GameObject screen){
        screen.SetActive(true);
    }
}
