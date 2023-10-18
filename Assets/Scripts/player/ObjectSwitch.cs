using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectSwitch : MonoBehaviour
{
    public GameObject[] randomPrefabs;

    private GameObject iconToDisplay;
    private GameObject controllerToDisplay;

    private GameObject interactable;

    private bool playerInZone;

    private int index;
    private void Start()
    {
        index = 0;
        playerInZone = false;
        if (randomPrefabs == null || randomPrefabs.Length == 0)
        {
            Debug.LogError("No random prefabs assigned to ObjectInteraction script.");
            enabled = false; // Disable the script if there are no prefabs.
        }
        GameObject canvasObject = GameObject.Find("SwitchCanvas");
        if (canvasObject != null)
        {
            iconToDisplay = canvasObject.transform.Find("KeyboardE").gameObject;
            controllerToDisplay = canvasObject.transform.Find("ControllerIcon").gameObject;
        }
        iconToDisplay.gameObject.SetActive(false);
        controllerToDisplay.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        GameObject canvasObject = GameObject.Find("SwitchCanvas");
        if (ArrayContainsObjectWithName(randomPrefabs, other.gameObject.tag))
        {
            Debug.Log(other.gameObject.tag);
            Debug.Log("Player in zone");
            iconToDisplay = canvasObject.transform.Find("KeyboardE").gameObject;
            controllerToDisplay = canvasObject.transform.Find("ControllerIcon").gameObject;
            iconToDisplay.gameObject.SetActive(true);
            controllerToDisplay.gameObject.SetActive(true);
            playerInZone = true;
            interactable = other.gameObject;
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        GameObject canvasObject = GameObject.Find("SwitchCanvas");
        iconToDisplay = canvasObject.transform.Find("KeyboardE").gameObject;
        controllerToDisplay = canvasObject.transform.Find("ControllerIcon").gameObject;
        iconToDisplay.gameObject.SetActive(false);
        controllerToDisplay.gameObject.SetActive(false);
        playerInZone = false;
        interactable = null;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerInZone || Input.GetKeyDown(KeyCode.JoystickButton0) && playerInZone)
        {
            Interact();
        }
    }

    private void indexCheck(){
        if(index == randomPrefabs.Length - 1){
            index = 0;
        }else{
            index++;
        }
    }

    private void Interact()
    {
        indexCheck();
        Vector3 spawnPosition = interactable.transform.position;
        Quaternion spawnRotation = interactable.transform.rotation;
        GameObject newObject = Instantiate(randomPrefabs[index], spawnPosition, spawnRotation);

        Debug.Log(interactable.name);
        interactable.SetActive(false); // Disable the old object
    }

    private bool ArrayContainsObjectWithName(GameObject[] array, string objectName)
    {
        foreach (GameObject obj in array)
        {
            if (obj.tag == objectName)
            {
                return true;
            }
        }
        return false;
    }

}
