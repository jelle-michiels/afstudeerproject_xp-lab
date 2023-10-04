using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using SimpleFileBrowser;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using TMPro;

public class TextureUploadController : MonoBehaviour
{
    public Button uploadButton;
    public GameObject ground; // Reference to the ground plane in your scene
    public GameObject UI;
    public GameObject selectedHitboxPrefab;

    private int clickCount = 0;
    private float cameraMoveSpeed = 5.0f; // Adjust this value to control camera movement speed
    private float cameraRotationSpeed = 2.0f; // Adjust this value to control camera rotation speed

    private Vector3 lastMousePosition;
    private bool isLeftDragging = false;
    private bool isRightDragging = false;

    private GameObject newModel; // Define a class-level variable to store the new model

    public TMP_Dropdown prefabDropdown;

    private void Start()
    {
        // No need to set up currentFloorPlane here, as it's not used in this script
        // CurrentFloorPlane should be set up in the Unity Editor or your other scripts.
        // Instead, reference the ground plane directly.

        string[] fileNames = Directory.GetFiles("Assets/3dmodel prefabs");

        prefabDropdown.ClearOptions();

        List<TMP_Dropdown.OptionData> prefabOptions = new List<TMP_Dropdown.OptionData>();

        foreach (string fileName in fileNames)
        {
            if (fileName.EndsWith(".meta")) continue;
            prefabOptions.Add(new TMP_Dropdown.OptionData(Path.GetFileNameWithoutExtension(fileName)));
        }

        prefabDropdown.AddOptions(prefabOptions);

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
            // Get the filename from the full path
            string fileName = Path.GetFileName(modelPath);

            // Define the target path within the "3dModels" folder in the Assets directory
            string targetPath = "Assets/3dModels/" + fileName;

            // Check if the file already exists at the target location
            if (!File.Exists(targetPath))
            {
                // Copy the selected 3D model file to the target path
                FileUtil.CopyFileOrDirectory(modelPath, targetPath);
            }

            // Load the 3D model from the relative file path
            GameObject modelPrefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(targetPath);

            if (modelPrefab != null)
            {
                // Instantiate the 3D model on the ground plane
                Vector3 spawnPosition = ground.transform.position + Vector3.up; // Adjust the height
                newModel = Instantiate(modelPrefab, spawnPosition, Quaternion.identity); // Store it in newModel

                // Optionally, you can set the parent of the new model to the ground to keep the hierarchy organized
                newModel.transform.parent = ground.transform;

                // Set a specific name for the new model
                newModel.name = "Modeltest";

                // Find the 'camera' child object under 'Modeltest'
                Transform cameraTransform = newModel.transform.Find("Camera");

                if (cameraTransform != null)
                {
                    // Attach the SandboxCameraController script to the 'camera' child object
                    SandboxCameraController cameraController = cameraTransform.gameObject.AddComponent<SandboxCameraController>();
                }
                else
                {
                    Debug.LogError("No 'camera' child object found under 'Modeltest'.");
                }

                // Display a success message
                UI.GetComponent<UIController>().messagePanel.SetActive(true);
                UI.GetComponent<UIController>().message.text = "Model successfully uploaded";
                StartCoroutine(UI.GetComponent<UIController>().CloseMessagePanel());
                // Disable upload button
                uploadButton.gameObject.SetActive(false);
            }
            else
            {
                Debug.LogError("Failed to load the 3D model.");
            }
        }
        else
        {
            UI.GetComponent<UIController>().messagePanel.SetActive(true);
            UI.GetComponent<UIController>().message.text = "No 3D model selected";
            StartCoroutine(UI.GetComponent<UIController>().CloseMessagePanel());
        }
    }

    public void AddSelectedHitbox()
    {
        if (selectedHitboxPrefab != null && newModel != null)
        {
            // Instantiate the selected hitbox prefab and set its parent to the newModel
            GameObject newHitbox = Instantiate(selectedHitboxPrefab, newModel.transform);

            // Optionally, adjust the position and rotation of the new hitbox
            newHitbox.transform.localPosition = Vector3.zero; // Adjust the position as needed
            newHitbox.transform.localRotation = Quaternion.identity; // Adjust the rotation as needed
        }
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

    //void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.name == )
    //}
}
