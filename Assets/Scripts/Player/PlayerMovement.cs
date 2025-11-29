using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Referencias")]
    public InputSystem_Actions inputActions;
    public PlayerRuido ruido;
    private CharacterController controller;
    public Transform playerCamera;

    [Header("Audio Pasos")]
    public AudioSource footstepSource;
    public AudioClip[] walkSteps;
    public AudioClip[] runSteps;
    public AudioClip[] crouchSteps;
    public float walkStepInterval = 0.5f;
    public float runStepInterval = 0.35f;
    public float crouchStepInterval = 0.7f;

    private bool isRunning = false;
    private bool isCrouching = false;
    private Vector3 velocity;
    private bool isGrounded;
    private bool playingSteps = false;

    public float walkPitch = 1f;
    public float runPitch = 1f;
    public float crouchPitch = 0.85f;
    private float currentPitch = 1f;

    // --- campos añadidos mínimos ---
    private float currentInterval = -1f;
    private AudioClip[] currentClips = null;
    private Coroutine footstepCoroutine = null;
    // ------------------------------

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        inputActions = new InputSystem_Actions();
        if (footstepSource == null) footstepSource = gameObject.AddComponent<AudioSource>();
        footstepSource.spatialBlend = 1f;
        footstepSource.playOnAwake = false;
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
        StopFootsteps();
    }

    void Update()
    {
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0f) velocity.y = -2f;

        Vector2 moveInput = inputActions.Player.Move.ReadValue<Vector2>();
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;

        isRunning = inputActions.Player.Sprint.IsPressed();
        isCrouching = inputActions.Player.Crouch.IsPressed();

        float currentSpeed = 0f;
        bool moving = move.magnitude > 0.1f;

        if (isCrouching && moving)
        {
            currentSpeed = crouchStepInterval;
            ruido.dejaDeHacerRuido();
            currentPitch = crouchPitch;
            StartFootstepsIfNeeded(crouchStepInterval, crouchSteps);
        }
        else if (isRunning && moving)
        {
            currentSpeed = runStepInterval;
            ruido.haceRuido();
            currentPitch = runPitch;
            StartFootstepsIfNeeded(runStepInterval, runSteps);
        }
        else if (moving)
        {
            currentSpeed = walkStepInterval;
            ruido.haceRuido(); 
            currentPitch = runPitch;
            StartFootstepsIfNeeded(walkStepInterval, walkSteps);
        }
        else
        {
            ruido.dejaDeHacerRuido();
            currentPitch = crouchPitch;
            StopFootsteps();
        }

        velocity.y += Physics.gravity.y * Time.deltaTime;
        Vector3 totalMove = move * (currentSpeed == 0f ? 0f : (isRunning ? 6f : isCrouching ? 1.5f : 3f)) + new Vector3(0f, velocity.y, 0f);
        controller.Move(totalMove * Time.deltaTime);
    }

    private void StartFootstepsIfNeeded(float interval, AudioClip[] clips)
    {
        // Si no se están reproduciendo pasos, iniciar la coroutine
        if (!playingSteps && isGrounded)
        {
            currentInterval = interval;
            currentClips = clips;
            footstepCoroutine = StartCoroutine(FootstepRoutine(interval, clips));
            return;
        }

        // Si ya se están reproduciendo pero cambió el intervalo o el array, reiniciar limpiamente
        if (playingSteps)
        {
            bool intervalChanged = !Mathf.Approximately(currentInterval, interval);
            bool clipsChanged = currentClips != clips;
            if (intervalChanged || clipsChanged)
            {
                // Detener la coroutine actual si existe
                if (footstepCoroutine != null)
                {
                    StopCoroutine(footstepCoroutine);
                    footstepCoroutine = null;
                }

                // Asegurarse de que no quede audio en reproducción
                if (footstepSource != null)
                {
                    footstepSource.Stop();
                }

                // Actualizar referencias y arrancar la nueva coroutine
                playingSteps = false;
                currentInterval = interval;
                currentClips = clips;
                // arrancar nueva rutina
                footstepCoroutine = StartCoroutine(FootstepRoutine(interval, clips));
            }
        }
    }

    private void StopFootsteps()
    {
        // Detener coroutine y limpiar estado
        if (footstepCoroutine != null)
        {
            StopCoroutine(footstepCoroutine);
            footstepCoroutine = null;
        }
        playingSteps = false;
        currentInterval = -1f;
        currentClips = null;

        if (footstepSource != null)
        {
            footstepSource.Stop();
        }
    }

    private IEnumerator FootstepRoutine(float interval, AudioClip[] clips)
    {
        playingSteps = true;

        // pequeña espera inicial opcional
        yield return null;

        while (playingSteps && controller.isGrounded)
        {
            // Si el jugador dejó de moverse, salir
            if (inputActions.Player.Move.ReadValue<Vector2>().magnitude <= 0.1f)
            {
                playingSteps = false;
                break;
            }

            if (clips != null && clips.Length > 0)
            {
                AudioClip clip = clips[Random.Range(0, clips.Length)];
                if (footstepSource != null)
                {
                    float pitchVar = Random.Range(-0.02f, 0.02f);
                    footstepSource.pitch = Mathf.Clamp(currentPitch + pitchVar, 0.5f, 1.5f);
                    footstepSource.PlayOneShot(clip);
                }
            }

            // Espera respetando el intervalo, pero salta si el jugador deja de moverse o pierde el suelo
            float elapsed = 0f;
            while (elapsed < interval)
            {
                if (!controller.isGrounded || inputActions.Player.Move.ReadValue<Vector2>().magnitude <= 0.1f)
                {
                    playingSteps = false;
                    break;
                }
                elapsed += Time.deltaTime;
                yield return null;
            }
        }

        // limpiar estado al salir
        playingSteps = false;
        footstepCoroutine = null;
    }
}
