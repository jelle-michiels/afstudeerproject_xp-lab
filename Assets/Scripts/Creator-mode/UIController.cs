using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEditor;



public class UIController : MonoBehaviour
{
    public GameObject controlsText;
    public GameObject mainUI;
    public GameObject saveAndLoadUI;
    public GameObject saveMenu;
    public GameObject loadMenu;
    public GameObject loadPanel;
    public GameObject saveNameInputObject;
    public GameObject levelButtonPrefab;
    public GameObject deleteIconPrefab;
    public GameObject buttonTemplate;
    public GameObject messagePanel;
    public GameObject mouse;

    public InputField saveNameInput;

    public Button saveFileButton;
    public Button saveButton;
    public Button loadButton;
    public Button closeSave;
    public Button closeLoad;

    public bool allowInput;

    public TextMeshProUGUI message;

    List<string> levels;

    private string activeLevel;

    // Start is called before the first frame update
    void Start()
    {
        controlsText.SetActive(false);
        saveAndLoadUI.SetActive(false);
        saveMenu.SetActive(false); 
        loadMenu.SetActive(false);
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

            if (Input.GetKey(KeyCode.R) && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)))
            {
                Debug.Log("R and Control pressed");
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

    public void OpenSaveMenu()
    {
        allowInput = false;

        mainUI.SetActive(false);
        saveAndLoadUI.SetActive(true);
        saveMenu.SetActive(true);
        loadMenu.SetActive(false);

        saveNameInput.text = activeLevel;
    }

    public void OpenLoadMenu()
    {
        allowInput = false;

        mainUI.SetActive(false);
        saveAndLoadUI.SetActive(true);
        saveMenu.SetActive(false);
        loadMenu.SetActive(true);

        levels = GetLevels();

        CreateButtons();
    }

    public void SaveLevel()
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

    public void CloseSaveMenu()
    {
        allowInput = true;

        mainUI.SetActive(true);
        saveAndLoadUI.SetActive(false);
        saveMenu.SetActive(false);
        loadMenu.SetActive(false);
    }

    public void CloseLoadMenu()
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
        buttonTemplate.SetActive(true);
        foreach (string levelName in levels)
        {
            // Instantiate the ButtonTemplate prefab
            GameObject buttonInstance = Instantiate(buttonTemplate, loadPanel.transform);

            // Set the name of the group (ButtonTemplate) to the level name
            buttonInstance.name = levelName;

            // Find the TextMeshProUGUI component in the group to display the level name
            TextMeshProUGUI levelNameText = buttonInstance.GetComponentInChildren<TextMeshProUGUI>();
            if (levelNameText != null)
            {
                levelNameText.text = levelName;
            }
            else
            {
                Debug.LogError("TextMeshProUGUI component not found in ButtonTemplate.");
                continue; // Skip to the next level if the text component is not found.
            }

            // Find the LevelButton and DeleteButton in the group
            Button levelButton = buttonInstance.transform.Find("LevelButton").GetComponent<Button>();
            Button deleteButton = buttonInstance.transform.Find("DeleteButton").GetComponent<Button>();

            // Add onClick listeners to the LevelButton and DeleteButton
            levelButton.onClick.AddListener(() => LoadLevel(levelName));
            deleteButton.onClick.AddListener(() => DeleteLevel(levelName));
        }

        // Deactivate the template button
        buttonTemplate.SetActive(false);
    }

    void DeleteButtons()
    {
        for (int i = 0; i < levels.Count; i++)
        {
            GameObject levelButton = GameObject.Find(levels[i]);
            Destroy(levelButton);
        }
    }

    public IEnumerator CloseMessagePanel()
    {
        yield return new WaitForSeconds(3);
        messagePanel.SetActive(false);
    }

    public void RefreshButtons()
    {
        // Clear existing buttons
        DeleteButtons();

        // Recreate buttons with the updated list of levels
        CreateButtons();
    }

}
