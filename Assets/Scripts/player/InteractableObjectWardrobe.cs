using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UIElements.Image;

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
            txtToDisplay = canvasObject.transform.Find("KeyboardE").gameObject;
        }
        txtToDisplay.gameObject.SetActive(false);
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
        txtToDisplay.gameObject.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        playerInZone = false;
        txtToDisplay.gameObject.SetActive(false);
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