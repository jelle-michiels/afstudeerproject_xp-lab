using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    public Slider maxSlider;
    public Button downArrowButton, upArrowButton;
    public TextMeshProUGUI maxSliderValue, levelTextField;
    public Sprite disabledDownArrowImage, disabledUpArrowImage, enabledDownArrowImage, enabledUpArrowImage;
    public Image downArrowImage, upArrowImage;

    private List<string> levels;
    private int levelIndex;

    void Start()
    {
        levels = GetComponent<EditorDatabase>().GetLevels();

        if (levels.Count == 0)
        {
            levelTextField.text = "No levels found";

            downArrowButton.interactable = false;
            upArrowButton.interactable = false;

            downArrowImage.sprite = disabledDownArrowImage;
            upArrowImage.sprite = disabledUpArrowImage;
        }
        else
        {
            LevelValueChanged(levels[0]);
            levelIndex = 0;
            downArrowImage.sprite = disabledDownArrowImage;
            downArrowButton.interactable = false;
        }
    }


    public void LevelValueChanged(string levelText)
    {
        Debug.Log("Selected: " + levelText);

        levelTextField.text = levelText;

        PlayerPrefs.SetString("ActiveLevel", levelText);

        GetComponent<EditorDatabase>().LoadLevel(levelText);
    }

    public void getNextLevel()
    {
        if (levelIndex < levels.Count - 1)
        {
            levelIndex++;
            LevelValueChanged(levels[levelIndex]);
            downArrowImage.sprite = enabledDownArrowImage;
            downArrowButton.interactable = true;
        }

        if (levelIndex == levels.Count - 1)
        {
            upArrowButton.interactable = false;
            upArrowImage.sprite = disabledUpArrowImage;
        }
    }

    public void getPreviousLevel()
    {
        if (levelIndex > 0)
        {
            levelIndex--;
            LevelValueChanged(levels[levelIndex]);
            upArrowImage.sprite = enabledUpArrowImage;
            upArrowButton.interactable = true;
        }

        if (levelIndex == 0)
        {
            downArrowButton.interactable = false;
            downArrowImage.sprite = disabledDownArrowImage;
        }
    }

}
