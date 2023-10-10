using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using SimpleFileBrowser;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using TMPro;
using System.Linq;
using Dummiesman;
using Unity.VisualScripting;

public class TextureUploadController : MonoBehaviour
{
    public Button uploadButton;
    public GameObject UI;
    public GameObject selectedHitboxPrefab;
  

    private int clickCount = 0;
    private float cameraMoveSpeed = 5.0f; // Adjust this value to control camera movement speed
    private float cameraRotationSpeed = 2.0f; // Adjust this value to control camera rotation speed

    private Vector3 lastMousePosition;
    private bool isLeftDragging = false;
    private bool isRightDragging = false;


    public TMP_Dropdown prefabDropdown;

    public Slider scaleSlider, xAxisSlider, yAxisSlider, zAxisSlider;

    public Toggle toggleCamera;

    public Toggle hitboxToggle;

    private void Start()
    {
/*        // No need to set up currentFloorPlane here, as it's not used in this script
        // CurrentFloorPlane should be set up in the Unity Editor or your other scripts.
        // Instead, reference the ground plane directly.

        string[] fileNames = Directory.GetFiles("Assets/3dmodel prefabs");

        //prefabDropdown.ClearOptions();

        List<TMP_Dropdown.OptionData> prefabOptions = new List<TMP_Dropdown.OptionData>();

        foreach (string fileName in fileNames)
        {
            if (fileName.EndsWith(".meta")) continue;
            prefabOptions.Add(new TMP_Dropdown.OptionData(Path.GetFileNameWithoutExtension(fileName)));
        }

        prefabDropdown.AddOptions(prefabOptions);*/

    }

    private void Update()
    {
        // You can access newModel here or in other functions
    }

    IEnumerator ClickTimer()
    {
        yield return new WaitForSeconds(0.5f);
        clickCount = 0;
    }

    public void Upload()
    {
        Debug.Log("Model path: " + modelPath);

        if (!string.IsNullOrEmpty(modelPath))
        {
            // Use a coroutine to load the 3D model asynchronously
            StartCoroutine(LoadModelAsync(modelPath));
        }
        else
        {
            UI.GetComponent<UIController>().messagePanel.SetActive(true);
            UI.GetComponent<UIController>().message.text = "No 3D model selected";
            StartCoroutine(UI.GetComponent<UIController>().CloseMessagePanel());
        }
    }

    private IEnumerator LoadModelAsync(string modelPath)
    {
        // Load the 3D model from the relative file path asynchronously
        GameObject test = new OBJLoader().Load(modelPath);
        UI.GetComponent<UIController>().messagePanel.SetActive(true);
        UI.GetComponent<UIController>().message.text = "start loading object!";
        // Wait until the model is loaded
        while (!test)
        {
            yield return null;
        }
        StartCoroutine(UI.GetComponent<UIController>().CloseMessagePanel());
        GameObject newModel = GameObject.Find("Modeltest");

        if (newModel != null)
        {
            // Set the position to(0, 0, -5)
             newModel.transform.position = new Vector3(0f, 0f, -5f);

            // Set the rotation to (0, 180, 0)
            newModel.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            // Display a success message
            UI.GetComponent<UIController>().messagePanel.SetActive(true);
            UI.GetComponent<UIController>().message.text = "Model successfully uploaded";
            StartCoroutine(UI.GetComponent<UIController>().CloseMessagePanel());
            // Disable upload button
            uploadButton.gameObject.SetActive(false);
        }
        else
        {
            // Display an error message
            UI.GetComponent<UIController>().messagePanel.SetActive(true);
            UI.GetComponent<UIController>().message.text = "Failed to upload model";
            StartCoroutine(UI.GetComponent<UIController>().CloseMessagePanel());
        }
        
    }



    private void UpdateDropdownOptions()
    {
        // Load all prefabs from the "3DModelPrefabs" folder in the "Resources" folder
        GameObject[] prefabs = Resources.LoadAll<GameObject>("3DModelPrefabs");

        prefabDropdown.ClearOptions();

        List<TMP_Dropdown.OptionData> prefabOptions = new List<TMP_Dropdown.OptionData>();

        foreach (GameObject prefab in prefabs)
        {
            prefabOptions.Add(new TMP_Dropdown.OptionData(prefab.name));
        }

        prefabDropdown.AddOptions(prefabOptions);
    }

