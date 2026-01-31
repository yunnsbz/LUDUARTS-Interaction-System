using UnityEngine;

/// <summary>
/// CharacterController kullanan, WASD ve mouse ile kontrol edilen bir FPS oyuncu kontrolcüsü.
/// Smooth hareket ve smooth kamera dönüþü içerir.
/// </summary>
public class PlayerController : MonoBehaviour
{
    #region Fields

    [Header("References")]
    [SerializeField] private Transform m_CameraTransform;
    [SerializeField] private CharacterController m_Controller;
    [SerializeField] private PlayerInputActions m_Input;

    [Header("Movement")]
    [SerializeField] private float m_MoveSpeed = 6f;
    [SerializeField] private float m_SmoothTime = 0.1f;
    [SerializeField] private float m_Gravity = -20f;

    [Header("Mouse Look")]
    [SerializeField] private float m_MouseSensitivity = 15f;
    [SerializeField] private float m_LookSmoothTime = 0.05f;
    [SerializeField] private float m_MaxLookAngle = 80f;

    private Vector2 m_MoveInput;
    private Vector2 m_LookInput;

    private Vector2 m_CurrentMoveVelocity;
    private Vector2 m_CurrentLookVelocity;

    private float m_VerticalVelocity;
    private float m_CameraPitch;

    private Vector2 m_SmoothMove;
    private Vector2 m_SmoothLook;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        m_Controller = GetComponent<CharacterController>();
        m_Input = InputActionProvider.Inputs;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnEnable()
    {
        m_Input.Player.Enable();

        m_Input.Player.Move.performed += OnMovePerformed;
        m_Input.Player.Move.canceled += OnMoveCanceled;

        m_Input.Player.Look.performed += OnLookPerformed;
        m_Input.Player.Look.canceled += OnLookCanceled;
    }

    private void OnDisable()
    {
        m_Input.Player.Disable();

        m_Input.Player.Move.performed -= OnMovePerformed;
        m_Input.Player.Move.canceled -= OnMoveCanceled;

        m_Input.Player.Look.performed -= OnLookPerformed;
        m_Input.Player.Look.canceled -= OnLookCanceled;
    }

    private void Update()
    {
        HandleLook();
        HandleMovement();
    }

    #endregion

    #region Input Callbacks

    private void OnMovePerformed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        m_MoveInput = context.ReadValue<Vector2>();
    }

    private void OnMoveCanceled(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        m_MoveInput = Vector2.zero;
    }

    private void OnLookPerformed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        m_LookInput = context.ReadValue<Vector2>();
    }

    private void OnLookCanceled(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        m_LookInput = Vector2.zero;
    }

    #endregion

    #region Movement

    private void HandleMovement()
    {
        m_SmoothMove = Vector2.SmoothDamp(
            m_SmoothMove,
            m_MoveInput,
            ref m_CurrentMoveVelocity,
            m_SmoothTime);

        Vector3 moveDirection =
            transform.right * m_SmoothMove.x +
            transform.forward * m_SmoothMove.y;

        if (m_Controller.isGrounded && m_VerticalVelocity < 0f)
        {
            m_VerticalVelocity = -2f;
        }

        m_VerticalVelocity += m_Gravity * Time.deltaTime;

        Vector3 velocity = moveDirection * m_MoveSpeed;
        velocity.y = m_VerticalVelocity;

        m_Controller.Move(velocity * Time.deltaTime);
    }

    #endregion

    #region Look

    private void HandleLook()
    {
        m_SmoothLook = Vector2.SmoothDamp(
            m_SmoothLook,
            m_LookInput,
            ref m_CurrentLookVelocity,
            m_LookSmoothTime);

        float mouseX = m_SmoothLook.x * m_MouseSensitivity;
        float mouseY = m_SmoothLook.y * m_MouseSensitivity;

        m_CameraPitch -= mouseY;
        m_CameraPitch = Mathf.Clamp(
            m_CameraPitch,
            -m_MaxLookAngle,
            m_MaxLookAngle);

        m_CameraTransform.localRotation =
            Quaternion.Euler(m_CameraPitch, 0f, 0f);

        transform.Rotate(Vector3.up * mouseX);
    }

    #endregion
}
