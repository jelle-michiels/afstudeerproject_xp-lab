using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class InteractableObjectWarderobe : MonoBehaviour
{
    public GameObject[] boxes;

    public GameObject attachedObject;
    
    private bool playerInZone;
    enum ObjectState
    {
        filled,
        empty
    }

    // any trigger laat het afgaan
    private void OnTriggerEnter(Collider other)
    {
        playerInZone = true;
    }

    private void OnTriggerExit(Collider other)
    {
        playerInZone = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerInZone)
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
