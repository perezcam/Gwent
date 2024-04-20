using System.Diagnostics;
using System.Collections;
using UnityEngine;

public class CardMover: MonoBehaviour
{
    public Transform handContainer; // Contenedor de la mano
    public GameObject cardBack; // El dorso de la carta
    public GameObject cardFront; // El frente de la carta
    DragHandler dragHandler;
    private Vector2 cardScale = new Vector2(0.0244f,0.0255f);

    public void MoveCardToHand(GameObject card)
    {
        dragHandler = card.GetComponent<DragHandler>();
        dragHandler.enabled = false;
       
        cardBack = card.transform.Find("CardBack").gameObject;
        cardFront = card.transform.Find("CardFront").gameObject;
        cardFront.SetActive(false); 

        StartCoroutine(MoveAndRotateCard(card, handContainer.position));
        
    }
    private IEnumerator MoveAndRotateCard(GameObject card, Vector3 targetPosition)
    {
        float halfDuration = 0.5f; // Mitad de la duración de la animación
        float duration = 1f; // Duración total de la rotación y el movimiento
        float elapsed = 0; // Tiempo transcurrido
        card.transform.localScale = cardScale;
       
        Vector3 startPosition = card.transform.position;
        Quaternion startRotation = Quaternion.Euler(0, 180,0); // Inicia sin rotación
        Quaternion midRotation = Quaternion.Euler(0, 270, 0); // Punto medio de la rotación
        Quaternion endRotation = Quaternion.Euler(0,360, 0); // Finaliza rotada 180 grados

        while (elapsed < halfDuration)
        {
            // Mueve y rota a la posición y rotación media
            card.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsed / duration);
            card.transform.rotation = Quaternion.Lerp(startRotation, midRotation, elapsed / halfDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        cardBack.SetActive(true);
        cardFront.SetActive(true);

        while (elapsed < duration)
        {
            // Completa la rotación
            card.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsed / duration);
            card.transform.rotation = Quaternion.Lerp(midRotation, endRotation, (elapsed - halfDuration) / halfDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        card.transform.position = targetPosition; // Asegura que la carta llegue a la posición
        card.transform.rotation = endRotation; // Asegura que la carta esté correctamente orientada
     
        dragHandler.enabled = true;  
        card.transform.SetParent(handContainer); // Configura el padre al contenedor de la mano
        card.transform.localScale = cardScale;
    }
}