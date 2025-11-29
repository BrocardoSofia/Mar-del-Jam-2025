using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Referencias")]
    public InputSystem_Actions inputActions;
    private CharacterController controller;

    [Header("Velocidades")]
    public float walkSpeed = 3f;
    public float runSpeed = 6f;
    public float crouchSpeed = 1.5f;

    [Header("Alturas")]
    public float standHeight = 2f;
    public float crouchHeight = 1f;
    public float crouchTransitionSpeed = 5f;

    [Header("Estados")]
    public bool ruido = false;
    private bool isRunning = false;
    private bool isCrouching = false;

    private Vector3 velocity;
    private bool isGrounded;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        inputActions = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
    }

    private void OnDisable()
    {
        inputActions.Player.Disable();
    }

    void Update()
    {
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        Vector2 moveInput = inputActions.Player.Move.ReadValue<Vector2>();
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;

        isRunning = inputActions.Player.Sprint.IsPressed();
        isCrouching = inputActions.Player.Crouch.IsPressed();

        float currentSpeed = walkSpeed;

        if (isCrouching)
        {
            currentSpeed = crouchSpeed;
            ruido = move.magnitude > 0.1f ? false : false;
        }
        else if (isRunning && move.magnitude > 0.1f)
        {
            currentSpeed = runSpeed;
            ruido = true;
        }
        else if (move.magnitude > 0.1f)
        {
            currentSpeed = walkSpeed;
            ruido = true;
        }
        else
        {
            currentSpeed = 0f;
            ruido = false;
        }

        controller.Move(move * currentSpeed * Time.deltaTime);

        float targetHeight = isCrouching ? crouchHeight : standHeight;
        controller.height = Mathf.Lerp(controller.height, targetHeight, Time.deltaTime * crouchTransitionSpeed);

        velocity.y += Physics.gravity.y * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
