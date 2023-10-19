using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEditor;

public class TextureUIController : MonoBehaviour
{
    public GameObject mainUI;
    public GameObject saveMenu;
    public GameObject saveNameInputObject;
    public GameObject messagePanel;

    public InputField saveNameInput;

    public Button saveButton;
    public Button closeSave;

    public bool allowInput;

    public TextMeshProUGUI message;

    // Start is called before the first frame update
    void Start()
    {
        saveMenu.SetActive(false);
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

    void ToggleUI()
    {
        mainUI.SetActive(!mainUI.activeSelf);
    }

    public void OpenSaveMenu()
    {
        allowInput = false;
        mainUI.SetActive(false);
        saveMenu.SetActive(true);

        //saveNameInput.text = "FIX IT";
    }

    public void OpenLoadMenu()
    {
        allowInput = false;

        mainUI.SetActive(false);
        saveMenu.SetActive(false);
    }

    public void SaveLevel()
    {
        string levelName = "Untitled";

        if (saveNameInput.text != "")
        {
            levelName = saveNameInput.text;
        }

        CloseSaveMenu();

        messagePanel.SetActive(true);
        message.text = "Level " + levelName + " succesvol opgeslagen!";
        StartCoroutine(CloseMessagePanel());
    }

    public void CloseSaveMenu()
    {
        allowInput = true;

        mainUI.SetActive(true);
        saveMenu.SetActive(false);
    }

    public IEnumerator CloseMessagePanel()
    {
        yield return new WaitForSeconds(3);
        messagePanel.SetActive(false);
    }
}

