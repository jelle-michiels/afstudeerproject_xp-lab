using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
public class TextureController : MonoBehaviour
{


    private bool isRightClicking = false;
    private Vector3 lastMousePosition;
    private float cameraRotationSpeed = 2.0f;
    private float cameraPanSpeed = 0.1f;
    private float cameraZoomSpeed = 10.0f;

    public LevelEditor level;

    
    private Button deleteButton;

    private GameObject selected;
    private Vector3 selectedPosition;
    private GameObject selectedObject;

    private GameObject UI;



    void Start()
    {
        UI = GameObject.Find("UI");

        deleteButton = GameObject.Find("Delete").GetComponent<Button>();

        deleteButton.onClick.AddListener(() => { DestroyObject(selectedObject); });

    }


    // Update is called once per frame
    void Update()
    {
        if (UI.GetComponent<UIController>().allowInput)
        {
         
                // Handle camera rotation
                if (Input.GetMouseButtonDown(1))
                {
                    isRightClicking = true;
                    lastMousePosition = Input.mousePosition;
                }
                else if (Input.GetMouseButtonUp(1))
                {
                    isRightClicking = false;
                }

                if (isRightClicking)
                {
                    Vector3 deltaMousePosition = Input.mousePosition - lastMousePosition;
                    transform.Rotate(Vector3.up, deltaMousePosition.x * cameraRotationSpeed, Space.World);
                    transform.Rotate(Vector3.left, deltaMousePosition.y * cameraRotationSpeed);
                    lastMousePosition = Input.mousePosition;
                }

                // Handle camera panning
                float horizontalInput = Input.GetAxis("Horizontal");
                float verticalInput = Input.GetAxis("Vertical");

                if (Mathf.Abs(horizontalInput) > 0 || Mathf.Abs(verticalInput) > 0)
                {
                    Vector3 cameraDirection = new Vector3(horizontalInput, 0, verticalInput).normalized;
                    transform.Translate(cameraDirection * cameraPanSpeed);
                }

                // Handle camera zooming
                float scrollWheelInput = Input.GetAxis("Mouse ScrollWheel");

                if (Mathf.Abs(scrollWheelInput) > 0)
                {
                    float zoomAmount = scrollWheelInput * cameraZoomSpeed;
                    Vector3 newPosition = transform.position + transform.forward * zoomAmount;
                    // Limit the zoom to a minimum and maximum distance if necessary
                    // Example: newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);
                    transform.position = newPosition;
                }


            }
        
        if (deleteButton != null)
        {
            deleteButton.interactable = UI.GetComponent<UIController>().allowInput;
        }

    }

}
