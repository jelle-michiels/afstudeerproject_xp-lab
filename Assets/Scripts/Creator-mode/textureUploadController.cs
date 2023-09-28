using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using SimpleFileBrowser;
using System.Collections;
using System.Collections.Generic;

public class TextureUploadController : MonoBehaviour
{
    public Button uploadButton;
    public Button scratchButton;
    public GameObject floors;
    public GameObject floorPrefab;
    public GameObject ground;

    private GameObject currentFloorPlane;
    private string modelPath;
    public GameObject UI;
    private int clickCount = 0;
    private int currentFloor = 0;

    // Start is called before the first frame update
    void Start()
    {
        FileBrowser.SetFilters(true, new FileBrowser.Filter("3D Models", ".fbx", ".obj"));
        FileBrowser.AddQuickLink("Users", "C:\\Users", null);
    }

    // Update is called once per frame
    void Update()
    {
        if (UI.GetComponent<UIController>().allowInput)
        {
            Ray ray;
            RaycastHit hit;

            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == currentFloorPlane)
                {
                    // On double click
                    if (Input.GetMouseButtonDown(0))
                    {
                        clickCount++;
                        if (clickCount == 2)
                        {
                            uploadButton.gameObject.SetActive(!uploadButton.gameObject.activeSelf);
                            clickCount = 0;
                        }
                        else
                        {
                            StartCoroutine(ClickTimer());
                        }
                    }
                }
            }
        }
        else
        {
            uploadButton.gameObject.SetActive(false);
        }
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
            // Load the 3D model from the file path
            GameObject modelPrefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(modelPath);

            if (modelPrefab != null)
            {
                // Instantiate the 3D model as the current floor
                GameObject newFloor = Instantiate(modelPrefab, currentFloorPlane.transform.position, Quaternion.identity);
                newFloor.name = "Floor " + currentFloor;

                // Replace the current floor with the new floor
                Destroy(currentFloorPlane);
                currentFloorPlane = newFloor;

                UI.GetComponent<UIController>().messagePanel.SetActive(true);
                UI.GetComponent<UIController>().message.text = "Model successfully uploaded";
                StartCoroutine(UI.GetComponent<UIController>().CloseMessagePanel());
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

    public void StartFromScratch()
    {
        uploadButton.gameObject.SetActive(false);
        scratchButton.gameObject.SetActive(false);
    }

    public void OpenFileExplorer()
    {
        StartCoroutine(ShowLoadDialogCoroutine());
    }

    IEnumerator ShowLoadDialogCoroutine()
    {
        // Show a load file dialog and wait for a response from the user
        yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.Files, false, null, null, "Load 3D Model", "Load");

        if (FileBrowser.Success)
        {
            modelPath = FileBrowser.Result[0];
            Upload();
        }
    }
}
