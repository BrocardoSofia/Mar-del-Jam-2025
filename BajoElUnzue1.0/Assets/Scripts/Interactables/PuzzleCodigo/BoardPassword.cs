using UnityEngine;

public class BoardPassword : MonoBehaviour
{
    public Digito[] digitos;
    public DoorConCodigo puerta;

    [SerializeField]
    private int codigo = 37102;

    public void abrirContraseña()
    {
        int actual = digitosAInt();

        if (actual == codigo)
        {
            puerta.abrirPuerta();
        }
    }

    int digitosAInt()
    {
        int resultado = 0;
        foreach (Digito d in digitos)
        {
            resultado = resultado * 10 + d.getDigito();
        }
        return resultado;
    }
}
