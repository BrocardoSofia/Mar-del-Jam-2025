using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoSceneLoader : MonoBehaviour
{
    public string nextSceneName;

    private VideoPlayer videoPlayer;

    void Start()
    {
        MusicManager.Instance.StopMusic();
        videoPlayer = GetComponent<VideoPlayer>();

        if (videoPlayer == null)
        {
            Debug.LogError("No hay VideoPlayer en este objeto");
            return;
        }

        // Suscribirse al evento cuando termina el video
        videoPlayer.loopPointReached += OnVideoFinished;

        // Reproducir
        videoPlayer.Play();
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        SceneManager.LoadScene("1.Level1");
    }
}

