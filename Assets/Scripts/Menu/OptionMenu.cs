using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class OptionMenu : MonoBehaviour
{
/*    public InputField maxTime;
    public InputField minTime;*/

    public Slider minSlider, maxSlider;
    public TextMeshProUGUI minSliderValue, maxSliderValue;

    public static string maxTimeText = "3600";
    public static string minTimeText = "0";

/*    public Toggle setActive;*/

    void Start()
    {
        /*setActive = GameObject.Find("MakeActiveToggle").GetComponent<Toggle>();
        setActive.onValueChanged.AddListener(delegate { SetActiveLevel(); });*/
    }

    public void addTimer()
    {
/*        maxTimeText = maxTime.text;
        minTimeText = minTime.text;*/

        Debug.Log("Max time: " + maxTimeText);
        Debug.Log("Min time: " + minTimeText);
        string level = GetComponent<DropdownHandler>().value;
        Debug.Log("Selected: " + level);

        GetComponent<EditorDatabase>().addTimers(int.Parse(maxTimeText), int.Parse(minTimeText), level);
        /*SceneManager.LoadScene("Menu");*/
    }

    public void SetActiveLevel()
    {
        /*if (setActive.isOn)
        {*/
            string level = GetComponent<DropdownHandler>().value;
            Debug.Log("Selected: " + level);
            PlayerPrefs.SetString("ActiveLevel", level);
        /*}
        else if (!setActive.isOn)
        {
            PlayerPrefs.SetString("ActiveLevel", "");
        }*/

    }

    public void UpdateMinSlider()
    {
        minSliderValue.text = minSlider.value.ToString();
        minTimeText = minSlider.value.ToString();
    }

    public void UpdateMaxSlider()
    {
        maxSliderValue.text = maxSlider.value.ToString();
        maxTimeText = maxSlider.value.ToString();
    }

}
