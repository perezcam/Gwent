using UnityEngine;
using TMPro;
using System.Collections;  // Asegúrate de tener importado TextMeshPro

public class ErrorAnimator : MonoBehaviour
{
    public TextMeshProUGUI errorText;
    private Animator animator;
    private string currentErrorMessage;
    private bool isAnimating;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void ShowError(string message)
    {
        currentErrorMessage = message;
        errorText.text = currentErrorMessage;

        if (!isAnimating)
        {
            isAnimating = true;
            animator.Play("ErrorSlideInOut");
            StartCoroutine(WaitForAnimationEnd());
        }
        else
        {
            animator.Play("ErrorSlideInOut", 0, 0f);  // Reinicia la animación
        }
    }

    private IEnumerator WaitForAnimationEnd()
    {
        yield return new WaitForSeconds(3f);  // Tiempo de la animación (3 segundos en este caso)
        isAnimating = false;
    }
}
