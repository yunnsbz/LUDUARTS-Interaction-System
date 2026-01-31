using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    public Transform cameraTransform;
    private CharacterController controller;
    private PlayerInputActions input;

    [Header("Movement")]
    public float moveSpeed = 6f;
    public float smoothTime = 0.1f;
    public float gravity = -20f;

    [Header("Mouse Look")]
    public float mouseSensitivity = 15f;
    public float lookSmoothTime = 0.05f;
    public float maxLookAngle = 80f;

    Vector2 moveInput;
    Vector2 lookInput;

    Vector2 currentMoveVelocity;
    Vector2 currentLookVelocity;

    float verticalVelocity;
    float cameraPitch;

    Vector2 smoothMove;
    Vector2 smoothLook;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        input = new PlayerInputActions();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void OnEnable()
    {
        input.Player.Enable();

        input.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        input.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        input.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        input.Player.Look.canceled += ctx => lookInput = Vector2.zero;
    }

    void OnDisable()
    {
        input.Player.Disable();
    }

    void Update()
    {
        HandleLook();
        HandleMovement();
    }

    void HandleMovement()
    {
        smoothMove = Vector2.SmoothDamp(
            smoothMove,
            moveInput,
            ref currentMoveVelocity,
            smoothTime
        );

        Vector3 move = transform.right * smoothMove.x +
                       transform.forward * smoothMove.y;

        if (controller.isGrounded && verticalVelocity < 0)
            verticalVelocity = -2f;

        verticalVelocity += gravity * Time.deltaTime;

        Vector3 velocity = move * moveSpeed;
        velocity.y = verticalVelocity;

        controller.Move(velocity * Time.deltaTime);
    }

    void HandleLook()
    {
        smoothLook = Vector2.SmoothDamp(
            smoothLook,
            lookInput,
            ref currentLookVelocity,
            lookSmoothTime
        );

        float mouseX = smoothLook.x * mouseSensitivity;
        float mouseY = smoothLook.y * mouseSensitivity;

        cameraPitch -= mouseY;
        cameraPitch = Mathf.Clamp(cameraPitch, -maxLookAngle, maxLookAngle);

        cameraTransform.localRotation = Quaternion.Euler(cameraPitch, 0, 0);
        transform.Rotate(Vector3.up * mouseX);
    }
}
