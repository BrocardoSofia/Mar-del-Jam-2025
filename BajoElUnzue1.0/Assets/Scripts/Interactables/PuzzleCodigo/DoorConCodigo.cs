using UnityEngine;

public class DoorConCodigo : MonoBehaviour
{
    public DoorLogic door;

    public void abrirPuerta()
    {
        int defaultLayer = LayerMask.NameToLayer("Default");
        CambiarLayerRecursivo(gameObject, defaultLayer);
        door.abrir();
    }

    void CambiarLayerRecursivo(GameObject obj, int layer)
    {
        obj.layer = layer;

        foreach (Transform hijo in obj.transform)
        {
            CambiarLayerRecursivo(hijo.gameObject, layer);
        }
    }
}
