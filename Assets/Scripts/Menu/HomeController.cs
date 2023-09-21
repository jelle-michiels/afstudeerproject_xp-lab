using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomeController : MonoBehaviour
{
    private Button home;

    // Start is called before the first frame update
    void Start()
    {
        GameObject.Find("home").GetComponent<Button>().onClick.AddListener(ToMainMenu);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void ToMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
