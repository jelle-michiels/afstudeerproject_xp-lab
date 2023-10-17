using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class InteractableObjectWardrobe : MonoBehaviour
{
    public GameObject[] boxes;

    public GameObject attachedObject;
    
    private GameObject txtToDisplay;
    private bool playerInZone;

    
    enum ObjectState
    {
        filled,
        empty
    }

    void Start()
    {
        GameObject canvasObject = GameObject.Find("InteractableCanvas");
        if (canvasObject != null)
        {
            txtToDisplay = canvasObject.transform.Find("ObjectText").gameObject;
        }
    }


    // any trigger laat het afgaan
    private void OnTriggerEnter(Collider other)
    {
        if (txtToDisplay == null)
        {
            txtToDisplay = GameObject.Find("InteractableCanvas").transform.Find("ObjectText").gameObject;
        }

        txtToDisplay.GetComponent<Text>().text = "Press 'F' to interact";
        playerInZone = true;
    }

    private void OnTriggerExit(Collider other)
    {
        txtToDisplay.GetComponent<Text>().text = "";
        playerInZone = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && playerInZone)
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
