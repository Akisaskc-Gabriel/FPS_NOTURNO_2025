using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : CharacterBase
{
    public float jumpForce = 5f;
    public float speed = 5f;
    public float runSpeed = 10f;
    public Transform footPosition;
    private PlayerInput playerInput;
    private Rigidbody rb;
    private Vector2 movementInput;
    private Camera mainCamera;
    private Vector2 lookInput;
    public float mouseSensitivity = 1f;
    private float cameraPitch = 0f;
    private bool isGrounded = false;
    private bool isRunning = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        movementInput = playerInput.actions["Move"].ReadValue<Vector2>();
        lookInput = playerInput.actions["Look"].ReadValue<Vector2>();
        isRunning = playerInput.actions["Sprint"].IsPressed();
        RotatePlayer();
        RotateCamera();
        if (playerInput.actions["Jump"].triggered && isGrounded)
        {
            Jump();
        }
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

    void Jump() 
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isGrounded = false;
    }

    private void FixedUpdate()
    {
        isGrounded = Physics.Raycast(footPosition.position, Vector3.down, 0.05f);
        Move();
    }

    void Move() 
    {
        Vector3 cameraFoward = mainCamera.transform.forward;
        cameraFoward.y = 0;
        cameraFoward.Normalize();

        Vector3 cameraRight = mainCamera.transform.right;
        cameraRight.y = 0;
        cameraRight.Normalize();

        Vector3 movementDirection =
            (cameraFoward * movementInput.y + cameraRight * movementInput.x).normalized;

        //IF Tern�rio
        //float currentSpeed = isRunning ? runSpeed : speed;

        float currentSpeed;

        if (isRunning) 
        {
            currentSpeed = runSpeed;
        }else
        {
            currentSpeed = speed;
        }

        Vector3 displacement = movementDirection * currentSpeed * Time.deltaTime;

        rb.MovePosition(rb.transform.position + displacement);
    }

    protected override void Die()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        if (footPosition != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(footPosition.position,
                footPosition.position + Vector3.down * 0.05f);
        }
    }

}
