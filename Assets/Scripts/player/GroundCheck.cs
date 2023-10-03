using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GroundCheck : MonoBehaviour
{
    public PlayerControl player;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject != player.gameObject)
        {
           /* player.SetGrounded(true);*/
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject != player.gameObject)
        {
            /*player.SetGrounded(false);*/
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject != player.gameObject)
        {
          /*  player.SetGrounded(true);*/
        }
    }
}
