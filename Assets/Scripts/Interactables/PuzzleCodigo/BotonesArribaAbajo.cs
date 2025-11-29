using System;
using UnityEngine;

public class BotonesArribaAbajo : Interactable
{
    public int numActual = 0;
    public Material[] materials;
    public bool sube =  true;
    public GameObject pantallaDigito;

    private string startPrompt;

    void Start()
    {
        startPrompt = prompMessage;
    }

    protected override void Interact()
    {
        if(pantallaDigito != null && materials.Length != 0)
        {
            Renderer renderer = pantallaDigito.GetComponent<Renderer>();

            if (renderer != null)
            {
                int max = materials.Length - 1;
                int num = sube ? 1 : -1;

                if ((numActual + num) > max)
                    numActual = 0;
                else if ((numActual + num) < 0)
                    numActual = materials.Length - 1;
                else
                    numActual += num;

                renderer.material = materials[numActual];
            }
        }
        
    }
}
