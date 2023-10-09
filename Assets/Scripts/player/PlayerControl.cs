using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    public Rigidbody rb;
    public GameObject camHolder;
    public GameObject rdPersonCam;

    public GameObject stPersonCam;
    public float speed;
    public static float sensitivity = 0.5f;
    public float maxForce;
    public float jumpForce;
    public bool grounded;
    public float finish;

    public GameObject winScreen;

    public GameObject loseScreen;

    public GameObject checkpoint;

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

    void Look3rd(){
        // Get the horizontal and vertical input for camera movement
    //float horizontalInput = Input.GetAxis("HorizontalLook");  // Horizontal input for orbiting horizontally
    //float verticalInput = Input.GetAxis("VerticalLook");      // Vertical input for orbiting vertically

    // Rotate the camera horizontally around the player
    transform.Rotate(Vector3.up * look.x * sensitivity);

    // Calculate the new vertical rotation
    lookrotation += -look.y * sensitivity;
    lookrotation = Mathf.Clamp(lookrotation, -90, 90);

    // Rotate the camera holder (the object holding the camera) vertically around the player
    camHolder.transform.localEulerAngles = new Vector3(lookrotation, 0, 0);
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
        
        this.camHolder = stPersonCam;
        //Mouse lock
        //Cursor.lockState = CursorLockMode.Locked;
        /*gameIsPaused = true;
        PauseGame();*/
        
    }

    void LateUpdate()
    {
        /*if (stPersonCam.activeSelf == true)
        {
            Look();
        }
        else
        {
            Look3rd();
        }
        //Look();*/
        Look();
    }

 /*   void FixedUpdate()
    {
        Move();

        // Check if the player is grounded.
        CheckGrounded();

        if (transform.position.y < -finish)
        {
            Debug.Log("You Win");
            gameIsPaused = true;
        }
    }*/


    void CheckGrounded()
    {
        // Perform a raycast to check if the player is grounded.
        grounded = Physics.Raycast(transform.position, Vector3.down, 0.1f); // Adjust the distance as needed.
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
        if (Input.GetKeyDown(KeyCode.A))
        {
            changeCam();
        }
    }

    void changeCam(){
        Debug.Log("changeCam");
        if (rdPersonCam.activeSelf == true)
        {
            this.camHolder = stPersonCam;
            stPersonCam.SetActive(true);
            rdPersonCam.SetActive(false);
        }
        else
        {
            this.camHolder = rdPersonCam;
            stPersonCam.SetActive(false);
            rdPersonCam.SetActive(true);
        }
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
            Debug.Log("endpoint reached");
            GameObject.Find("TimerCanvas").GetComponent<CountdownTimer>().EndGame(true);
            //LevelState.endPointReached(winScreen);
            
            print("test");
        }
        if (other.gameObject.tag == "CheckPoint"){
            other.gameObject.SetActive(false);
            GameObject.Find("TimerCanvas").GetComponent<CountdownTimer>().checkpointReached();
        }
    }

    private IEnumerator checkpointDelay()
    {
        yield return new WaitForSeconds(0.5f);
        LevelState.disableCheckpointscreen(checkpoint);
    }

}
