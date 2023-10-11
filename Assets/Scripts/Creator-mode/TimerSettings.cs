using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UIElements;

public class TimerSettings : MonoBehaviour
{
    public UnityEngine.UI.Slider maxSlider;
    public TextMeshProUGUI maxSliderValue;
    public TMP_InputField sliderInputField;

    public static string maxTimeText = "3600";
    public static string minTimeText = "0";

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void UpdateMaxSlider()
    {
        maxSliderValue.text = maxSlider.value.ToString();
        maxTimeText = maxSlider.value.ToString();
    }

    public void OnSliderChanged()
    {
        sliderInputField.text = maxSlider.value.ToString();
        maxTimeText = maxSlider.value.ToString();
    }

    public void OnFieldChanged()
    {
        if (sliderInputField.text != "")
        {
            maxSlider.value = float.Parse(sliderInputField.text);
            maxTimeText = sliderInputField.text;
        }
    }
}
