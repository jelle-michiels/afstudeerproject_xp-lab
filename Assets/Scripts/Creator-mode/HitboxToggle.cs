using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitboxToggle : MonoBehaviour
{
    public RectTransform hitboxToggle;

    public Color backgroundActiveColor;
    Color backgroundDefaultColor;

    public Toggle toggle;

    public Image backgroundImage;

    public GameObject buttonCanvas;

    Vector2 handlePosition;

    void Start()
    {
        handlePosition = hitboxToggle.anchoredPosition;

        backgroundImage = hitboxToggle.parent.GetComponent<Image>();
        backgroundDefaultColor = backgroundImage.color;

        toggle.onValueChanged.AddListener(OnSwitch);

        if (toggle.isOn)
        {
            OnSwitch(true);
        }

    }

    void OnSwitch(bool on)
    {
        hitboxToggle.DOAnchorPos(on ? handlePosition * -1 : handlePosition, .4f).SetEase(Ease.InOutBack);
        backgroundImage.DOColor(on ? backgroundActiveColor : backgroundDefaultColor, .6f);
        /*buttonCanvas.SetActive(on ? false : true);*/
    }

    void OnDestroy()
    {
        toggle.onValueChanged.RemoveListener(OnSwitch);
    }
}
