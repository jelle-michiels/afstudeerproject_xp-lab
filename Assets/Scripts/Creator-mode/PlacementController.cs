using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class PlacementController : MonoBehaviour
{
    public GameObject[] placeableObjectPrefabs;

    private GameObject currentPlaceableObject;

    public Material placing;
    public Material selectedMaterial;

    private float height;
    private float initialHeight;

    private Material objectMaterial;

    public float gridSize = 0.05f;

    public LevelEditor level;

    private Button heightPlusButton;
    private Button heightMinusButton;
    private Button smallRotateButton;
    private Button largeRotateButton;
    private Button widthPlusButton;
    private Button widthMinusButton;
    private Button lengthPlusButton;
    private Button lengthMinusButton;

    private Button wall;
    private Button floor;
    private Button wallPart;
    private Button stairs;
    private Button checkpoint;
    private Button realCheckpoint;
    private Button damagePoint;
    private Button endpoint;
    private Button wrongDoor;
    private Button correctDoor;

    private Button heightButton;
    private Button deleteButton;

    private TextMeshProUGUI heightText;
    private float heightForText;
    private float displayHeight;

    private GameObject selected;
    private Vector3 selectedPosition;

    private GameObject selectedObject;

    private GameObject UI;



    void Start()
    {
        UI = GameObject.Find("UI");

        heightPlusButton = GameObject.Find("HeightPlusButton").GetComponent<Button>();
        heightMinusButton = GameObject.Find("HeightMinusButton").GetComponent<Button>();


        smallRotateButton = GameObject.Find("RotateSmall").GetComponent<Button>();
        largeRotateButton = GameObject.Find("RotateLarge").GetComponent<Button>();

        widthPlusButton = GameObject.Find("WidthPlusButton").GetComponent<Button>();
        widthMinusButton = GameObject.Find("WidthMinusButton").GetComponent<Button>();


        lengthPlusButton = GameObject.Find("LengthPlusButton").GetComponent<Button>();
        lengthMinusButton = GameObject.Find("LengthMinusButton").GetComponent<Button>();


        selected = GameObject.Find("Selected");
        selectedPosition = selected.transform.position;
        selected.SetActive(false);

        wall = GameObject.Find("Wall").GetComponent<Button>();
        floor = GameObject.Find("Floor").GetComponent<Button>();
        wallPart = GameObject.Find("WallPart").GetComponent<Button>();
        stairs = GameObject.Find("Stairs").GetComponent<Button>();
        checkpoint = GameObject.Find("Checkpoint").GetComponent<Button>();
        endpoint = GameObject.Find("Endpoint").GetComponent<Button>();
        wrongDoor = GameObject.Find("WrongDoor").GetComponent<Button>();
        correctDoor = GameObject.Find("CorrectDoor").GetComponent<Button>();
        if (GameObject.Find("DamagePoint") != null)
        { damagePoint = GameObject.Find("DamagePoint").GetComponent<Button>(); }

        realCheckpoint = GameObject.Find("RealCheckpoint").GetComponent<Button>();


        heightButton = GameObject.Find("HeightButton").GetComponent<Button>();
        heightText = heightButton.GetComponentInChildren<TextMeshProUGUI>();

        deleteButton = GameObject.Find("Delete").GetComponent<Button>();

        heightPlusButton.onClick.AddListener(IncreaseHeight);
        heightMinusButton.onClick.AddListener(DecreaseHeight);
        smallRotateButton.onClick.AddListener(RotateObject);
        largeRotateButton.onClick.AddListener(RotateObjectQuick);
        widthPlusButton.onClick.AddListener(IncreaseWidth);
        widthMinusButton.onClick.AddListener(DecreaseWidth);
        lengthPlusButton.onClick.AddListener(IncreaseLength);
        lengthMinusButton.onClick.AddListener(DecreaseLength);
        wall.onClick.AddListener(() => { ChangeObject(0); });
        floor.onClick.AddListener(() => { ChangeObject(1); });
        wallPart.onClick.AddListener(() => { ChangeObject(2); });
        stairs.onClick.AddListener(() => { ChangeObject(3); });
        checkpoint.onClick.AddListener(() => { ChangeObject(4); });
        endpoint.onClick.AddListener(() => { ChangeObject(5); });
        wrongDoor.onClick.AddListener(() => { ChangeObject(6); });
        correctDoor.onClick.AddListener(() => { ChangeObject(7); });
        damagePoint.onClick.AddListener(() => { ChangeObject(8); });
        realCheckpoint.onClick.AddListener(() => { ChangeObject(9); });

        deleteButton.onClick.AddListener(() => { DestroyObject(selectedObject); });

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
                    Destroy(currentPlaceableObject);
                    heightText.text = "Hoogte";
                    selected.SetActive(false);
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
        wall.interactable = UI.GetComponent<UIController>().allowInput;
        floor.interactable = UI.GetComponent<UIController>().allowInput;
        wallPart.interactable = UI.GetComponent<UIController>().allowInput;
        if (heightButton != null)
        {
            heightButton.interactable = UI.GetComponent<UIController>().allowInput;
        }
        if (deleteButton != null)
        {
            deleteButton.interactable = UI.GetComponent<UIController>().allowInput;
        }



        if (!UI.GetComponent<UIController>().allowInput)
        {
            selectedObject = null;
            currentPlaceableObject = null;
            selected.SetActive(false);
        }



    }

    private void HandleNewObjectHotkey()
    {

        for (int i = 0; i < placeableObjectPrefabs.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0 + 1 + i))
            {
                ChangeObject(i);

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

    void ChangeObject(int i)
    {


        if (currentPlaceableObject != null && selectedObject == null)
        {
            Destroy(currentPlaceableObject);
        }

        if (selectedObject != null)
        {
            selectedObject.GetComponent<MeshRenderer>().material = objectMaterial;

            if (currentPlaceableObject.tag == "Stairs" || currentPlaceableObject.tag == "CorrectDoor" || currentPlaceableObject.tag == "WrongDoor")
            {
                foreach (Transform child in currentPlaceableObject.transform)
                {
                    child.GetComponent<MeshRenderer>().material = selectedObject.GetComponent<MeshRenderer>().material;
                }
            }

            selectedObject = null;
        }



        currentPlaceableObject = Instantiate(placeableObjectPrefabs[i]);
        objectMaterial = currentPlaceableObject.GetComponent<MeshRenderer>().material;
        currentPlaceableObject.GetComponent<MeshRenderer>().material = placing;
        placing.SetColor("_Color", new Color(0.3f, 0.8f, 1f, 0.5f));

        selected.SetActive(true);
        selected.transform.position = selectedPosition + new Vector3(80 * i, 0, 0);

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
        foreach (CreatedObject.Data createdObjectData in level.createdObjectsData)
        {
            GameObject objectToRemove = FindGameObjectByCreatedObjectData(createdObjectData);

            Destroy(objectToRemove);
        }

        level.createdObjectsData.Clear();
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




}
