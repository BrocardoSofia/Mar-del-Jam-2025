using UnityEngine;

public class FinalDoor : Interactable
{
    [SerializeField]
    private DoorLogic door;

    [SerializeField]
    private int llavesParaAbrir = 2;

    [SerializeField]
    private PlayerInventory playerInventory;

    [SerializeField]
    private GameObject[] llaves;

    private int cantLlaves = 0;
    private string startPrompt;

    void Start()
    {
        startPrompt = prompMessage;

        foreach (GameObject llave in llaves)
        {
            llave.gameObject.SetActive(false);
        }
    }

    protected override void Interact()
    {
        if (playerInventory.useKey() != 0)
        {
            cantLlaves++;
            llaves[cantLlaves - 1].gameObject.SetActive(true);

            if (cantLlaves == llavesParaAbrir)
            {
                door.abrir();
                gameObject.layer = LayerMask.NameToLayer("Default");
            }
        }
    }
}
