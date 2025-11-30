using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [Header("Settings")]
    [Range(0f, 1f)] public float volume = 0.5f;
    public bool loop = true;

    public AudioSource audioSource;

    void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Start()
    {

    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
    }

    void AssignCameraAudioSource()
    {
        Camera cam = Camera.main;

        if (cam == null)
        {
            Debug.LogWarning("No hay una cámara con el tag MainCamera.");
            return;
        }

        audioSource = cam.GetComponent<AudioSource>();

        if (audioSource == null)
            audioSource = cam.gameObject.AddComponent<AudioSource>();

        audioSource.loop = loop;
        audioSource.volume = volume;
        audioSource.playOnAwake = false;
    }

    // ================== FUNCIONES PUBLICAS ==================

    public void PlayMusic(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogWarning("El clip es null.");
            return;
        }

        if (audioSource == null)
            AssignCameraAudioSource();

        if (audioSource.clip == clip && audioSource.isPlaying)
            return;

        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.Play();
    }

    public void StopMusic()
    {
        if (audioSource != null)
            audioSource.Stop();
    }

    public void PauseMusic()
    {
        if (audioSource != null && audioSource.isPlaying)
            audioSource.Pause();
    }

    public void ResumeMusic()
    {
        if (audioSource != null && audioSource.clip != null)
            audioSource.UnPause();
    }

    public void SetVolume(float newVolume)
    {
        volume = Mathf.Clamp01(newVolume);

        if (audioSource != null)
            audioSource.volume = volume;
    }

    public AudioClip GetCurrentClip()
    {
        return audioSource != null ? audioSource.clip : null;
    }

    public bool IsPlaying()
    {
        return audioSource != null && audioSource.isPlaying;
    }
}
