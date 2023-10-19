using UnityEngine;
using UnityEngine.UI;

public class DoorController : MonoBehaviour
{
    /* public bool keyNeeded = false;              //Is key needed for the door
    public bool gotKey;                  //Has the player acquired key
    public GameObject keyGameObject;  */           //If player has Key,  assign it here
    private GameObject iconToDisplay;
    private GameObject controllerToDisplay;             //Display the information about how to close/open the door

    private bool playerInZone;                  //Check if the player is in the zone
    private bool doorOpened;                    //Check if door is currently opened or not

    private Animation doorAnim;
    private BoxCollider doorCollider; //To enable the player to go through the door if door is opened else block him

    private string doorType;

    enum DoorState
    {
        Closed,
        Opened,
        Jammed
    }

    DoorState doorState = new DoorState();      //To check the current state of the door

    /// <summary>
    /// Initial State of every variables
    /// </summary>
    private void Start()
    {
        /* gotKey = false; */
        doorOpened = false;                     //Is the door currently opened
        playerInZone = false;                   //Player not in zone
        doorState = DoorState.Closed;           //Starting state is door closed


        GameObject canvasObject = GameObject.Find("SwitchCanvas");
        if (canvasObject != null)
        {
            iconToDisplay = canvasObject.transform.Find("KeyboardE").gameObject;
            controllerToDisplay = canvasObject.transform.Find("ControllerIcon").gameObject;
        }
        iconToDisplay.gameObject.SetActive(false);
        controllerToDisplay.gameObject.SetActive(false);

        doorAnim = transform.parent.gameObject.GetComponent<Animation>();
        doorCollider = transform.parent.gameObject.GetComponent<BoxCollider>();

        doorType = gameObject.transform.parent.gameObject.tag;

        //If Key is needed and the KeyGameObject is not assigned, stop playing and throw error
        /* if (keyNeeded && keyGameObject == null)
        {
            //UnityEditor.EditorApplication.isPlaying = false;
            Debug.LogError("Assign Key GameObject");
        } */
    }

    private void OnTriggerEnter(Collider other)
    {
        iconToDisplay.SetActive(true);
        controllerToDisplay.SetActive(true);
        playerInZone = true;
    }

    private void OnTriggerExit(Collider other)
    {
        playerInZone = false;
        iconToDisplay.SetActive(false);
        controllerToDisplay.SetActive(false);
    }

    private void Update()
    {
        //To Check if the player is in the zone
        if (doorState == DoorState.Opened)
        {
            if (doorState == DoorState.Opened)
            {
                //txtToDisplay.GetComponent<Text>().text = "Press 'E' to Close";
                doorCollider.enabled = false;
            }
            else if (doorState == DoorState.Closed/*  || gotKey */)
            {
                //txtToDisplay.GetComponent<Text>().text = "Press 'E' to Open";
                doorCollider.enabled = true;
            }
            /* else if (doorState == DoorState.Jammed)
            {
                txtToDisplay.GetComponent<Text>().text = "Needs Key";
                doorCollider.enabled = true;
            } */
        }

        if ( Input.GetKeyDown(KeyCode.E) && playerInZone || Input.GetKeyDown(KeyCode.JoystickButton2) && playerInZone)
        {
            if (doorType == "WrongDoor")
            {
                GameObject.Find("TimerCanvas").GetComponent<CountdownTimer>().EndGame(false);
            }
            
            doorOpened = !doorOpened;           //The toggle function of door to open/close

            if (doorState == DoorState.Closed && !doorAnim.isPlaying)
            {
                /* if (!keyNeeded)
                { */
                    doorAnim.Play("Door_Open");
                    doorState = DoorState.Opened;
               /*  }
                else if (keyNeeded && !gotKey)
                {
                    if (doorAnim.GetClip("Door_Jam") != null)
                        doorAnim.Play("Door_Jam");
                    doorState = DoorState.Jammed;
                } */
            }

            if (doorState == DoorState.Closed && !doorAnim.isPlaying)
            {
                doorAnim.Play("Door_Open");
                doorState = DoorState.Opened;
            }

            if (doorState == DoorState.Opened && !doorAnim.isPlaying)
            {
                doorAnim.Play("Door_Close");
                doorState = DoorState.Closed;
            }


            /* if (doorState == DoorState.Jammed && !gotKey)
            {
                if (doorAnim.GetClip("Door_Jam") != null)
                    doorAnim.Play("Door_Jam");
                doorState = DoorState.Jammed;
            }
            else if (doorState == DoorState.Jammed && gotKey && !doorAnim.isPlaying)
            {
                doorAnim.Play("Door_Open");
                doorState = DoorState.Opened;
            } */
        }

        if (doorState == DoorState.Opened && !playerInZone)
        {
            doorAnim.Play("Door_Close");
            doorState = DoorState.Closed;
        }
    }
}