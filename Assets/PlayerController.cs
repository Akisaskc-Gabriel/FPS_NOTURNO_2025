using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    private PlayerInput playerInput;
    private Rigidbody rb;
    private Vector2 movementInput;
    private Camera mainCamera;
    private Vector2 lookInput;
    public float mouseSensitivity = 1f;
    private float cameraPitch = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        movementInput = playerInput.actions["Move"].ReadValue<Vector2>();
        lookInput = playerInput.actions["Look"].ReadValue<Vector2>();
        RotatePlayer();
        RotateCamera();
    }

    void RotatePlayer() 
    {
        float mouseX = lookInput.x * mouseSensitivity * Time.deltaTime;
        transform.Rotate(Vector3.up * mouseX);
    }

    void RotateCamera() 
    {
        cameraPitch -= lookInput.y * mouseSensitivity * Time.deltaTime;
        
        cameraPitch = Mathf.Clamp(cameraPitch, -80f, 80f);

        mainCamera.transform.localEulerAngles = 
            new Vector3(cameraPitch, 0f, 0f);
    }

    private void FixedUpdate()
    {
        Move();
    }

    void Move() 
    {
        Vector3 movementDirection = new Vector3
            (movementInput.x, 0, movementInput.y);

        Vector3 displacement = movementDirection * speed * Time.deltaTime;

        rb.MovePosition(rb.transform.position + displacement);
    }
}
