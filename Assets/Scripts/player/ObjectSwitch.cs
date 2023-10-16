using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectSwitch : MonoBehaviour
{
    public GameObject[] randomPrefabs;

    private GameObject txtToDisplay;

    private GameObject interactable;
    private bool playerInZone;
    private void Start()
    {
        playerInZone = false;
        txtToDisplay = GameObject.Find("InteractableCanvas").transform.Find("ObjectText").gameObject;
        if (randomPrefabs == null || randomPrefabs.Length == 0)
        {
            Debug.LogError("No random prefabs assigned to ObjectInteraction script.");
            enabled = false; // Disable the script if there are no prefabs.
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if (ArrayContainsObjectWithName(randomPrefabs, other.gameObject.tag))
        {
            Debug.Log("Player in zone");
            txtToDisplay.GetComponent<Text>().text = "\n Press 'F' to interact";
            playerInZone = true;
            interactable = other.gameObject;
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        txtToDisplay.GetComponent<Text>().text = "";
        playerInZone = false;
        interactable = null;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && playerInZone)
        {
            Interact();
        }
    }

    private void Interact()
    {
        int randomIndex = Random.Range(0, randomPrefabs.Length);
        Vector3 spawnPosition = interactable.transform.position + (transform.position - interactable.transform.position).normalized;
        GameObject newObject = Instantiate(randomPrefabs[randomIndex], spawnPosition, transform.rotation);
        Debug.Log(interactable.name);
        interactable.SetActive(false); // Disable the old object
        //Destroy(interactable); // Remove the old object
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
