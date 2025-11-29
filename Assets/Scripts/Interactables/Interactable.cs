using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [SerializeField]
    public string prompMessage;

    public virtual string OnLook()
    {
        return prompMessage;
    }

    public void BaseInteract()
    {
        Interact();
    }

    protected virtual void Interact()
    {

    }
}