using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Splines.ExtrusionShapes;

public class Piedra : Interactable
{
    public PlayerInventory inventory;

    public TextMeshProUGUI promptText;

    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }
    protected override void Interact()
    {
        bool agarrar = inventory.agarrarPiedra();

        if (!agarrar)
            StartCoroutine(MostrarPorTresSegundos("Ya tenes una piedra"));
        else
        {
            gameObject.layer = LayerMask.NameToLayer("Piedra");
            transform.SetParent(inventory.transform);
            if (rend != null)
                rend.enabled = false;

            Collider col = GetComponent<Collider>();
            if (col != null)
                col.enabled = false;
        }

    }

    private IEnumerator MostrarPorTresSegundos(string mensaje)
    {
        promptText.text = mensaje;
        yield return new WaitForSeconds(3f);
        promptText.text = "";
    }
}
