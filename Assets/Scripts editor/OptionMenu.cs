using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OptionMenu : MonoBehaviour
{
    public InputField maxTime;
    public InputField minTime;

    public static string maxTimeText = "300";
    public static string minTimeText;

    private Toggle setActive;

    void Start()
    {
        setActive = GameObject.Find("MakeActiveToggle").GetComponent<Toggle>();
        setActive.onValueChanged.AddListener(delegate { SetActiveLevel(); });
    }

    public void addTimer()
    {
        maxTimeText = maxTime.text;
        minTimeText = minTime.text;

        Debug.Log("Max time: " + maxTime.text);
        Debug.Log("Min time: " + minTime.text);
        string level = GameObject.Find("LevelDropdown").GetComponent<DropdownHandler>().value;
        Debug.Log("Selected: " + level);

        GetComponent<EditorDatabase>().addTimers(int.Parse(maxTime.text), int.Parse(minTime.text), level);
        SceneManager.LoadScene("Menu");
    }

    public void SetActiveLevel()
    {
        if (setActive.isOn)
        {
            string level = GameObject.Find("LevelDropdown").GetComponent<DropdownHandler>().value;
            Debug.Log("Selected: " + level);
            PlayerPrefs.SetString("ActiveLevel", level);
        }
        else if (!setActive.isOn)
        {
            PlayerPrefs.SetString("ActiveLevel", "");
        }

    }

}
