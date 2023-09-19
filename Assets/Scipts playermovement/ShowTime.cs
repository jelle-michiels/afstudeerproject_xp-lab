using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowTime : MonoBehaviour
{
    new public TextMeshProUGUI gameObject;
    public TMP_InputField timeInput;
    public void updateTime(){
        gameObject.text = timeInput.text;
    }
}
