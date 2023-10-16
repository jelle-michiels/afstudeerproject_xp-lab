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

    private int index;
    private void Start()
    {
        index = 0;
        playerInZone = false;
        txtToDisplay = GameObject.Find("SwitchCanvas").transform.Find("ObjectText").gameObject;
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
            txtToDisplay.GetComponent<Text>().text = "Press 'F' to switch object";
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

    private void indexCheck(){
        if(index == randomPrefabs.Length - 1){
            index = 0;
        }else{
            index++;
        }
    }

    private void Interact()
    {
        //int randomIndex = Random.Range(0, randomPrefabs.Length);

        indexCheck();
        Vector3 spawnPosition = interactable.transform.position + (transform.position - interactable.transform.position).normalized;
        GameObject newObject = Instantiate(randomPrefabs[index], spawnPosition, transform.rotation);
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
