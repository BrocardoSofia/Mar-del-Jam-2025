using UnityEngine;
using UnityEngine.Audio;

public class Key : Interactable
{
    [SerializeField]
    private PlayerInventory playerInventory;

    public PlayerRuido ruido;

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
