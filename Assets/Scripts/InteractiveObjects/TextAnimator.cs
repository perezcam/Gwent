using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextAnimator : MonoBehaviour
{
    public Text textDisplay;
    public Text winnerDisplay;
    public float typingSpeed = 0.05f;
    public string[] winners;
    public AudioSource keyboardAudio;
    private string[] sentences = 
    {
        "//inicializando sistema de detección de ganador...",
        "//importando módulo de Titanes...",
        "//conectando con el servidor de Trost...",
        "//recuperando datos de los jugadores...",
        "//analizando estrategias de juego...",
        "//calculando puntuaciones de Gwent...",
        "//sincronizando datos con Wall Maria Database...",
        "//desplegando tácticas de juego final...",
        "//identificando jugador con mayor habilidad...",
        "//recopilación final de datos completada.",
        "//preparando para revelar el ganador..."
    };

    public void Awake()
    {
        winners = new string[2];
    }
    public void StartTextAnimation()
    {
        StartCoroutine(TypeAllSentences());
        keyboardAudio.Play();
    }

   IEnumerator TypeAllSentences()
{
    foreach (string sentence in sentences)
    {
        yield return StartCoroutine(TypeSentence(sentence + "\n", Color.green, 16, textDisplay));
    }
    textDisplay.text = "";

    //Verifica si es empate
    if (winners[1] is null)
    {
        yield return StartCoroutine(TypeSentence(winners[0], Color.red, 48, winnerDisplay)); 
        keyboardAudio.Stop();
        gameObject.GetComponent<PlayAgainSystem>().ActivatePlayAgain();
        
    }
    else 
    {
        //muestra los dos nombres en caso de empate
        yield return StartCoroutine(TypeSentence("Error al determinar un único ganador.\n", Color.red, 16, textDisplay));
        yield return StartCoroutine(TypeSentence("Revalidando...\n", Color.green, 16, textDisplay));
        yield return StartCoroutine(TypeSentence("Empate detectado.\n", Color.green, 16, textDisplay));
      
        textDisplay.text = "";  
        foreach (string winner in winners)
        {
            yield return StartCoroutine(TypeSentence(winner + "\n", Color.red, 48, winnerDisplay));
        }
        gameObject.GetComponent<PlayAgainSystem>().ActivatePlayAgain();  
    }
}

    IEnumerator TypeSentence(string sentence, Color color, int fontSize, Text textDisplay)
    {
        textDisplay.color = color;
        textDisplay.fontSize = fontSize;  
        foreach (char letter in sentence)
        {
            textDisplay.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}
