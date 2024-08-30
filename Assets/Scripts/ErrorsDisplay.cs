using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ErrorDisplay : MonoBehaviour
{
    public float moveDistance = 180f;
    public float moveSpeed = 80f;
    private Queue<string> errorQueue = new Queue<string>();
    private bool isAnimating = false;
    public TMP_Text errorText;

    public void ShowError(string errorMessage)
    {
        errorQueue.Enqueue(errorMessage);
        if (!isAnimating)
        {
            StartCoroutine(ProcessQueue());
        }
    }

    private IEnumerator ProcessQueue()
    {
        while (errorQueue.Count > 0)
        {
            isAnimating = true;
            string error = errorQueue.Dequeue();
            errorText.text = "";
            errorText.text = error;

            // Desplazar a la izquierda
            yield return StartCoroutine(MoveHorizontal(-moveDistance));
            // Esperar 3 segundos
            yield return new WaitForSeconds(3f);
            // Desplazar a la derecha
            yield return StartCoroutine(MoveHorizontal(moveDistance));
        }
        isAnimating = false;
    }

    private IEnumerator MoveHorizontal(float distance)
    {
        float startPosition = transform.localPosition.x;
        float endPosition = startPosition + distance;
        float elapsedTime = 0f;
        float duration = Mathf.Abs(distance) / moveSpeed;

        while (elapsedTime < duration)
        {
            transform.localPosition = new Vector3(Mathf.Lerp(startPosition, endPosition, elapsedTime / duration), transform.localPosition.y, transform.localPosition.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = new Vector3(endPosition, transform.localPosition.y, transform.localPosition.z);
    }
}
