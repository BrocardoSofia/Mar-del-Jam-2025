using UnityEngine;
using UnityEngine.UI;

public class PlayerPiedra : MonoBehaviour
{/*
    [Header("Referencias")]
    public PlayerInventory inventory;          // Inventario del jugador
    public Transform puntoLanzamiento;         // Lugar desde donde se lanza la piedra
    public Text avisoText;                     // Texto de UI para avisar
    public Slider barraFuerza;                 // Barra de fuerza

    [Header("Fuerza de lanzamiento")]
    public float fuerzaMax = 20f;              // Fuerza máxima
    public float velocidadCarga = 10f;         // Qué tan rápido carga la barra
    private float fuerzaActual = 0f;
    private bool cargando = false;

    private GameObject piedraActual;           // Referencia a la piedra en inventario

    void Update()
    {
        // Si tengo piedra en inventario
        piedraActual = inventory.ObtenerPiedra(); // método que devuelve el hijo "Piedra" si existe

        if (piedraActual != null)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                cargando = true;
                fuerzaActual = 0f;

                // al empezar a cargar, inicializo la barra en 0
                if (barraFuerza != null)
                    barraFuerza.value = 0f;
            }

            if (Input.GetKey(KeyCode.Q) && cargando)
            {
                fuerzaActual += velocidadCarga * Time.deltaTime;
                fuerzaActual = Mathf.Clamp(fuerzaActual, 0, fuerzaMax);

                if (barraFuerza != null)
                    barraFuerza.value = fuerzaActual / fuerzaMax;
            }

            if (Input.GetKeyUp(KeyCode.Q) && cargando)
            {
                LanzarPiedra();
                cargando = false;

                // al soltar, vaciar la barra
                if (barraFuerza != null)
                    barraFuerza.value = 0f;
            }
        }
        else
        {
            // Aviso si no tengo piedras
            if (Input.GetKeyDown(KeyCode.Q))
            {
                avisoText.text = "No tienes piedras!";
                Invoke("LimpiarAviso", 2f);
            }
        }
    }

    void LanzarPiedra()
    {
        // Sacar la piedra del inventario (quitar parent)
        piedraActual.transform.SetParent(null);

        // Posicionar en el punto de lanzamiento
        piedraActual.transform.position = puntoLanzamiento.position;
        piedraActual.transform.rotation = puntoLanzamiento.rotation;

        // Activar visualmente
        Renderer rend = piedraActual.GetComponent<Renderer>();
        if (rend != null) rend.enabled = true;

        // Asegurar que tenga Rigidbody
        Rigidbody rb = piedraActual.GetComponent<Rigidbody>();
        if (rb == null) rb = piedraActual.AddComponent<Rigidbody>();

        // Aplicar fuerza
        rb.AddForce(puntoLanzamiento.forward * fuerzaActual, ForceMode.Impulse);

        // Remover del inventario lógico
        inventory.tirarPiedra();
        piedraActual = null;
    }

    void LimpiarAviso()
    {
        avisoText.text = "";
    }*/
}

