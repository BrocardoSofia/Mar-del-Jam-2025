using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeInImage : MonoBehaviour
{
    public float duration = 2f;

    private Image image;
    private RawImage rawImage;

    void Start()
    {
        image = GetComponent<Image>();
        rawImage = GetComponent<RawImage>();

        if (image != null)
        {
            Color c = image.color;
            c.a = 0f;
            image.color = c;
            StartCoroutine(FadeInImageUI());
        }
        else if (rawImage != null)
        {
            Color c = rawImage.color;
            c.a = 0f;
            rawImage.color = c;
            StartCoroutine(FadeInRawImage());
        }
        else
        {
            Debug.LogError("No hay Image ni RawImage en este objeto.");
        }
    }

    IEnumerator FadeInImageUI()
    {
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            Color c = image.color;
            c.a = Mathf.Lerp(0, 1, timer / duration);
            image.color = c;
            yield return null;
        }
    }

    IEnumerator FadeInRawImage()
    {
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            Color c = rawImage.color;
            c.a = Mathf.Lerp(0, 1, timer / duration);
            rawImage.color = c;
            yield return null;
        }
    }
}
