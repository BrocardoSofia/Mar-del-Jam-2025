using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventario : MonoBehaviour
{
    public SFXManager soundMaker;
    public AudioClip sonidoAgarrar;
    public AudioClip sonidoUsarKey;
    public Image llaves;
    public Sprite sinLlaves;
    public Sprite conLlaves;
    public TextMeshProUGUI cantKeysText;
    /*
    public Image piedras;
    public Sprite sinPiedras;
    public Sprite conPiedras;*/

    [SerializeField]
    private int keys = 0;

    public void addKey()
    {
        if (sonidoAgarrar != null)
        {
            soundMaker.playerSoundOnce(sonidoAgarrar);
        }

        keys++;
        llaves.sprite = conLlaves;
        cantKeysText.text = "" + keys;
    }

    public void removeKey()
    {
        keys--;
        cantKeysText.text = "" + keys;
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
                soundMaker.playerSoundOnce(sonidoUsarKey);
            }
        }

        if (keys == 0)
        {
            llaves.sprite = sinLlaves;
        }
        cantKeysText.text = "" + keys;
        return key;
    }
}
