using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthState : MonoBehaviour
{

    public TextMeshProUGUI healthText;
    private int health;

    void Start()
    {
        health = 3; // Set the initial health value here
    }

    public int Health{
        get { return health; }
        set {
            if (value <= 0)
            {
                health = 3;
            }
            else
            {
                health = value;
            }
        }
    }
    
    public bool damaged(){
        health--;
        if (health > 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    void Update()
    {
        //Debug.Log("Update method called."); // Add this line
        if (healthText != null) // Ensure the reference is not null
        {
            //Debug.Log("X " + health.ToString()); // Add this line
            healthText.text = "X " + health.ToString();
        }
    }


}
