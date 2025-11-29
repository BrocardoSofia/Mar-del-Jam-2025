using UnityEngine;

public class Key : Interactable
{
    [SerializeField]
    private PlayerInventory playerInventory;

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
