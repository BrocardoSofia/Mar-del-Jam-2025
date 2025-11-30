using UnityEngine;
using UnityEngine.Audio;

public class PlayerMusic : MonoBehaviour
{
    public AudioClip ambiente;
    public AudioClip finalBueno;
    public AudioClip finalMalo;
    public AudioClip llegada;

    public AudioSource audioSource;

    void Start()
    {
        if (llegada != null && audioSource != null)
        {
            audioSource.PlayOneShot(llegada);
        }
        MusicManager.Instance.PlayMusic(ambiente);
    }

    public void reproducirFinalBueno()
    {
        MusicManager.Instance.PlayMusic(finalBueno);
    }

    public void reproducirFinalMalo()
    {
        MusicManager.Instance.PlayMusic(finalMalo);
    }
}
