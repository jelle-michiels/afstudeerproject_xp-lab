using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropdownHandler : MonoBehaviour
{
    public Dropdown levelDropdown;

    public string value;

    // Start is called before the first frame update
    void Start()
    {
        List<string> levels = GetComponent<EditorDatabase>().GetLevels();

        foreach (string level in levels)
        {
            levelDropdown.options.Add(new Dropdown.OptionData() { text = level });
        }

        DropdownValueChanged(levelDropdown);

        levelDropdown.RefreshShownValue();

        /*levelDropdown.onValueChanged.AddListener(delegate {
            DropdownValueChanged(levelDropdown);
        });*/
    }

    public void DropdownValueChanged(Dropdown change)
    {
        value = change.options[change.value].text.ToString();
        Debug.Log("Selected: " + value);

        PlayerPrefs.SetString("ActiveLevel", value);
    }

    public string getValue()
    {
        return value;
    }
}
