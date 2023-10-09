using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class TimerSettings : MonoBehaviour
{
    public Slider maxSlider;
    public TextMeshProUGUI maxSliderValue;
    public Text inputText;

    public static string maxTimeText = "3600";
    public static string minTimeText = "0";

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void addTimer()
    {
        Debug.Log("Max time: " + maxTimeText);
        string level = inputText.text;
        Debug.Log("Selected: " + level);

        GetComponent<EditorDatabase>().addTimers(int.Parse(maxTimeText), int.Parse(minTimeText), level);
    }

    public void UpdateMaxSlider()
    {
        maxSliderValue.text = maxSlider.value.ToString();
        maxTimeText = maxSlider.value.ToString();
    }
}
