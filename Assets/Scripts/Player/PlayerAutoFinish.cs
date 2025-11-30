using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class PlayerAutoFinish : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public PlayerCamera playerCamera;
    public Camera cam;
    public Transform playerTargetPoint;
    public Transform targetPoint;
    public float duration = 1f;
    public Image fadeImage;
    public float durationWalk = 5f;
    public string sceneToLoad;
    public PlayerMusic music;

    void OnTriggerEnter(Collider other)
    {
        playerMovement.enabled = false;
        playerCamera.enabled = false;

        music.reproducirFinalBueno();

        StartCoroutine(RotateToTarget());
        StartCoroutine(MoveToPoint(other.transform));
        StartCoroutine(FadeIn());
    }

    IEnumerator RotateToTarget()
    {
        Quaternion from = cam.transform.rotation;
        Quaternion to = Quaternion.LookRotation(targetPoint.position - cam.transform.position);
        float t = 0f;
        while (t < 1f)
        {
            cam.transform.rotation = Quaternion.Slerp(from, to, t);
            t += Time.deltaTime / duration;
            yield return null;
        }
        cam.transform.rotation = to;
    }

    IEnumerator MoveToPoint(Transform obj)
    {
        Vector3 from = obj.position;
        Vector3 to = playerTargetPoint.position;
        float t = 0f;
        while (t < 1f)
        {
            obj.position = Vector3.Lerp(from, to, t);
            t += Time.deltaTime / durationWalk;
            yield return null;
        }
        obj.position = to;
    }

    IEnumerator FadeIn()
    {
        float t = 0f;
        Color c = fadeImage.color;
        while (t < 1f)
        {
            c.a = Mathf.Lerp(0f, 1f, t);
            fadeImage.color = c;
            t += Time.deltaTime / duration;
            yield return null;
        }
        c.a = 1f;
        fadeImage.color = c;
        SceneManager.LoadScene(sceneToLoad);
    }
}