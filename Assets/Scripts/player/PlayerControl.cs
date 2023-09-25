using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    public Rigidbody rb;
    public GameObject camHolder;
    public float speed;
    public float sensitivity;
    public float maxForce;
    public float jumpForce;
    public bool grounded;
    public float finish;

    public static bool gameIsPaused;

    private Vector2 move;
    private Vector2 look;
    private float lookrotation;

    public void OnMove(InputAction.CallbackContext context)
    {
       move = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
       look = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
       Jump();
    }

    void FixedUpdate()
    {
       Move();
       if (transform.position.y < -finish)
        {
            Debug.Log("You Win");
            gameIsPaused = true;
        }
    }

    void Move()
    {
         //find target velocity
        Vector3 currentVelocity = rb.velocity;
        Vector3 targetVelocity = new Vector3(move.x, 0, move.y);
        targetVelocity *= speed;

        //align direction
        targetVelocity = transform.TransformDirection(targetVelocity);

        //calculate forces
        Vector3 velocityChange = targetVelocity - currentVelocity;
        velocityChange = new Vector3(velocityChange.x, 0, velocityChange.z);

        //limit forces
        Vector3.ClampMagnitude(velocityChange, maxForce);

        rb.AddForce(velocityChange, ForceMode.VelocityChange);
    }

    void Look()
    {
        //turn 
        transform.Rotate(Vector3.up * look.x * sensitivity);

        //look
        lookrotation += -look.y * sensitivity;
        lookrotation = Mathf.Clamp(lookrotation, -90, 90);
        camHolder.transform.eulerAngles = new Vector3(lookrotation, camHolder.transform.eulerAngles.y, camHolder.transform.eulerAngles.z);
    }

    void Jump()
    {
        Vector3 jumpForces = Vector3.zero;

        if(grounded)
        {
            jumpForces = Vector3.up * jumpForce;
        }

        rb.AddForce(jumpForces, ForceMode.VelocityChange);
    }


    // Start is called before the first frame update
    void Start()
    {
        //Mouse lock
        //Cursor.lockState = CursorLockMode.Locked;
        /*gameIsPaused = true;
        PauseGame();*/
        
    }

    void LateUpdate()
    {
        Look();
    }

    public void SetGrounded(bool state)
    {
        grounded = state;
    }

    /* private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("CheckPoint")) 
        {
            Debug.Log("Checkpoint");
            other.gameObject.SetActive(false);
        }
    } */

    void Update()
    {
        PauseGame();
    }

    void PauseGame()
    {
        if(gameIsPaused)
        {
            Time.timeScale = 0f;
        }
        else 
        {
            Time.timeScale = 1;
        }
    }

    public void OnClickCheck()
    {
        gameIsPaused = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "finish")
        {
            //GameObject.Find("ScoreCanvas").GetComponent<CountdownTimer>().gameFinished = true;
            Debug.Log("endpoint reached");
            print("test");
        }
        if (other.gameObject.tag == "checkpoint"){
            
        }
    }
}
