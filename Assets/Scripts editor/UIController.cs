using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEditor;



public class UIController : MonoBehaviour
{

    private GameObject controlsText;
    private Button controls;
    private GameObject mainUI;

    private GameObject saveAndLoadUI;
    private GameObject saveMenu;
    private GameObject loadMenu;
    private GameObject loadPanel;
    private GameObject saveNameInputObject;
    private InputField saveNameInput;
    private Button saveFileButton;

    private Button saveButton;
    private Button loadButton;

    private Button closeSave;
    private Button closeLoad;

    private GameObject mouse;


    public bool allowInput;
    public GameObject levelButtonPrefab;
    public GameObject deleteIconPrefab;

    public GameObject messagePanel;
    public TextMeshProUGUI message;

    List<string> levels;

    private string activeLevel;


    // Start is called before the first frame update
    void Start()
    {
        mouse = GameObject.Find("Mouse");

        controlsText = GameObject.Find("ControlsText");
        controlsText.SetActive(false);
        mainUI = GameObject.Find("MainUI");
        controls = GameObject.Find("Controls").GetComponent<Button>();
        controls.onClick.AddListener(ToggleControls);

        saveAndLoadUI = GameObject.Find("SaveAndLoadMenu");
        saveMenu = GameObject.Find("SaveMenu");
        loadMenu = GameObject.Find("LoadMenu");
        saveButton = GameObject.Find("SaveButton").GetComponent<Button>();
        loadButton = GameObject.Find("LoadButton").GetComponent<Button>();
        saveAndLoadUI.SetActive(false);
        saveMenu.SetActive(false);
        loadMenu.SetActive(false);

        saveButton.onClick.AddListener(OpenSaveMenu);
        loadButton.onClick.AddListener(OpenLoadMenu);

        messagePanel = GameObject.Find("MessagePanel");
        message = GameObject.Find("Message").GetComponent<TextMeshProUGUI>();
        
        messagePanel.SetActive(false);

        allowInput = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (allowInput)
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                ToggleUI();
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                ToggleControls();
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.S))
        {
            OpenSaveMenu();
        }

        if (Input.GetKeyDown(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.L))
        {
            OpenLoadMenu();
        }


    }

    void ToggleControls()
    {
        controlsText.SetActive(!controlsText.activeSelf);
    }

    void ToggleUI()
    {
        mainUI.SetActive(!mainUI.activeSelf);
    }

    void OpenSaveMenu()
    {
        allowInput = false;
        mainUI.SetActive(false);

        saveAndLoadUI.SetActive(true);
        saveMenu.SetActive(true);
        loadMenu.SetActive(false);

        closeSave = GameObject.Find("CloseSave").GetComponent<Button>();
        closeSave.onClick.AddListener(CloseSaveMenu);

        saveNameInputObject = GameObject.Find("SaveName");
        saveNameInput = saveNameInputObject.GetComponent<InputField>();
        saveNameInput.text = activeLevel;


        saveFileButton = GameObject.Find("SaveFileButton").GetComponent<Button>();
        saveFileButton.onClick.AddListener(SaveLevel);

    }

    void OpenLoadMenu()
    {
        allowInput = false;
        mainUI.SetActive(false);

        saveAndLoadUI.SetActive(true);
        saveMenu.SetActive(false);
        loadMenu.SetActive(true);

        loadPanel = GameObject.Find("LoadPanel");

        closeLoad = GameObject.Find("CloseLoad").GetComponent<Button>();
        closeLoad.onClick.AddListener(CloseLoadMenu);

        levels = GetLevels();

        CreateButtons();
    }

    void SaveLevel()
    {
        string levelName = "Untitled";

        if (saveNameInput.text != "")
        {
            levelName = saveNameInput.text;
        }

        mouse.GetComponent<LevelController>().SaveLevel(levelName);

        CloseSaveMenu();
        
        messagePanel.SetActive(true);
        message.text = "Level " + levelName + " succesvol opgeslagen!";
        StartCoroutine(CloseMessagePanel());
    }

    void CloseSaveMenu()
    {
        allowInput = true;
        mainUI.SetActive(true);

        saveAndLoadUI.SetActive(false);
        saveMenu.SetActive(false);
        loadMenu.SetActive(false);
    }

    void CloseLoadMenu()
    {
        DeleteButtons();

        allowInput = true;
        mainUI.SetActive(true);

        saveAndLoadUI.SetActive(false);
        saveMenu.SetActive(false);
        loadMenu.SetActive(false);
        
    }

    List<string> GetLevels()
    {
        /*string folder = UnityEngine.Application.dataPath + "/Saved/";
        List<string> files = new List<string>(System.IO.Directory.GetFiles(folder, "*.json"));

        for (int i = 0; i < files.Count; i++)
        {
            files[i] = files[i].Replace(folder, "");
            files[i] = files[i].Replace(".json", "");
        }

        return files;*/

        List<string> files =  mouse.GetComponent<EditorDatabase>().GetLevels();

        return files;


    }

    void LoadLevel(string levelName)
    {
        mouse.GetComponent<LevelController>().LoadLevel(levelName);
        CloseLoadMenu();

        messagePanel.SetActive(true);
        message.text = "Level " + levelName + " succesvol geladen!";
        StartCoroutine(CloseMessagePanel());


        OpenSaveMenu();

        saveNameInput.text = levelName;

        CloseSaveMenu();

        activeLevel = levelName;
    }

    void DeleteLevel(string levelName)
    {
        string folder = UnityEngine.Application.dataPath + "/Saved/";
        string file = folder + levelName + ".json";
        System.IO.File.Delete(file);
        System.IO.File.Delete(file + ".meta");
        GameObject levelButton = GameObject.Find(levelName + "Button");
        Destroy(levelButton);
        CloseLoadMenu();
        OpenLoadMenu();

        mouse.GetComponent<EditorDatabase>().DeleteLevel(levelName);

        messagePanel.SetActive(true);
        message.text = "Level " + levelName + " succesvol verwijderd!";
        StartCoroutine(CloseMessagePanel());
    }

    void CreateButtons()
    {

        for (int i = 0; i < levels.Count; i++)
        {
            GameObject levelButton = Instantiate(levelButtonPrefab, loadPanel.transform);
            levelButton.name = levels[i] + "Button";
            if (i < 6)
            {
                levelButton.transform.position = new Vector3(levelButton.transform.position.x - 160, levelButton.transform.position.y + 60, levelButton.transform.position.z);
                levelButton.transform.position = new Vector3(levelButton.transform.position.x, levelButton.transform.position.y - (i * 30), levelButton.transform.position.z);
            }
            else if (i < 12)
            {
                levelButton.transform.position = new Vector3(levelButton.transform.position.x +10, levelButton.transform.position.y + 60, levelButton.transform.position.z);
                levelButton.transform.position = new Vector3(levelButton.transform.position.x, levelButton.transform.position.y - ((i - 6) * 30), levelButton.transform.position.z);

            }
            else
            {
                levelButton.transform.position = new Vector3(levelButton.transform.position.x + 140, levelButton.transform.position.y + 60, levelButton.transform.position.z);
                levelButton.transform.position = new Vector3(levelButton.transform.position.x, levelButton.transform.position.y - ((i - 12) * 30), levelButton.transform.position.z);
            }

            GameObject deleteButton = Instantiate(deleteIconPrefab, levelButton.transform);
            deleteButton.name = "Delete" + levels[i];
            deleteButton.transform.position = new Vector3(levelButton.transform.position.x + 70, levelButton.transform.position.y, levelButton.transform.position.z);

            levelButton.GetComponentInChildren<TextMeshProUGUI>().text = levels[i];
            string name = levels[i];
            levelButton.GetComponent<Button>().onClick.AddListener(() => LoadLevel(name));
            deleteButton.GetComponent<Button>().onClick.AddListener(() => DeleteLevel(name));
        }
    }

    void DeleteButtons()
    {
        for (int i = 0; i < levels.Count; i++)
        {
            GameObject levelButton = GameObject.Find(levels[i] + "Button");
            Destroy(levelButton);
        }
    }

    public IEnumerator CloseMessagePanel()
    {
        yield return new WaitForSeconds(3);
        messagePanel.SetActive(false);
    }

}
