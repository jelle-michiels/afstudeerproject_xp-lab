using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObjectWarderobe : MonoBehaviour
{
    public GameObject[] boxes;

    public GameObject attachedObject;
    

    enum ObjectState
    {
        filled,
        empty
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            toggleBoxes();
        }
    }

    public void toggleBoxes()
    {
        foreach (GameObject box in boxes)
        {
            box.SetActive(!box.activeSelf);
        }
    }

    
}
