using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SettingsMenu : MonoBehaviour
{

    public Slider maxSlider;
    public TextMeshProUGUI maxSliderValue;

    public static string maxTimeText = "3600";
    public static string minTimeText = "0";

    void Start()
    {
        
    }

    public void addTimer()
    {
        Debug.Log("Max time: " + maxTimeText);
        string level = GetComponent<DropdownHandler>().value;
        Debug.Log("Selected: " + level);

        GetComponent<EditorDatabase>().addTimers(int.Parse(maxTimeText), int.Parse(minTimeText), level);
    }

    public void SetActiveLevel()
    {
        string level = GetComponent<DropdownHandler>().value;
        Debug.Log("Selected: " + level);
        PlayerPrefs.SetString("ActiveLevel", level);
    }

    public void UpdateMaxSlider()
    {
        maxSliderValue.text = maxSlider.value.ToString();
        maxTimeText = maxSlider.value.ToString();
    }

}
