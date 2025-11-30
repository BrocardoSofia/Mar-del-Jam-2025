using UnityEngine;

public class SoundButton : MonoBehaviour
{
    public AudioClip llegada;

    public AudioSource audioSource;

    public void sound()
    {
        audioSource.PlayOneShot(llegada);
    }
}
