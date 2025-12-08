using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public AudioSource playerAudioSource;

    public void playerSoundLoop(AudioClip sound)
    {
        playerAudioSource.Stop();
        playerAudioSource.clip = sound;
        playerAudioSource.Play();
    }

    public void playerStopSoundLoop()
    {
        playerAudioSource.Stop();
    }

    public void playerSoundOnce(AudioClip sound)
    {
        playerAudioSource.PlayOneShot(sound);
    }

    public void otherSoundLoop(AudioSource audioSource, AudioClip sound)
    {
        audioSource.Stop();
        audioSource.clip = sound;
        audioSource.Play();
    }

    public void otherSoundOnce(AudioSource audioSource, AudioClip sound)
    {
        audioSource.PlayOneShot(sound);
    }
}
