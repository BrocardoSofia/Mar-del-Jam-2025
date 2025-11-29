using UnityEngine;
using UnityEngine.Audio;

public class PlayerInventory : MonoBehaviour
{
    public AudioClip sonidoKey;
    public AudioSource audioSource;

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
    }

    public void removeKey() 
    { 
        keys--; 
    }

    public int useKey()
    {
        int key;
        if (keys == 0)
            key = 0;
        else
        {
            key = 1;
            keys--;
        }

        return key;
    }
}
