using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIGameOver : MonoBehaviour
{
    public GameObject uiCanvas; // Canvas desactivado al inicio
    public bool pauseTime = true;
    public bool pauseAudio = true;
    public bool showCursor = true;

    CanvasGroup canvasGroup;
    EventSystem eventSystem;

    void Awake()
    {
        if (uiCanvas != null)
            canvasGroup = uiCanvas.GetComponent<CanvasGroup>();

        eventSystem = EventSystem.current;
        if (eventSystem == null)
        {
            // Crea un EventSystem si no existe
            var esGO = new GameObject("EventSystem");
            eventSystem = esGO.AddComponent<EventSystem>();
            esGO.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
        }
    }

    public void ShowUIAndPause()
    {
        if (uiCanvas == null) return;

        uiCanvas.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // Asegurar CanvasGroup para recibir raycasts
        if (canvasGroup == null)
            canvasGroup = uiCanvas.GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
            canvasGroup.alpha = 1f;
        }

        // Asegurar GraphicRaycaster
        var gr = uiCanvas.GetComponent<UnityEngine.UI.GraphicRaycaster>();
        if (gr == null) uiCanvas.AddComponent<UnityEngine.UI.GraphicRaycaster>();

        if (pauseTime) Time.timeScale = 0f;
        if (pauseAudio) AudioListener.pause = true;

    }


}
