using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ControlToggle : MonoBehaviour
{
    public RectTransform controlToggle;

    public Color backgroundActiveColor;
    Color backgroundDefaultColor;

    public Toggle toggle;

    public Image backgroundImage;

    public GameObject controlMenu;

    Vector2 handlePosition;

    void Start()
    {
        handlePosition = controlToggle.anchoredPosition;

        backgroundImage = controlToggle.parent.GetComponent<Image>();
        backgroundDefaultColor = backgroundImage.color;

        toggle.onValueChanged.AddListener(OnSwitch);

        if (toggle.isOn)
        {
            OnSwitch(true);

        }

    }

    void OnSwitch(bool on)
    {
        controlToggle.DOAnchorPos(on ? handlePosition * -1 : handlePosition, .4f).SetEase(Ease.InOutBack);
        backgroundImage.DOColor(on ? backgroundActiveColor : backgroundDefaultColor, .6f);
    }

    void OnDestroy()
    {
        toggle.onValueChanged.RemoveListener(OnSwitch);
    }

    public void ToggleControlMenu()
    {
        controlMenu.SetActive(!controlMenu.activeSelf);
    }
}
