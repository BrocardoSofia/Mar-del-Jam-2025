using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    public AudioClip sonidoKey; 
    public AudioClip sonidoUsarKey;
    public AudioSource audioSource;
    public Image llaves;
    public Sprite sinLlaves;
    public Sprite conLlaves;
    public TextMeshProUGUI promptText;

    public Image piedras;
    public Sprite sinPiedras;
    public Sprite conPiedras;

    private int keys = 0;
    private bool piedra = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

    }

    public bool agarrarPiedra()
    {
        if(piedra)
            return false;
        else
        {
            if (sonidoKey != null)
            {
                AudioSource.PlayClipAtPoint(sonidoKey, Camera.main.transform.position, 1f);
            }
            piedras.sprite = conPiedras;
            piedra = true;
            return true;
        }
    }

    public GameObject ObtenerPiedra()
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.layer == LayerMask.NameToLayer("Piedra"))
            {
                return child.gameObject;
            }
        }
        return null;
    }

    public bool tirarPiedra()
    {
        bool tiro = piedra;

        if (tiro)
        {
            piedras.sprite = sinPiedras;
            piedra = !piedra;
        }

       return tiro;
    }

    public bool hayPiedra()
    {
        return piedra;
    }

    public void addKey()
    {
        if (sonidoKey != null)
        {
            AudioSource.PlayClipAtPoint(sonidoKey, Camera.main.transform.position, 1f);
        }
        
        keys++;
        llaves.sprite = conLlaves;
        promptText.text = ""+keys;
    }

    public void removeKey() 
    { 
        keys--;
        promptText.text = "" + keys;
    }

    public int useKey()
    {
        int key;
        if (keys == 0)
        {
            key = 0;
        }
        else
        {
            key = 1;
            keys--;
            if (sonidoUsarKey != null)
            {
                AudioSource.PlayClipAtPoint(sonidoUsarKey, Camera.main.transform.position, 1f);
            }
        }

        if (keys == 0)
        {
            llaves.sprite = sinLlaves;
        }
        promptText.text = "" + keys;
        return key;
    }
}
