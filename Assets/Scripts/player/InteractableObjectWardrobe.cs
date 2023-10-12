using Microsoft.Unity.VisualStudio.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class InteractableObjectWardrobe : MonoBehaviour
{
    public GameObject[] boxes;

    public GameObject attachedObject;
    public Image keybboardE;
    
    private GameObject txtToDisplay;
    private bool playerInZone;

    
    enum ObjectState
    {
        filled,
        empty
    }

    void Start()
    {
        if (keybboardE != null)
        {
            keybboardE.gameObject.SetActive(false);
        }
    }


    // any trigger laat het afgaan
    private void OnTriggerEnter(Collider other)
    {
        //interactableCanvas.SetActive(true);
        //txtToDisplay.GetComponent<Text>().text = "Press 'E' or 'X' on the controller to interact";
        playerInZone = true;
    }

    private void OnTriggerExit(Collider other)
    {
        txtToDisplay.GetComponent<Text>().text = "";
        playerInZone = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerInZone || Input.GetKeyDown(KeyCode.JoystickButton0) && playerInZone)
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
