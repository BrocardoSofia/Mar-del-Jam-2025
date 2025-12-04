using UnityEngine;

public class MenuMusic : MonoBehaviour
{
    public AudioClip menuMusic;

    void Start()
    {
        MusicManager.Instance.PlayMusic(menuMusic);
    }

}
