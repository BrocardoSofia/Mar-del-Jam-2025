using UnityEngine;
using UnityEngine.Audio;

public class Key : Interactable
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
        playerInventory.addKey();
        Destroy(gameObject);
    }
}