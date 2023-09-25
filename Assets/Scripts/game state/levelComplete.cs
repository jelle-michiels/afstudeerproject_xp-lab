using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class levelComplete : MonoBehaviour
{

    public GameObject youWinPanel;

    private void OnTrigerredEnter(Collider collider){
        if (collider.tag == "player"){
            youWinPanel.SetActive(true);
            Debug.Log("endpoint reached");
        }
    }

    
}
