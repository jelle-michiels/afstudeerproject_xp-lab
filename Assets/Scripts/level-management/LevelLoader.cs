using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    private Button loadLevel;
    private string activeLevel;

    public GameObject[] placeableObjectPrefabs;

    private LevelEditor level;

    public PlayerControl playerControl;

    // Start is called before the first frame update
    void Start()
    {
        playerControl.enabled = false;
        activeLevel = PlayerPrefs.GetString("ActiveLevel");
        StartCoroutine(DelayedLoadLevel());
        

    }

    IEnumerator DelayedLoadLevel()
    {
        yield return new WaitForSeconds(0.01f); // Adjust the delay time as needed
        LoadLevel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadLevel()
    {
        /*activeLevel = PlayerPrefs.GetString("activeLevel");*/

        Debug.Log("Loading level");

        level = GetComponent<EditorDatabase>().LoadLevel(activeLevel);

        if (level == null)
        {
            Debug.Log("Level not found");
        }
        else
        {

            CreateFromFile();
            GameObject.Find("Player").GetComponent<SpawnManager>().SpawnPlayer();
        }

    }

    void CreateFromFile()
    {
        foreach (CreatedObject.Data data in level.createdObjectsData)
        {
            Debug.Log("Loading object..");
            for (int i = 0; i < placeableObjectPrefabs.Length; i++)
            {
                if (placeableObjectPrefabs[i].tag == data.tag)
                {
                    Debug.Log("Creating object " + data.tag);
                    GameObject obj = Instantiate(placeableObjectPrefabs[i], data.position, data.rotation);
                    if (data.tag == "Wardrobe" || data.tag == "PoorWardrobe")
                {
                    // Modify the Y-position to be 1.5 units lower
                    Vector3 newPosition = obj.transform.position;
                    newPosition.y -= 1.5f;
                    obj.transform.position = newPosition;
                }

                    obj.transform.localScale = data.scale;
                    Debug.Log("data.position: " + data.position);

                    CreatedObject newObjData = obj.AddComponent<CreatedObject>();
                    newObjData.data = data;

                    if (data.tag == "Spawnpoint")
                    {
                        GameObject.Find("Player").GetComponent<SpawnManager>().SetSpawnPoint(obj.transform);
                        obj.SetActive(false);
                    }

                    if (data.tag == "Endpoint")
                    {
                        
                        /*obj.SetActive(false);*/
                    }

                }
            }
            
        }
    }
}
