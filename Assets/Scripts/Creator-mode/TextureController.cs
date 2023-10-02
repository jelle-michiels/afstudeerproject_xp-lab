using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
public class TextureController : MonoBehaviour
{


    private bool isRightClicking = false;
    private Vector3 lastMousePosition;
    private float cameraRotationSpeed = 2.0f;
    private float cameraPanSpeed = 0.1f;
    private float cameraZoomSpeed = 10.0f;

    public GameObject[] placeableObjectPrefabs;

    private GameObject currentPlaceableObject;

    public Material placing;
    public Material selectedMaterial;

    private float height;
    private float initialHeight;

    private Material objectMaterial;

    public float gridSize = 0.05f;

    public LevelEditor level;

    
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

        selected = GameObject.Find("Selected");
        selectedPosition = selected.transform.position;
        selected.SetActive(false);

        deleteButton = GameObject.Find("Delete").GetComponent<Button>();

        deleteButton.onClick.AddListener(() => { DestroyObject(selectedObject); });

    }


    // Update is called once per frame
    void Update()
    {
        if (UI.GetComponent<UIController>().allowInput)
        {
         
                // Handle camera rotation
                if (Input.GetMouseButtonDown(1))
                {
                    isRightClicking = true;
                    lastMousePosition = Input.mousePosition;
                }
                else if (Input.GetMouseButtonUp(1))
                {
                    isRightClicking = false;
                }

                if (isRightClicking)
                {
                    Vector3 deltaMousePosition = Input.mousePosition - lastMousePosition;
                    transform.Rotate(Vector3.up, deltaMousePosition.x * cameraRotationSpeed, Space.World);
                    transform.Rotate(Vector3.left, deltaMousePosition.y * cameraRotationSpeed);
                    lastMousePosition = Input.mousePosition;
                }

                // Handle camera panning
                float horizontalInput = Input.GetAxis("Horizontal");
                float verticalInput = Input.GetAxis("Vertical");

                if (Mathf.Abs(horizontalInput) > 0 || Mathf.Abs(verticalInput) > 0)
                {
                    Vector3 cameraDirection = new Vector3(horizontalInput, 0, verticalInput).normalized;
                    transform.Translate(cameraDirection * cameraPanSpeed);
                }

                // Handle camera zooming
                float scrollWheelInput = Input.GetAxis("Mouse ScrollWheel");

                if (Mathf.Abs(scrollWheelInput) > 0)
                {
                    float zoomAmount = scrollWheelInput * cameraZoomSpeed;
                    Vector3 newPosition = transform.position + transform.forward * zoomAmount;
                    // Limit the zoom to a minimum and maximum distance if necessary
                    // Example: newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);
                    transform.position = newPosition;
                }


            }
            // -----
        
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

                level.createdObjects.Add(newObjData.data);

            }
        }

    }

    private float SnapToGrid(float n)
    {
        return (Mathf.Round(n / gridSize) * gridSize) / 2;
    }

    void DestroyObject(GameObject selectedObject)
    {
        level.createdObjects.Remove(selectedObject.GetComponent<CreatedObject>().data);
        Destroy(selectedObject);
        selectedObject = null;

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


}
