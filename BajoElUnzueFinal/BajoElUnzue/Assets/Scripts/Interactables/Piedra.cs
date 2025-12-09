using UnityEngine;

public class Piedra : Interactable
{
    [SerializeField]
    private PlayerInventario playerInventory;
    private string startPrompt;

    void Start()
    {
        startPrompt = prompMessage;
    }

    protected override void Interact()
    {
        if(playerInventory.puedoAgarrarPiedras())
        {
            playerInventory.addPiedra();
            Destroy(gameObject);
        }
        else
        {

        }
    }
}