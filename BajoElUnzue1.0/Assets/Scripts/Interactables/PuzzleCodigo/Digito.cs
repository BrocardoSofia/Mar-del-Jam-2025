using UnityEngine;

public class Digito : MonoBehaviour
{
    public Material[] materials;

    private int digito = 0;

    public void cambiarDigito(int num)
    {
        if(materials.Length != 0)
        {
            Renderer renderer = gameObject.GetComponent<Renderer>();

            if (renderer != null)
            {
                int max = materials.Length - 1;

                if ((digito + num) > max)
                    digito = 0;
                else if ((digito + num) < 0)
                    digito = materials.Length - 1;
                else
                    digito += num;

                renderer.material = materials[digito];
            }
        }
    }

    public int getDigito()
    {
        return digito;
    }
}
