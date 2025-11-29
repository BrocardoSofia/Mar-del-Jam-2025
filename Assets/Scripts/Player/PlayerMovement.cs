using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Referencias")]
    public InputSystem_Actions inputActions;
    public PlayerRuido ruido;
    private CharacterController controller;
    public LayerMask enemyLayer;
    public LayerMask finalLayer;
    public Transform playerCamera;
    public float standCameraHeight = 1.7f;
    public float crouchCameraHeight = 1.0f;
    public float cameraTransitionSpeed = 6f;

    [Header("Velocidades")]
    public float walkSpeed = 3f;
    public float runSpeed = 6f;
    public float crouchSpeed = 1.5f;

    [Header("Alturas")]
    public float standHeight = 2f;
    public float crouchHeight = 1f;
    public float crouchTransitionSpeed = 5f;

    [Header("Estados")]
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
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnDisable()
    {
        inputActions.Player.Disable();
    }

    void Update()
    {
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0f)
            velocity.y = -2f;

        Vector2 moveInput = inputActions.Player.Move.ReadValue<Vector2>();
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;

        isRunning = inputActions.Player.Sprint.IsPressed();
        isCrouching = inputActions.Player.Crouch.IsPressed();

        float currentSpeed = walkSpeed;

        if (isCrouching)
        {
            currentSpeed = crouchSpeed;
            ruido.dejaDeHacerRuido();
        }
        else if (isRunning && move.magnitude > 0.1f)
        {
            currentSpeed = runSpeed;
            ruido.haceRuido();
        }
        else if (move.magnitude > 0.1f)
        {
            currentSpeed = walkSpeed;
            ruido.haceRuido();
        }
        else
        {
            currentSpeed = 0f;
            ruido.dejaDeHacerRuido();
        }

        velocity.y += Physics.gravity.y * Time.deltaTime;

        Vector3 totalMove = move * currentSpeed + new Vector3(0f, velocity.y, 0f);
        controller.Move(totalMove * Time.deltaTime);

        if (playerCamera != null)
        {
            float targetCamY = isCrouching ? crouchCameraHeight : standCameraHeight;
            Vector3 camLocalPos = playerCamera.localPosition;
            camLocalPos.y = Mathf.Lerp(camLocalPos.y, targetCamY, Time.deltaTime * cameraTransitionSpeed);
            playerCamera.localPosition = camLocalPos;
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & enemyLayer) != 0)
        {
            ruido.haceRuido();
        }
        else if(((1 << collision.gameObject.layer) & finalLayer) != 0)
        {
            //final
        }
    }

}
