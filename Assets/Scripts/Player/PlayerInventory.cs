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

    private int keys = 0;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

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
