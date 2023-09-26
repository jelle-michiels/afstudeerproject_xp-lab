using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public GameObject wallText;

    // Start is called before the first frame update
    void Start()
    {
        wallText.SetActive(false);
    }

    public void OnMouseOver()
    {
        wallText.SetActive(true);
    }

    public void OnMouseExit()
    {
        wallText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
