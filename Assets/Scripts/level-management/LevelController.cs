using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static System.Net.Mime.MediaTypeNames;
using System.IO;

public class LevelController : MonoBehaviour
{
    public List<CreatedObject.Data> createdObjects;
    private GameObject[] placeableObjectPrefabs;

    private LevelEditor level;

    private GameObject UI;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SaveLevel(string levelName)
    {
        level = GetComponent<PlacementController>().level;

        if (level == null)
        {
            UI.GetComponent<UIController>().messagePanel.SetActive(true);
            UI.GetComponent<UIController>().message.text = "Level is leeg";
            StartCoroutine(UI.GetComponent<UIController>().CloseMessagePanel());
        }

        foreach (CreatedObject.Data data in level.createdObjects)
        {
            Debug.Log(data.tag);
        }

        Debug.Log("Saving.");


        string json = JsonUtility.ToJson(level);
        string folder = UnityEngine.Application.dataPath + "/Saved/";
        string levelFile = levelName + ".json";

        Debug.Log("Saving..");

        //Create new directory if LevelData directory does not yet exist.
        if (!Directory.Exists(folder))
            Directory.CreateDirectory(folder);

        Debug.Log("Saving...");

        string path = Path.Combine(folder, levelFile); // set filepath

        //Overwrite file with same name, if applicable
        if (File.Exists(path))
            File.Delete(path);

        Debug.Log("Saving....");

        // create and save file
        File.WriteAllText(path, json);

        Debug.Log("Saved succesfully");

        Debug.Log("Saving to DB");

        GetComponent<EditorDatabase>().SaveLevel(level, levelName);

        StartCoroutine(WaitAndDeleteFile(levelName));


    }

    private IEnumerator WaitAndDeleteFile(string levelName)
    {
        yield return new WaitForSeconds(1.0f); // Wait for 1 second

        string folder = UnityEngine.Application.dataPath + "/Saved/";
        string levelFile = levelName + ".json";
        string path = Path.Combine(folder, levelFile); // set filepath

        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log("File deleted after waiting for 1 second.");
        }
    }





    // Loading a level
    public void LoadLevel(string levelName)
    {
        Debug.Log("Loading level");

        level = GetComponent<EditorDatabase>().LoadLevel(levelName);


        if (level == null)
        {
            UI.GetComponent<UIController>().messagePanel.SetActive(true);
            UI.GetComponent<UIController>().message.text = "Level bestaat niet";
            StartCoroutine(UI.GetComponent<UIController>().CloseMessagePanel());
        }
        else
        {

            Debug.Log("Loading level.");

            CreatedObject[] foundObjects = FindObjectsOfType<CreatedObject>();
            foreach (CreatedObject obj in foundObjects)
                Destroy(obj.gameObject);

            GameObject.Find("Floors").GetComponent<FloorplanController>().ClearFloors();

            Debug.Log("Loading level..");


            CreateFromFile(); // create objects from level data.

            Debug.Log("Loading floors");

            List<FloorData.Data> floors = level.floors;

            Debug.Log("Loading level...");

            foreach (FloorData.Data floor in floors)
                GameObject.Find("Floors").GetComponent<FloorplanController>().LoadFloorplanFromSave(floor.floorNumber, floor.floorPlanPath);

            GameObject.Find("Floors").GetComponent<FloorplanController>().ActivateGroundFloor();

            GetComponent<PlacementController>().level = level;

            Debug.Log("Level loaded");
        }



        string folder = UnityEngine.Application.dataPath + "/Saved/";
        string levelFile = levelName + ".json";

        string path = Path.Combine(folder, levelFile); // set filepath

        if (File.Exists(path)) // if the file could be found in LevelData
        {
            Debug.Log("Loading level");


            CreatedObject[] foundObjects = FindObjectsOfType<CreatedObject>();
            foreach (CreatedObject obj in foundObjects)
                Destroy(obj.gameObject);

            GameObject.Find("Floors").GetComponent<FloorplanController>().ClearFloors();

            Debug.Log("Loading level.");

            string json = File.ReadAllText(path); // provide text from json file
            level = JsonUtility.FromJson<LevelEditor>(json); // level information filled from json file

            Debug.Log("Loading level..");

            CreateFromFile(); // create objects from level data.

            Debug.Log("Loading floors");

            List<FloorData.Data> floors = level.floors;

            foreach (FloorData.Data floor in floors)
                GameObject.Find("Floors").GetComponent<FloorplanController>().LoadFloorplanFromSave(floor.floorNumber, floor.floorPlanPath);

            GameObject.Find("Floors").GetComponent<FloorplanController>().ActivateGroundFloor();

            Debug.Log("Level loaded");
        }
        else // if the file could not be found
        {
            Debug.Log("File not found");
            UI.GetComponent<UIController>().messagePanel.SetActive(true);
            UI.GetComponent<UIController>().message.text = "Level " + levelName + " bestaat niet!";
            StartCoroutine(UI.GetComponent<UIController>().CloseMessagePanel());
        }
    }

    void CreateFromFile()
    {
        placeableObjectPrefabs = GetComponent<PlacementController>().placeableObjectPrefabs;

        foreach (CreatedObject.Data data in level.createdObjects)
        {
            Debug.Log("Loading object..");
            for (int i = 0; i < placeableObjectPrefabs.Length; i++)
            {
                if (placeableObjectPrefabs[i].tag == data.tag)
                {
                    Debug.Log("Creating object..");
                    GameObject obj = Instantiate(placeableObjectPrefabs[i], data.position, data.rotation);
                    obj.transform.localScale = data.scale;

                    CreatedObject newObjData = obj.AddComponent<CreatedObject>();
                    newObjData.data = data;

                }
            }
        }
    }
}
