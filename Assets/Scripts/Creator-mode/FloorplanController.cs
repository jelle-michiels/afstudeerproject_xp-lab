using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.IO;
using SimpleFileBrowser;

public class FloorplanController : MonoBehaviour
{
    private Button uploadButton;
    private Button scratchButton;
    private GameObject floors;

    public GameObject floorPlane;

    private GameObject currentFloorPlane;

    private GameObject ground;

    private string imagePath;
    private GameObject UI;
    private int clickCount = 0;

    private List<Button> uploadButtons = new List<Button>();

    private int currentFloor;

    private Dictionary<int, GameObject> floorplans;

    // Start is called before the first frame update
    void Start()
    {
        floors = GameObject.Find("Floors");
        scratchButton = GameObject.Find("ScratchButton").GetComponent <Button>();
        scratchButton.onClick.AddListener(StartFromScratch);
        uploadButton = GameObject.Find("UploadButton").GetComponent<Button>();
        uploadButton.onClick.AddListener(OpenFileExplorer);
        uploadButtons.Add(uploadButton);
        currentFloor = 0;
        ground = GameObject.Find("Ground");
        currentFloorPlane = ground;

        FloorData floorData = currentFloorPlane.AddComponent<FloorData>();
        floorData.data.floorNumber = currentFloor;

        GameObject.Find("Mouse").GetComponent<PlacementController>().level.floors.Add(floorData.data);

        floorplans = new Dictionary<int, GameObject>();
        floorplans.Add(0, currentFloorPlane);


        UI = GameObject.Find("UI");

        FileBrowser.SetFilters(true, new FileBrowser.Filter("Images", ".jpg", ".png"));
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
                    //on double click
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

            if (Input.GetKeyDown(KeyCode.PageUp))
            {
                currentFloor++;
                SwitchFloor();
            }
            if (Input.GetKeyDown(KeyCode.PageDown))
            {
                if (currentFloor > 0)
                {
                    currentFloor--;
                    SwitchFloor();
                }

            }
        }
        else
        {
            uploadButton.gameObject.SetActive(false);
        }
    }

    void Upload()
    {

        /*// Open file explorer to select image file
        imagePath = UnityEditor.EditorUtility.OpenFilePanel("Select Image", "", "png,jpg,jpeg");*/

        Debug.Log("Image path: " + imagePath);

        if (imagePath != "")
        {
            // Load image from file path
            byte[] imageData = File.ReadAllBytes(imagePath);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(imageData);


            Material floorplan = new Material(Shader.Find("Standard"));
            floorplan.mainTexture = texture;

            currentFloorPlane.GetComponent<MeshRenderer>().material = floorplan;

            uploadButton.gameObject.SetActive(false);


            currentFloorPlane.GetComponent<FloorData>().data.floorPlanPath = imagePath;

            GameObject.Find("Mouse").GetComponent<PlacementController>().level.floors[currentFloor] = currentFloorPlane.GetComponent<FloorData>().data;


            UI.GetComponent<UIController>().messagePanel.SetActive(true);
            UI.GetComponent<UIController>().message.text = "Vloerplan succesvol geupload";
            StartCoroutine(UI.GetComponent<UIController>().CloseMessagePanel());


        }
        else
        {
            UI.GetComponent<UIController>().messagePanel.SetActive(true);
            UI.GetComponent<UIController>().message.text = "Geen vloerplan geselecteerd";
            StartCoroutine(UI.GetComponent<UIController>().CloseMessagePanel());
        }

    }
    void StartFromScratch()
    {
        uploadButton.gameObject.SetActive(false);
        scratchButton.gameObject.SetActive(false);
    }

    public void LoadFloorplanFromSave(int floor, string imagePath)
    {

        Debug.Log("Loading floorplan from save");

        Debug.Log("Floor " + floor + " " + imagePath);

        if (floor == 0)
        {
            Debug.Log("Adding ground floor");

            currentFloorPlane = ground;
            floorplans.Add(0, currentFloorPlane);

            FloorData floorData = currentFloorPlane.AddComponent<FloorData>();
            floorData.data.floorNumber = floor;

            GameObject.Find("Mouse").GetComponent<PlacementController>().level.floors.Add(floorData.data);

        }
        else
        {
            Debug.Log("Adding floor " + floor);

            currentFloorPlane = Instantiate(floorPlane, floors.transform);
            currentFloorPlane.name = "Floor " + currentFloor;
            currentFloorPlane.transform.position = new Vector3(0, 3 * currentFloor, 0);
            floorplans.Add(floor, currentFloorPlane);

            uploadButton = GameObject.Find("UploadButton").GetComponent<Button>();
            uploadButton.onClick.AddListener(Upload);

            uploadButtons.Add(uploadButton);

            FloorData floorData = currentFloorPlane.AddComponent<FloorData>();
            floorData.data.floorNumber = currentFloor;

            GameObject.Find("Mouse").GetComponent<PlacementController>().level.floors.Add(floorData.data);
        }

        if (!string.IsNullOrEmpty(imagePath))
        {
            byte[] imageData = File.ReadAllBytes(imagePath);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(imageData);

            Debug.Log("Loading Image");

            Material floorplan = new Material(Shader.Find("Standard"));
            floorplan.mainTexture = texture;

            currentFloorPlane.GetComponent<MeshRenderer>().material = floorplan;

            Debug.Log("Setting Material");


            currentFloorPlane.GetComponent<FloorData>().data.floorPlanPath = imagePath;

            GameObject.Find("Mouse").GetComponent<PlacementController>().level.floors[currentFloor] = currentFloorPlane.GetComponent<FloorData>().data;


        }
    }

    

    IEnumerator ClickTimer()
    {
        yield return new WaitForSeconds(0.5f);
        clickCount = 0;
    }

    void SwitchFloor()
    {
        GameObject previousFloor = currentFloorPlane;
        uploadButton.gameObject.SetActive(false);

        if (!floorplans.ContainsKey(currentFloor))
        {
            currentFloorPlane = Instantiate(floorPlane, floors.transform);
            currentFloorPlane.name = "Floor " + currentFloor;
            currentFloorPlane.transform.position = new Vector3(0, 3 * currentFloor, 0);
            floorplans.Add(currentFloor, currentFloorPlane);

            uploadButton = GameObject.Find("UploadButton").GetComponent<Button>();
            uploadButton.onClick.AddListener(OpenFileExplorer);

            uploadButtons.Add(uploadButton);

            FloorData floorData = currentFloorPlane.AddComponent<FloorData>();
            floorData.data.floorNumber = currentFloor;
            floorData.data.floorPlanPath = "";

            GameObject.Find("Mouse").GetComponent<PlacementController>().level.floors.Add(floorData.data);

        }
        else
        {
            currentFloorPlane = floorplans[currentFloor];
        }
        
        previousFloor.SetActive(false);
        currentFloorPlane.SetActive(true);

        uploadButton = uploadButtons[currentFloor];
        uploadButton.gameObject.SetActive(true);

        UI.GetComponent<UIController>().messagePanel.SetActive(false);
        UI.GetComponent<UIController>().messagePanel.SetActive(true);
        UI.GetComponent<UIController>().message.text = "Niveau " + currentFloor;
        StartCoroutine(UI.GetComponent<UIController>().CloseMessagePanel());



    }

    public void ClearFloors()
    {
        foreach (KeyValuePair<int, GameObject> floor in floorplans)
        {
            if (floor.Key != 0)
            {
                Destroy(floor.Value);
            }
            else
            {
                floor.Value.GetComponent<MeshRenderer>().material = new Material(Shader.Find("Standard"));
                floor.Value.GetComponent<FloorData>().data.floorPlanPath = "";
                uploadButton = uploadButtons[0];
            }
        }

        floorplans.Clear();
        uploadButtons.Clear();

        uploadButtons.Add(uploadButton);
    }

    public void ActivateGroundFloor()
    {
        currentFloorPlane = ground;
        currentFloorPlane.SetActive(true);
        currentFloor = 0;
        uploadButton = uploadButtons[0];
        uploadButton.gameObject.SetActive(true);
    }

    void OpenFileExplorer()
    {
        StartCoroutine(ShowLoadDialogCoroutine());
    }
    
    IEnumerator ShowLoadDialogCoroutine()
    {

        // Show a load file dialog and wait for a response from user
        // Load file/folder: both, Allow multiple selection: true
        // Initial path: default (Documents), Initial filename: empty
        // Title: "Load File", Submit button text: "Load"
        yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.FilesAndFolders, false, null, null, "Load Files and Folders", "Load");

        // Dialog is closed
        // Print whether the user has selected some files/folders or cancelled the operation (FileBrowser.Success)
        Debug.Log(FileBrowser.Success);

        if (FileBrowser.Success)
        {
            /*// Print paths of the selected files (FileBrowser.Result) (null, if FileBrowser.Success is false)
            for (int i = 0; i < FileBrowser.Result.Length; i++)
                Debug.Log(FileBrowser.Result[i]);

            // Read the bytes of the first file via FileBrowserHelpers
            // Contrary to File.ReadAllBytes, this function works on Android 10+, as well
            byte[] bytes = FileBrowserHelpers.ReadBytesFromFile(FileBrowser.Result[0]);

            // Or, copy the first file to persistentDataPath
            string destinationPath = Path.Combine(Application.persistentDataPath, FileBrowserHelpers.GetFilename(FileBrowser.Result[0]));
            FileBrowserHelpers.CopyFile(FileBrowser.Result[0], destinationPath);*/
            imagePath = FileBrowser.Result[0];
            Upload();
        }
    }



}

