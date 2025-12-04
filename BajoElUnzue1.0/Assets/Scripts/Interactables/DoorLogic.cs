using System;
using System.Collections;
using UnityEngine;

public class DoorLogic : MonoBehaviour
{
    public float openAngle = 90f;
    public float openSpeed = 2f;
    public bool isOpen = false;
    public AudioClip sonidoInteractuar;
    public AudioSource audioSource;

    private Quaternion closedRotation;
    private Quaternion openRotation;
    private Coroutine currentCoroutine;

    private void Start()
    {
        closedRotation = transform.rotation;
        openRotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, openAngle, 0));

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }
    public void abrir()
    {
        if (sonidoInteractuar != null && audioSource != null)
        {
            audioSource.PlayOneShot(sonidoInteractuar);
        }
        if (currentCoroutine != null) 
            StopCoroutine(currentCoroutine);

        currentCoroutine = StartCoroutine(moverPuerta());
    }

    private IEnumerator moverPuerta()
    {
        Quaternion targetRotation = isOpen ? closedRotation : openRotation;
        isOpen = !isOpen;

        while(Quaternion.Angle(transform.rotation, targetRotation) > 0.01f)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * openSpeed);
            yield return null;
        }

        transform.rotation = targetRotation;
    }

}
