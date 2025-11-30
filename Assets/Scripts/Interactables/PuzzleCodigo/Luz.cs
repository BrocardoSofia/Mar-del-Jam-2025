using UnityEngine;

public class Luz : Interactable
{
    [SerializeField]
    private string codigo = "0";

    [SerializeField]
    private Luces BoardPassword;

    public AudioClip sonidoInteractuar;
    public AudioSource audioSource;

    private string startPrompt;

    [SerializeField] private Renderer targetRenderer;
    [SerializeField] private Color emissionColor = Color.yellow;
    [SerializeField] private float intensity = 3f;

    private Material mat;

    void Start()
    {
        startPrompt = prompMessage;
        mat = targetRenderer.material;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    protected override void Interact()
    {
        if (sonidoInteractuar != null && audioSource != null)
        {
            audioSource.PlayOneShot(sonidoInteractuar);
        }
        BoardPassword.abrirContraseña(codigo, this);
    }

    public void cierra()
    {
        gameObject.layer = LayerMask.NameToLayer("Default");
    }

    public void apagar()
    {
        gameObject.layer = LayerMask.NameToLayer("Interactable");
        mat.SetColor("_EmissionColor", Color.black);
    }

    public void encender()
    {
        gameObject.layer = LayerMask.NameToLayer("Default");
        mat.EnableKeyword("_EMISSION");
        mat.SetColor("_EmissionColor", emissionColor * intensity);
    }
}
