using UnityEngine;

public class FindGameObjectWithScript : MonoBehaviour
{
    public string scriptNameToFind = "PlacementController"; // Replace with the name of the script you're looking for

    void Start()
    {
        // Find all GameObjects with the specified script attached
        GameObject[] gameObjectsWithScript = GameObject.FindObjectsOfType<GameObject>();

        foreach (GameObject obj in gameObjectsWithScript)
        {
            // Check if the GameObject has the script attached
            if (obj.GetComponent(scriptNameToFind) != null)
            {
                // Do something with the GameObject that has the script
                Debug.Log("Found GameObject with script: " + obj.name);

                
            }
        }
    }
}