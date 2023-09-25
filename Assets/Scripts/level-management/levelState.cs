using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class levelState
{
    public static void FinishReached(){
        GameObject.Find("winScreen").SetActive(true);
        Debug.Log("endpoint reached");
    }
}
