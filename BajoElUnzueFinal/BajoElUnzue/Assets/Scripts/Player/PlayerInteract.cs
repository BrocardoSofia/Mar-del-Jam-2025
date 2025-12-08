using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.LowLevel;

public class PlayerInteract : MonoBehaviour
{
    [Header("Input")]
    public InputActionReference interactActionRef;
    public Camera cam;
    public bool debugMode = false;

    [SerializeField]
    private float distance = 3f;

    [SerializeField]
    private LayerMask mask;
    private PlayerUI playerUI;

    void Start()
    {
        playerUI = GetComponent<PlayerUI>();
    }

    void Update()
    {
        playerUI.UpdateText(string.Empty);

        if (debugMode && cam != null)
        {
            Ray ray = new Ray(cam.transform.position, cam.transform.forward);
            Debug.DrawRay(ray.origin, ray.direction * distance);
        }

        if (cam != null)
        {
            Ray ray = new Ray(cam.transform.position, cam.transform.forward);
            Debug.DrawRay(ray.origin, ray.direction * distance);

            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo, distance, mask))
            {
                if (hitInfo.collider.GetComponent<Interactable>() != null)
                {
                    Interactable interactable = hitInfo.collider.GetComponent<Interactable>();
                    playerUI.UpdateText(interactable.prompMessage);
                }
            }

        }

    }

    void OnEnable()
    {
        if (interactActionRef != null && interactActionRef.action != null)
        {
            interactActionRef.action.performed += OnInteractPerformed;
            interactActionRef.action.Enable();
        }
    }

    void OnDisable()
    {
        if (interactActionRef != null && interactActionRef.action != null)
        {
            interactActionRef.action.performed -= OnInteractPerformed;
            interactActionRef.action.Disable();
        }
    }

    private void OnInteractPerformed(InputAction.CallbackContext ctx)
    {
        if (cam != null)
        {
            Ray ray = new Ray(cam.transform.position, cam.transform.forward);
            Debug.DrawRay(ray.origin, ray.direction * distance);

            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo, distance, mask))
            {
                if (hitInfo.collider.GetComponent<Interactable>() != null)
                {
                    Interactable interactable = hitInfo.collider.GetComponent<Interactable>();

                    if (interactable != null)
                    {
                        interactable.BaseInteract();
                    }

                }
            }

        }
    }
}