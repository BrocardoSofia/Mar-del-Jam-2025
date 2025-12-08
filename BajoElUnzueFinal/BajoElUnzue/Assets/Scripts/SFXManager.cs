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

    public void playerSoundLoopRandomStart(AudioClip sound)
    {
        playerAudioSource.Stop();
        playerAudioSource.clip = sound;
        playerAudioSource.loop = true;

        // Intentar posicionar por samples (más preciso). Si no hay samples, usar time.
        if (sound.samples > 0)
        {
            playerAudioSource.timeSamples = Random.Range(0, sound.samples);
        }
        else if (sound.length > 0f)
        {
            playerAudioSource.time = Random.Range(0f, sound.length);
        }

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
