using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using UnityEditor;
using System;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.Events;
using Dummiesman;

public class PlacementController : MonoBehaviour
{
    public List<GameObject> placeableObjectPrefabs = new List<GameObject>();
    public List<GameObject> placeableModels = new List<GameObject>();

    private GameObject currentPlaceableObject;

    public Material placing;
    public Material selectedMaterial;

    private float height;
    private float initialHeight;
    private float heightForText;
    private float displayHeight;

    private Material objectMaterial;

    public float gridSize = 0.05f;

    public LevelEditor level;

    public Button heightPlusButton;
    public Button heightMinusButton;
    public Button smallRotateButton;
    public Button largeRotateButton;
    public Button widthPlusButton;
    public Button widthMinusButton;
    public Button lengthPlusButton;
    public Button lengthMinusButton;
    public Button deleteButton;

    public TextMeshProUGUI heightText;

    public GameObject selected;
    public GameObject buttonTemplate;
    private GameObject selectedObject;
    private GameObject UI;

    private Vector3 selectedPosition;

    public Transform buttonPanel;

    void Start()
    {
        UI = GameObject.Find("UI");

        //Height buttons
        heightPlusButton.onClick.AddListener(IncreaseHeight);
        heightMinusButton.onClick.AddListener(DecreaseHeight);

        //Rotation buttons
        smallRotateButton.onClick.AddListener(RotateObject);
        largeRotateButton.onClick.AddListener(RotateObjectQuick);

        //Transform object buttons
        widthPlusButton.onClick.AddListener(IncreaseWidth);
        widthMinusButton.onClick.AddListener(DecreaseWidth);
        lengthPlusButton.onClick.AddListener(IncreaseLength);
        lengthMinusButton.onClick.AddListener(DecreaseLength);

        //Delete button
        deleteButton.onClick.AddListener(() => { DestroyObject(selectedObject); });

        //Selected object
        selectedPosition = selected.transform.position;
        selected.SetActive(false);

        // Load prefabs for building
        placeableObjectPrefabs = Resources.LoadAll<GameObject>("BuilderPrefabs").ToList();

        // Create buttons for prefabs
        CreatePrefabButtons();

        //Create buttons for own 3D models
        CreateJsonFileButtons();
    }


    // Update is called once per frame
    void Update()
    {
        if (UI.GetComponent<UIController>().allowInput)
        {
            if (!Input.GetKeyDown(KeyCode.LeftControl))
            {
                HandleNewObjectHotkey();
            }


            if (currentPlaceableObject != null)
            {


                MoveCurrentObjectToMouse();

                if (Input.GetMouseButtonDown(0))
                {
                    CreateObject();

                }

                if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.LeftShift))
                {
                    Debug.Log(level.createdObjectsData.Count);
                    if (selectedObject != null)
                    {
                        DestroyObject(selectedObject);
                    }
                    Destroy(currentPlaceableObject);
                    heightText.text = "Hoogte";
                    selected.SetActive(false);
                    Debug.Log(level.createdObjectsData.Count);
                }
            }

            if (!selected.activeSelf)
            {
                if (!EventSystem.current.IsPointerOverGameObject()) // if not over UI
                {

                    if (Input.GetMouseButtonDown(0))
                    {
                        ToggleSelection();
                    }

                }
            }

            if (Input.GetKey(KeyCode.R))
            {
                RemoveAllCreatedObjects();
            }

            if (Input.GetKey(KeyCode.Delete))
            {
                DestroyObject(selectedObject);

            }

            if (Input.GetMouseButtonDown(1))
            {
                RotateObjectQuick();
            }

            if (Input.GetMouseButtonDown(2))
            {
                RotateObject();

            }

            if (Input.GetKeyDown(KeyCode.KeypadPlus) || Input.GetKeyDown(KeyCode.P))
            {
                IncreaseHeight();
            }

            if (Input.GetKeyDown(KeyCode.KeypadMinus) || Input.GetKeyDown(KeyCode.M))
            {
                DecreaseHeight();
            }