    public void OnDropdownValueChanged()
    {
        int value = prefabDropdown.value;
        string selectedPrefabName = prefabDropdown.options[value].text;

        // Load the selected prefab from the "Resources" folder
        string prefabPath = "3dmodel prefabs/" + selectedPrefabName;
        GameObject selectedHitboxPrefab = Resources.Load<GameObject>(prefabPath);

        if (selectedHitboxPrefab != null)
        {
            // Remove existing hitboxes
            RemoveExistingHitboxes();

            // Add the newly selected hitbox
            AddSelectedHitbox(selectedHitboxPrefab);
            Debug.Log("Selected prefab: " + selectedHitboxPrefab.name);
        }
        else
        {
            Debug.Log("Selected prefab: " + selectedPrefabName);
            Debug.LogError("Failed to load selected prefab: " + selectedPrefabName);
            Debug.LogError("Make sure the prefab is in the 'Resources/3dmodel prefabs' folder.");
        }
    }

    public void AddSelectedHitbox(GameObject selectedHitboxPrefab)
    {
          GameObject newModel = GameObject.Find("Modeltest");   
        if (selectedHitboxPrefab != null && newModel != null)
        {
            // Instantiate the selected hitbox prefab and set its parent to the newModel
            GameObject newHitbox = Instantiate(selectedHitboxPrefab, newModel.transform);

            // Optionally, adjust the position and rotation of the new hitbox
            newHitbox.transform.localPosition = Vector3.zero; // Adjust the position as needed
            newHitbox.transform.localRotation = Quaternion.identity; // Adjust the rotation as needed

            // Add a tag to the newHitbox to identify it as a hitbox
            newHitbox.tag = "Wall";
        }
    }


    private void RemoveExistingHitboxes()
    {
        GameObject newModel = GameObject.Find("Modeltest");
        if (newModel != null)
        {
            // Find all child objects with the tag "Hitbox" and destroy them
            Transform[] hitboxes = newModel.GetComponentsInChildren<Transform>().Where(child => child.CompareTag("Wall")).ToArray();

            foreach (Transform hitbox in hitboxes)
            {
                Destroy(hitbox.gameObject);
            }
        }
    }

    public void MoveCameraToGround()
    {
       
    }
    public void OpenFileExplorer()
    {
        StartCoroutine(ShowLoadDialogCoroutine());
    }

    private string modelPath; // Add a field to store the model path

    IEnumerator ShowLoadDialogCoroutine()
    {
        // Show a load file dialog and wait for a response from the user
        yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.Files, false, null, null, "Load 3D Model", "Load");

        if (FileBrowser.Success)
        {
            // Assign the selected model path to the modelPath variable before calling Upload
            modelPath = FileBrowser.Result[0];
            Upload();
        }
    }

    public void ScaleModel()
    {
        GameObject scalableModel = GameObject.Find("Modeltest");
        if (scalableModel != null && scaleSlider.value > 0.0f)
        {
            // Scale the model uniformly using the scaleValue.
            scalableModel.transform.localScale = Vector3.one * scaleSlider.value;
        }
    }

    public void ToggleCamera()
    {
        GameObject camera = GameObject.Find("Camera");

        if (camera != null)
        {
            SandboxCameraController cameraController = camera.GetComponent<SandboxCameraController>();

            if (cameraController != null)
            {
                // Enable or disable camera movement based on the Toggle state.
                cameraController.enabled = toggleCamera.isOn;
            }
            else
            {
                Debug.LogError("SandboxCameraController script not found on the camera.");
            }
        }
        else
        {
            Debug.LogError("Camera object not found.");
        }
    }

    public void ScaleHitbox()
    {
        GameObject[] wallObjects = GameObject.FindGameObjectsWithTag("Wall");

        foreach (GameObject wallObject in wallObjects)
        {
            Vector3 currentScale = wallObject.transform.localScale;

            // Adjust the scaling based on the values of the X, Y, and Z sliders
            currentScale.x = xAxisSlider.value;
            currentScale.y = yAxisSlider.value;
            currentScale.z = zAxisSlider.value;

            wallObject.transform.localScale = currentScale;
        }
    }

    public void ToggleHitbox()
    {
        GameObject wallObject = GameObject.FindGameObjectWithTag("Wall");

        MeshRenderer renderer = wallObject.GetComponent<MeshRenderer>();


                if (hitboxToggle != null)
                {
                    if (renderer != null)
                    {
                        // Toggle the visibility of the MeshRenderer based on the Toggle's state
                        renderer.enabled = hitboxToggle.isOn;
                    }   
                    else
                    {
                        Debug.LogError("MeshRenderer not found on object with the 'Wall' tag.");
                    }
                } else
                {
                    Debug.Log("Toggle is not found");
                }        
    }
}