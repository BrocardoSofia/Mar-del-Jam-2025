using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditScript : MonoBehaviour
{
    private Animator animator;
    private bool hasPlayed = false;

    void Start()
    {
        Cursor.visible = true;
        animator = GetComponent<Animator>();
        animator.Play("CreditsAnimation", 0, 0f);
    }

    void Update()
    {
        // Verifica si la animación ha terminado de reproducirse
        if (!animator.GetCurrentAnimatorStateInfo(0).loop &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            if (!hasPlayed)
            {
                hasPlayed = true;
                // Pausa la animación en el último fotograma
                animator.speed = 0f;
            }
        }
    }
    public void OnCreditAnimationEnd()
    {
        SceneManager.LoadScene(0);
    }
}