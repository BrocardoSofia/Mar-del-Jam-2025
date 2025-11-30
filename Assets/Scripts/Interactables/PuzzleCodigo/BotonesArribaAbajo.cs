using System;
using UnityEngine;

public class BotonesArribaAbajo : Interactable
{
    public bool sube =  true;
    public Digito digito;
    public BoardPassword board;
    public AudioClip sonidoInteractuar;
    public AudioSource audioSource;

    //public PlayerRuido ruido;

    private string startPrompt;
    private bool puertaAbierta = false;

    void Start()
    {
        startPrompt = prompMessage;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

    }

    public void cerrarPuerta()
    {
        puertaAbierta = true;
        gameObject.layer = LayerMask.NameToLayer("Default");
    }

    protected override void Interact()
    {
        if(!puertaAbierta)
        {
            gameObject.layer = LayerMask.NameToLayer("Interactable");
            //ruido.haceRuido(1, transform.position);
            int num = sube ? 1 : -1;

            if (sonidoInteractuar != null && audioSource != null)
            {
                audioSource.PlayOneShot(sonidoInteractuar);
            }

            digito.cambiarDigito(num);
            board.abrirContraseña();
        }
    }

}
