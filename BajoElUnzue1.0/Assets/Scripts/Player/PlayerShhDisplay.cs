using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShhDisplay : MonoBehaviour
{
    public float detectionRadius = 14f;
    public LayerMask enemyLayer;
    public Image alertImage; // asignar en Inspector
    public float blinkDuration = 1.5f;
    public float visibleAlpha = 1f;
    public float hiddenAlpha = 0f;
    private Coroutine blinkCoroutine;
    private Coroutine stopDelayCoroutine;

    void Start()
    {
        if (alertImage != null)
        {
            Color c = alertImage.color;
            c.a = hiddenAlpha;
            alertImage.color = c;
            alertImage.raycastTarget = false;
        }
    }

    void Update()
    {
        bool enemyNearby = Physics.OverlapSphere(transform.position, detectionRadius, enemyLayer).Length > 0;
        if (enemyNearby && blinkCoroutine == null)
        {
            blinkCoroutine = StartCoroutine(BlinkLoop());
            if (stopDelayCoroutine != null) { StopCoroutine(stopDelayCoroutine); stopDelayCoroutine = null; }
        }
        else if (!enemyNearby && blinkCoroutine != null && stopDelayCoroutine == null)
        {
            stopDelayCoroutine = StartCoroutine(StopBlinkWithDelay(1.2f));
        }
    }

    IEnumerator BlinkLoop()
    {
        while (true)
        {
            yield return StartCoroutine(FadeTo(hiddenAlpha, blinkDuration * 0.5f));
            yield return StartCoroutine(FadeTo(visibleAlpha, blinkDuration * 0.5f));
        }
    }

    IEnumerator FadeTo(float targetAlpha, float duration)
    {
        if (alertImage == null) yield break;
        float start = alertImage.color.a;
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float a = Mathf.Lerp(start, targetAlpha, t / duration);
            Color c = alertImage.color;
            c.a = a;
            alertImage.color = c;
            yield return null;
        }
        Color final = alertImage.color;
        final.a = targetAlpha;
        alertImage.color = final;
    }

    IEnumerator StopBlinkWithDelay(float delay)
    {
        float t = 0f;
        while (t < delay)
        {
            if (Physics.OverlapSphere(transform.position, detectionRadius, enemyLayer).Length > 0) yield break;
            t += Time.deltaTime;
            yield return null;
        }
        if (blinkCoroutine != null) { StopCoroutine(blinkCoroutine); blinkCoroutine = null; }
        StartCoroutine(FadeTo(hiddenAlpha, 0.4f));
        stopDelayCoroutine = null;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