            if (Input.GetKeyDown(KeyCode.Keypad4) || (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.Keypad4)) || Input.GetKeyDown(KeyCode.I) || (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.I)))
            {
                DecreaseWidth();
            }

            if (Input.GetKeyDown(KeyCode.Keypad6) || (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.Keypad6)) || Input.GetKeyDown(KeyCode.K) || (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.K)))
            {
                IncreaseWidth();
            }

            if (Input.GetKeyDown(KeyCode.Keypad2) || (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.Keypad2)) || Input.GetKeyDown(KeyCode.O) || (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.O)))
            {
                DecreaseLength();
            }

            if (Input.GetKeyDown(KeyCode.Keypad8) || (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.Keypad8)) || Input.GetKeyDown(KeyCode.L) || (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.L)))
            {
                IncreaseLength();
            }

        }

        heightPlusButton.interactable = UI.GetComponent<UIController>().allowInput;
        heightMinusButton.interactable = UI.GetComponent<UIController>().allowInput;
        smallRotateButton.interactable = UI.GetComponent<UIController>().allowInput;
        largeRotateButton.interactable = UI.GetComponent<UIController>().allowInput;
        widthPlusButton.interactable = UI.GetComponent<UIController>().allowInput;
        widthMinusButton.interactable = UI.GetComponent<UIController>().allowInput;
        lengthPlusButton.interactable = UI.GetComponent<UIController>().allowInput;
        lengthMinusButton.interactable = UI.GetComponent<UIController>().allowInput;
        deleteButton.interactable = UI.GetComponent<UIController>().allowInput;

        if (!UI.GetComponent<UIController>().allowInput)
        {
            selectedObject = null;
            currentPlaceableObject = null;
            selected.SetActive(false);
        }
    }

    private void HandleNewObjectHotkey()
    {
        for (int i = 0; i < placeableObjectPrefabs.Count; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0 + 1 + i))
            {
                ChangeObject(placeableObjectPrefabs[i]); // Pass the selected prefab.
            }
        }
    }

    private void MoveCurrentObjectToMouse()
    {

        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition) * 2;
        Vector3 position = new Vector3(SnapToGrid(mousePosition.x), height, SnapToGrid(mousePosition.z));
        currentPlaceableObject.transform.position = Vector3.Lerp(transform.position, position, 1f);
    }

    void CreateObject()
    {
        if (!EventSystem.current.IsPointerOverGameObject()) // if not over UI
        {
            if (currentPlaceableObject != null)
            {
                if (currentPlaceableObject.tag == "Spawnpoint")
                {
                    if (GameObject.FindGameObjectsWithTag("Spawnpoint").Length > 1)
                    {
                        return;
                    }
                }

                if (currentPlaceableObject.tag == "Endpoint")
                {
                    if (GameObject.FindGameObjectsWithTag("Endpoint").Length > 1)
                    {
                        return;
                    }
                }


                GameObject newObj = Instantiate(currentPlaceableObject);
                newObj.GetComponent<MeshRenderer>().material = objectMaterial;

                if (currentPlaceableObject.tag == "Stairs" || currentPlaceableObject.tag == "CorrectDoor" || currentPlaceableObject.tag == "WrongDoor")
                {
                    foreach (Transform child in newObj.transform)
                    {
                        child.GetComponent<MeshRenderer>().material = newObj.GetComponent<MeshRenderer>().material;
                    }
                }

                CreatedObject newObjData = newObj.AddComponent<CreatedObject>();
                newObjData.data.position = newObj.transform.position;
                newObjData.data.rotation = newObj.transform.rotation;
                newObjData.data.scale = newObj.transform.localScale;
                newObjData.data.tag = newObj.tag;

                Debug.Log(newObjData.data.tag);

                level.createdObjectsData.Add(newObjData.data);

            }
        }

    }

    void RotateObject()
    {
        currentPlaceableObject.transform.Rotate(0, 15, 0);
    }

    void RotateObjectQuick()
    {
        currentPlaceableObject.transform.Rotate(0, 90, 0);
    }

    private float SnapToGrid(float n)
    {
        return (Mathf.Round(n / gridSize) * gridSize) / 2;
    }

    void DestroyObject(GameObject selectedObject)
    {
        level.createdObjectsData.Remove(selectedObject.GetComponent<CreatedObject>().data);
        Destroy(selectedObject);
        selectedObject = null;

    }

    void IncreaseHeight()
    {
        if (currentPlaceableObject.tag == "WallPart")
        {
            height += 0.25f;
            heightForText += 0.25f;
            heightText.text = "Hoogte: " + heightForText.ToString();
        }
        else
        {
            height += 3f;
            heightForText += 3f;
            displayHeight = heightForText / 3;
            heightText.text = "Hoogte: " + displayHeight;
        }
    }

    void DecreaseHeight()
    {
        if (height > initialHeight)
        {
            if (currentPlaceableObject.tag == "WallPart")
            {
                height -= 0.5f;
                heightForText -= 0.5f;
                heightText.text = "Hoogte: " + heightForText.ToString();
            }
            else
            {
                height -= 3f;
                heightForText -= 3f;
                displayHeight = heightForText / 3;
                heightText.text = "Hoogte: " + displayHeight;
            }

        }
    }

    void DecreaseLength()
    {
        if (currentPlaceableObject.transform.localScale.x > 0.5f)
        {
            currentPlaceableObject.transform.localScale -= new Vector3(0.05f, 0, 0);
        }
    }

    void IncreaseLength()
    {
        currentPlaceableObject.transform.localScale += new Vector3(0.05f, 0, 0);
    }

    void DecreaseWidth()
    {


        if (currentPlaceableObject.transform.localScale.z > 0.2f)
        {
            currentPlaceableObject.transform.localScale -= new Vector3(0, 0, 0.05f);
        }

    }

    void IncreaseWidth()
    {


        if (currentPlaceableObject.tag == "Floor" || currentPlaceableObject.transform.localScale.z < 3f)
        {
            currentPlaceableObject.transform.localScale += new Vector3(0, 0, 0.05f);
        }

    }

    void ChangeObject(GameObject prefabToInstantiate)
    {
        if (currentPlaceableObject != null && selectedObject == null)
        {
            Destroy(currentPlaceableObject);
        }

        if (selectedObject != null)
        {
            selectedObject.GetComponent<MeshRenderer>().material = objectMaterial;

            if (currentPlaceableObject.tag == "Stairs" || currentPlaceableObject.tag == "WrongDoor" || currentPlaceableObject.tag == "CorrectDoor")
            {
                foreach (Transform child in currentPlaceableObject.transform)
                {
                    child.GetComponent<MeshRenderer>().material = selectedObject.GetComponent<MeshRenderer>().material;
                }
            }

            selectedObject = null;
        }

        currentPlaceableObject = Instantiate(prefabToInstantiate);
        objectMaterial = currentPlaceableObject.GetComponent<MeshRenderer>().material;
        currentPlaceableObject.GetComponent<MeshRenderer>().material = placing;
        placing.SetColor("_Color", new Color(0.3f, 0.8f, 1f, 0.5f));

        selected.SetActive(true);
        int prefabIndex = Array.IndexOf(placeableObjectPrefabs.ToArray(), prefabToInstantiate);
        float xPos = 80 * prefabIndex;
        selected.transform.position = selectedPosition + new Vector3(xPos, 0, 0);


        if (currentPlaceableObject.tag == "Floor")
        {
            height = 0;
            initialHeight = 0;
            heightForText = 0;
        }
        else if (currentPlaceableObject.tag == "WallPart")
        {
            height = 0.25f;
            initialHeight = 0.25f;
            heightForText = 0;
        }
        else if (currentPlaceableObject.tag == "Stairs" || currentPlaceableObject.tag == "WrongDoor" || currentPlaceableObject.tag == "CorrectDoor")
        {
            foreach (Transform child in currentPlaceableObject.transform)
            {
                child.GetComponent<MeshRenderer>().material = currentPlaceableObject.GetComponent<MeshRenderer>().material;
            }

            if (currentPlaceableObject.tag == "Stairs")
            {
                height = 0.3f;
                initialHeight = 0.3f;
                heightForText = 0;
            }
            else
            {
                height = 0;
                initialHeight = 0;
                heightForText = 0;
            }
        }
        else if (currentPlaceableObject.tag == "Spawnpoint" || currentPlaceableObject.tag == "Endpoint")
        {
            height = 0.5f;
            initialHeight = 0.5f;
            heightForText = 0;
        }
        else
        {
            height = 1.5f;
            initialHeight = 1.5f;
            heightForText = 0;
        }

        heightText.text = "Hoogte: " + heightForText.ToString();
    }


    void ToggleSelection()
    {

        Ray ray;
        RaycastHit hit;

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (selectedObject != null)
        {
            selectedObject.GetComponent<MeshRenderer>().material = objectMaterial;
            if (selectedObject.tag == "Stairs" || selectedObject.tag == "WrongDoor" || selectedObject.tag == "CorrectDoor")
            {
                foreach (Transform child in selectedObject.transform)
                {
                    child.GetComponent<MeshRenderer>().material = selectedObject.GetComponent<MeshRenderer>().material;
                }
            }
            selectedObject = null;
            currentPlaceableObject = null;
            heightText.text = "Hoogte";
        }

        if (Physics.Raycast(ray, out hit))
        {


            if (hit.transform.gameObject.transform.parent != null)
            {

                if (hit.transform.gameObject.transform.parent.gameObject.tag == "Stairs")
                {
                    selectedObject = hit.transform.gameObject.transform.parent.gameObject;
                }


            }
            else if (hit.transform.gameObject.tag == "Floor" || hit.transform.gameObject.tag == "WallPart" || hit.transform.gameObject.tag == "Wall" || hit.transform.gameObject.tag == "Spawnpoint" || hit.transform.gameObject.tag == "Endpoint" || hit.transform.gameObject.tag == "WrongDoor" || hit.transform.gameObject.tag == "CorrectDoor")
            {
                selectedObject = hit.transform.gameObject;
            }

            if (selectedObject != null)
            {

                objectMaterial = selectedObject.GetComponent<MeshRenderer>().material;
                selectedObject.GetComponent<MeshRenderer>().material = selectedMaterial;
                currentPlaceableObject = selectedObject;

                if (selectedObject.tag == "Floor")
                {
                    height = selectedObject.transform.position.y;
                    initialHeight = 0f;
                    heightForText = selectedObject.transform.position.y;

                }
                if (selectedObject.tag == "WallPart")
                {
                    height = selectedObject.transform.position.y;
                    initialHeight = 0.25f;
                    heightForText = selectedObject.transform.position.y - 0.25f;

                }
                if (selectedObject.tag == "Spawnpoint" || currentPlaceableObject.tag == "Endpoint")
                {
                    height = selectedObject.transform.position.y;
                    initialHeight = 0.5f;
                    heightForText = selectedObject.transform.position.y - 0.5f;
                }
                if (selectedObject.tag == "Stairs" || selectedObject.tag == "WrongDoor" || selectedObject.tag == "CorrectDoor")
                {
                    foreach (Transform child in selectedObject.transform)
                    {
                        child.GetComponent<MeshRenderer>().material = selectedObject.GetComponent<MeshRenderer>().material;
                    }

                    if (selectedObject.tag == "Stairs")
                    {
                        height = selectedObject.transform.position.y;
                        initialHeight = 0.3f;
                        heightForText = selectedObject.transform.position.y - 0.3f;
                    }
                    else
                    {
                        height = selectedObject.transform.position.y;
                        initialHeight = 0;
                        heightForText = selectedObject.transform.position.y;
                    }


                }
                if (selectedObject.tag == "Wall")
                {

                    height = selectedObject.transform.position.y;
                    initialHeight = 1.5f;
                    heightForText = selectedObject.transform.position.y - 1.5f;

                }

                heightText.text = "Hoogte: " + heightForText.ToString();

            }


        }


    }

    public void RemoveAllCreatedObjects()
    {
        Debug.Log(level.createdObjectsData.Count);
        foreach (CreatedObject.Data createdObjectData in level.createdObjectsData)
        {
            GameObject objectToRemove = FindGameObjectByCreatedObjectData(createdObjectData);

            Destroy(objectToRemove);
        }

        level.createdObjectsData.Clear();
        Debug.Log(level.createdObjectsData.Count);
    }

    private GameObject FindGameObjectByCreatedObjectData(CreatedObject.Data createdObjectData)
    {
        foreach (GameObject gameObject in GameObject.FindObjectsOfType<GameObject>())
        {
            CreatedObject objectComponent = gameObject.GetComponent<CreatedObject>();

            if (objectComponent != null && AreCreatedObjectDataEqual(objectComponent.data, createdObjectData))
            {
                return gameObject;
            }
        }
        return null;
    }

    private bool AreCreatedObjectDataEqual(CreatedObject.Data data1, CreatedObject.Data data2)
    {
        return data1.position == data2.position && data1.rotation == data2.rotation
            && data1.scale == data2.scale && data1.tag == data2.tag;
    }

    private void CreatePrefabButtons()
    {
        foreach (GameObject prefab in placeableObjectPrefabs)
        {
            GameObject buttonGO = Instantiate(buttonTemplate);
            buttonGO.transform.SetParent(buttonPanel, false);

            TextMeshProUGUI buttonText = buttonGO.GetComponentInChildren<TextMeshProUGUI>();

            string prefabNameWithSpaces = AddSpacesToPrefabName(prefab.name);
            buttonText.text = prefabNameWithSpaces;

            buttonGO.GetComponent<Button>().onClick.AddListener(() => { ChangeObject(prefab); });
        }
    }

    private void CreateJsonFileButtons()
    {
        string jsonFilesDirectory = Application.persistentDataPath; // Assuming saved JSON files are in the persistent data path

        // Get a list of .json files in the directory
        string[] jsonFilePaths = Directory.GetFiles(jsonFilesDirectory, "*.json");

        foreach (string jsonFilePath in jsonFilePaths)
        {
            GameObject buttonGO = Instantiate(buttonTemplate);
            buttonGO.transform.SetParent(buttonPanel, false);

            TextMeshProUGUI buttonText = buttonGO.GetComponentInChildren<TextMeshProUGUI>();

            string jsonFileName = Path.GetFileNameWithoutExtension(jsonFilePath);
            buttonText.text = jsonFileName;

            buttonGO.GetComponent<Button>().onClick.AddListener(() => { Load(jsonFilePath); });
        }

        Destroy(buttonTemplate);
    }

    private string AddSpacesToPrefabName(string prefabName)
    {
        StringBuilder spacedName = new StringBuilder();
        spacedName.Append(prefabName[0]);

        for (int i = 1; i < prefabName.Length; i++)
        {
            if (char.IsUpper(prefabName[i]))
            {
                spacedName.Append(' ');
            }
            spacedName.Append(prefabName[i]);
        }

        return spacedName.ToString();
    }

    public void Load(string objPath)
    {

        Debug.Log("Loading from: " + objPath);
        // Define the path to load the JSON file
        string loadPath = objPath;

        if (File.Exists(loadPath))
        {
            // Read the JSON data from the file
            string json = File.ReadAllText(loadPath);

            // Deserialize the JSON data into the SaveData class
            SaveData loadedData = JsonUtility.FromJson<SaveData>(json);
            string filepathtoload = Path.Combine(Application.persistentDataPath, loadedData.filename);

            GameObject loadedObject = new OBJLoader().Load(filepathtoload);

            if (loadedObject != null)
            {
                // Transform the object's scale based on the loaded data
                loadedObject.transform.localScale = new Vector3(loadedData.objectScale.x, loadedData.objectScale.y, loadedData.objectScale.z);

                // Load the hitbox prefab from the Resources folder
                GameObject hitbox = Resources.Load<GameObject>("HitboxPrefabs/" + loadedData.hitbox.name);

                GameObject hitboxPrefab = Directory.GetFiles("Assets/Resources/HitboxPrefabs").Where(x => x.Contains(loadedData.hitbox.name)).Select(x => AssetDatabase.LoadAssetAtPath<GameObject>(x)).FirstOrDefault();
                if (hitboxPrefab != null)
                {
                    // Create an instance of the hitbox prefab
                    GameObject instantiatedHitbox = Instantiate(hitboxPrefab);

                    // Parent the hitbox to the loaded object
                    instantiatedHitbox.transform.parent = loadedObject.transform;
                    Vector3 positionLoadedObject = loadedObject.transform.position;

                    // Transform the hitbox's position, rotation, and scale based on the loaded data
                    // instantiatedHitbox.transform.position = positionLoadedObject;
                    MeshRenderer renderer = instantiatedHitbox.GetComponent<MeshRenderer>();
                    renderer.enabled = false;
                    instantiatedHitbox.transform.localPosition = new Vector3(loadedData.hitbox.position.x, loadedData.hitbox.position.y, loadedData.hitbox.position.z + 5);
                    //instantiatedHitbox.transform.position = new Vector3(loadedData.hitbox.position.x, loadedData.hitbox.position.y, loadedData.hitbox.position.z);
                    instantiatedHitbox.transform.rotation = Quaternion.Euler(loadedData.hitbox.rotation.x, loadedData.hitbox.rotation.y, loadedData.hitbox.rotation.z);
                    instantiatedHitbox.transform.localScale = new Vector3(loadedData.hitbox.scale.x, loadedData.hitbox.scale.y, loadedData.hitbox.scale.z);

                    // Optionally, you can further adjust or apply other properties as needed
                }
                else
                {
                    Debug.LogError("Hitbox prefab not found: " + loadedData.hitbox.name);
                }
            }
            else
            {
                Debug.LogError("Object not found: " + loadedData.filename);
            }
        }
        else
        {
            Debug.LogError("File not found: " + loadPath);
        }
    }

    [System.Serializable]
    class SaveData
    {
        public string filename;
        public ScaleData objectScale;
        public HitboxData hitbox;
    }

    [System.Serializable]
    class RotationData
    {
        public float x;
        public float y;
        public float z;
    }

    [System.Serializable]
    class ScaleData
    {
        public float x;
        public float y;
        public float z;
    }

    [System.Serializable]
    class HitboxData
    {
        public string name;
        public PositionData position;
        public RotationData rotation;
        public ScaleData scale;
    }

    [System.Serializable]
    class PositionData
    {
        public float x;
        public float y;
        public float z;
    }
}