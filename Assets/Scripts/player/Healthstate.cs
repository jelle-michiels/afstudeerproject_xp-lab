using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthState : MonoBehaviour
{

    private int health = 3;
    public int Health{
        get { return health; }
        set {
            if (value == 0)
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
        if (health > 1)
        {
            health--;
            return false;
        }
        else
        {
            return true;
        }
    }


}
