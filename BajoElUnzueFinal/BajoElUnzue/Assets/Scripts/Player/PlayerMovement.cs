using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movimiento")]
    public float walkSpeed = 3f;
    public float runSpeed = 6f;
    public float crouchSpeed = 1.5f;

    [Header("Agacharse")]
    public Transform playerCamera;
    public float crouchCameraOffset = -0.5f;
    public float crouchTransitionSpeed = 8f;

    private CharacterController controller;
    private InputSystem_Actions inputActions;

    private Vector2 moveInput;
    private bool isRunning;
    private bool isCrouching;

    private float currentSpeed;
    private Vector3 cameraDefaultLocalPos;
    private Vector3 cameraCrouchLocalPos;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        inputActions = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        inputActions.Player.Sprint.performed += ctx => isRunning = true;
        inputActions.Player.Sprint.canceled += ctx => isRunning = false;

        inputActions.Player.Crouch.performed += ctx => isCrouching = true;
        inputActions.Player.Crouch.canceled += ctx => isCrouching = false;
    }

    private void OnDisable()
    {
        inputActions.Player.Disable();
    }

    private void Start()
    {
        cameraDefaultLocalPos = playerCamera.localPosition;
        cameraCrouchLocalPos = cameraDefaultLocalPos + new Vector3(0, crouchCameraOffset, 0);
    }

    private void Update()
    {
        if (isCrouching)
            currentSpeed = crouchSpeed;
        else if (isRunning)
            currentSpeed = runSpeed;
        else
            currentSpeed = walkSpeed;

        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        controller.Move(move * currentSpeed * Time.deltaTime);

        Vector3 targetPos = isCrouching ? cameraCrouchLocalPos : cameraDefaultLocalPos;
        playerCamera.localPosition = Vector3.Lerp(
            playerCamera.localPosition,
            targetPos,
            Time.deltaTime * crouchTransitionSpeed
        );
    }
}
