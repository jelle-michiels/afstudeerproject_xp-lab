using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectSwitch : MonoBehaviour
{
    //public GameObject[] randomPrefabs;

    private GameObject txtToDisplay;

    private GameObject interactable;
    private bool playerInZone;
    private void Start()
    {
        txtToDisplay = GameObject.Find("InteractableCanvas").transform.Find("ObjectText").gameObject;
        /*if (randomPrefabs == null || randomPrefabs.Length == 0)
        {
            Debug.LogError("No random prefabs assigned to ObjectInteraction script.");
            enabled = false; // Disable the script if there are no prefabs.
        }*/
    }

    private void OnTriggerEnter(Collider other)
    {
        txtToDisplay.GetComponent<Text>().text = "Press 'E' to interact";
        playerInZone = true;
        interactable = other.gameObject;
    }

    private void OnTriggerExit(Collider other)
    {
        txtToDisplay.GetComponent<Text>().text = "";
        playerInZone = false;
        interactable = null;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerInZone)
        {
            Interact();
        }
    }

    private void Interact()
    {
        //int randomIndex = Random.Range(0, randomPrefabs.Length);
        //GameObject newObject = Instantiate(randomPrefabs[randomIndex], transform.position, transform.rotation);
        Destroy(interactable); // Remove the old object
    }
}
