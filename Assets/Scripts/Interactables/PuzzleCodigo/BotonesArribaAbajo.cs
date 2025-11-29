using System;
using UnityEngine;

public class BotonesArribaAbajo : Interactable
{
    public bool sube =  true;
    public Digito digito;

    private string startPrompt;
    private bool puertaAbierta = false;

    void Start()
    {
        startPrompt = prompMessage;
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
            int num = sube ? 1 : -1;
            digito.cambiarDigito(num);
        }
    }
}
