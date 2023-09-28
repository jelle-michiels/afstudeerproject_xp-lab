using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MouseSensitivityController : MonoBehaviour
{
    public Slider sensitivitySlider;
    public TextMeshProUGUI sensitivityValueText;

    // Make sure to replace "PlayerControl" with the actual name of your player control script.
    private static PlayerControl playerControlScript;

    private void Start()
    {
        // Initialize Slider and Text values
        sensitivitySlider.value = PlayerControl.sensitivity;
        sensitivityValueText.text = sensitivitySlider.value.ToString("F2");
    }

    public void UpdateSensitivity()
    {
        Debug.Log(sensitivitySlider.value);
        PlayerControl.sensitivity = sensitivitySlider.value;
        sensitivityValueText.text = sensitivitySlider.value.ToString("F2");
    }

    // Find and reference the PlayerControl script when this script starts.
    private void Awake()
    {
        playerControlScript = FindObjectOfType<PlayerControl>();
    }
}
