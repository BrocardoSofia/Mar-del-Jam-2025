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

    public TextMeshProUGUI cantPiedrasText;
    public Image piedrasImg;
    public Sprite piedra0;
    public Sprite piedra1;
    public Sprite piedra2;
    public Sprite piedra3;

    [SerializeField]
    private int keys = 0;
    [SerializeField]
    private int piedras = 0;

    private int maxPiedras = 3;

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

    public void addPiedra()
    {
        if(piedras != maxPiedras)
        {
            if (sonidoAgarrar != null)
            {
                soundMaker.playerSoundOnce(sonidoAgarrar);
            }

            piedras++;

            switch (piedras)
            {
                case 0:
                    piedrasImg.sprite = piedra0;
                    break;
                case 1:
                    piedrasImg.sprite = piedra1;
                    break;
                case 2:
                    piedrasImg.sprite = piedra2;
                    break;
                case 3:
                    piedrasImg.sprite = piedra3;
                    break;
            }

            cantPiedrasText.text = "" + piedras;
        }
        
    }

    public bool puedoAgarrarPiedras()
    {
        return (piedras!=maxPiedras);
    }
}
