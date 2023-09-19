using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowTimeSettings : MonoBehaviour
{
   public GameObject gameobject;
   

   public void whenButtonClicked(){
    if(gameobject.activeInHierarchy == true){
        gameobject.SetActive(false);
    }else {
        gameobject.SetActive(true);
    }
   }
}

