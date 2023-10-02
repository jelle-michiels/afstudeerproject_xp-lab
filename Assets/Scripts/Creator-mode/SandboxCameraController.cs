using UnityEngine;

public class SandboxCameraController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 2f;
    public float zoomSpeed = 2f;

    private float rotationX;
    private float rotationY;

    private Vector3 lastMousePosition;
    private bool isLeftDragging = false;

    private void Start()
    {
        Vector3 initialRotation = transform.eulerAngles;
        rotationX = initialRotation.y;
        rotationY = initialRotation.x;
    }

    private void Update()
    {
        // Handle camera movement with left-click and drag
        if (Input.GetMouseButtonDown(0)) // Check if left mouse button is pressed
        {
            lastMousePosition = Input.mousePosition;
            isLeftDragging = true;
        }
        else if (Input.GetMouseButtonUp(0)) // Check if left mouse button is released
        {
            isLeftDragging = false;
        }

        if (isLeftDragging)
        {
            Vector3 deltaMousePosition = Input.mousePosition - lastMousePosition;
            lastMousePosition = Input.mousePosition;

            // Calculate camera movement based on mouse drag
            Vector3 moveDirection = new Vector3(deltaMousePosition.x, 0f, deltaMousePosition.y);
            transform.Translate(-moveDirection * moveSpeed * Time.deltaTime);
        }

        // Handle camera rotation, zoom, and reset (the rest of your camera control code) remains unchanged
        if (Input.GetMouseButton(1))
        {
            rotationX += Input.GetAxis("Mouse X") * rotationSpeed;
            rotationY -= Input.GetAxis("Mouse Y") * rotationSpeed;
            rotationY = Mathf.Clamp(rotationY, -90f, 90f); // Limit vertical rotation
            transform.rotation = Quaternion.Euler(rotationY, rotationX, 0f);
        }

        // Handle camera zoom
        float zoomInput = Input.GetAxis("Mouse ScrollWheel");
        transform.Translate(Vector3.forward * zoomInput * zoomSpeed);

        // Reset camera position and rotation
        if (Input.GetKeyDown(KeyCode.Space))
        {
            transform.position = Vector3.zero;
            rotationX = 0f;
            rotationY = 0f;
            transform.rotation = Quaternion.Euler(rotationY, rotationX, 0f);
        }
    }
}
