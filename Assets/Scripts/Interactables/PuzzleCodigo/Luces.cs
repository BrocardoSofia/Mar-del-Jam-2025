using UnityEditor;
using UnityEngine;

public class Luces : MonoBehaviour
{
    public DoorConCodigo puerta;
    public Luz[] luces;

    [SerializeField]
    private string codigo = "123456";

    private string codigoActual = "";

    public void abrirContraseña(string numero, Luz luzEncendida)
    {
        codigoActual += numero;

        if (CompareByShortest(codigo, codigoActual))
        {
            if(codigoActual == codigo)
            {
                luzEncendida.encender();
                puerta.abrirPuerta();
                foreach (Luz luz in luces)
                {
                    luz.cierra();
                }
            }
            else
            {
                luzEncendida.encender();
            }
            
        }
        else
        {
            codigoActual = "";
            foreach (Luz luz in luces)
            {
                luz.apagar();
            }
        }
    }

    bool CompareByShortest(string a, string b)
    {
        string shorter = a.Length <= b.Length ? a : b;
        string longer = a.Length > b.Length ? a : b;

        return longer.StartsWith(shorter);
    }
}
