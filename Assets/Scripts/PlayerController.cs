using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : CharacterBase
{
    [Header("Movimento")]
    public float jumpForce = 5f;
    public float speed = 5f;
    public float runSpeed = 10f;
    public Transform footPosition;

    [Header("Mouse/Camera")]
    public float mouseSensitivity = 1f;

    [Header("Referências")]
    public UIController uiController; // arraste o UIController aqui
    private PlayerInput playerInput;
    private Rigidbody rb;
    private Camera mainCamera;

    [Header("Status")]
    private Vector2 movementInput;
    private Vector2 lookInput;
    private float cameraPitch = 0f;
    private bool isGrounded = false;
    private bool isRunning = false;
    public float score;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (currentHealth <= 0)
        {
            if (uiController != null)
                uiController.GameOver();
            return;
        }

        movementInput = playerInput.actions["Move"].ReadValue<Vector2>();
        lookInput = playerInput.actions["Look"].ReadValue<Vector2>();
        isRunning = playerInput.actions["Sprint"].IsPressed();

        RotatePlayer();
        RotateCamera();

        if (playerInput.actions["Jump"].triggered && isGrounded)
            Jump();
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
        mainCamera.transform.localEulerAngles = new Vector3(cameraPitch, 0f, 0f);
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
        Vector3 cameraForward = mainCamera.transform.forward;
        cameraForward.y = 0;
        cameraForward.Normalize();

        Vector3 cameraRight = mainCamera.transform.right;
        cameraRight.y = 0;
        cameraRight.Normalize();

        Vector3 movementDirection = (cameraForward * movementInput.y + cameraRight * movementInput.x).normalized;

        float currentSpeed = isRunning ? runSpeed : speed;

        Vector3 displacement = movementDirection * currentSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + displacement);
    }

    // =====================================================
    //                     VIDA
    // =====================================================

    public void AddHealth(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth < 0)
            currentHealth = 0;
    }

    public float GetHealthPercentage()
    {
        return currentHealth / maxHealth;
    }

    protected override void Die()
    {
        if (uiController != null)
            uiController.GameOver();
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        if (footPosition != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(footPosition.position, footPosition.position + Vector3.down * 0.05f);
        }
    }
}
