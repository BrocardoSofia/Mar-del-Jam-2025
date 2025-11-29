using UnityEngine;
using UnityEngine.Rendering;

public class PlayerRuido : MonoBehaviour
{
    public bool ruido = false;

    public void haceRuido()
    {
        ruido = true;
    }

    public void dejaDeHacerRuido()
    {
        ruido = false;
    }
}
