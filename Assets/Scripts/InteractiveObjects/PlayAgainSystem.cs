using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.UIElements; // Importar para manejar eventos de puntero

public class PlayAgainSystem : MonoBehaviour
{
    public GameObject playAgainPanel;
    public GameObject yesIndicator;
    public GameObject noIndicator;
    public UnityEngine.UI.Button yesButton;
    public UnityEngine.UI.Button noButton;
    public GameObject WinImage;
    public AudioSource GameOverSound;
    public GameObject GameOverImage;

    void Start()
    {
        playAgainPanel.SetActive(false);
        yesIndicator.SetActive(false);
        noIndicator.SetActive(false);
        yesButton.gameObject.SetActive(false);
        noButton.gameObject.SetActive(false);
        WinImage.SetActive(false);
        GameOverImage.SetActive(false);
    }


    public void ActivatePlayAgain()
    {
        WinImage.SetActive(true);
        playAgainPanel.SetActive(true);
        yesButton.gameObject.SetActive(true);
        noButton.gameObject.SetActive(true);
        GameOverSound.Play();
        GameOverImage.SetActive(true);
    }
    //Reiniciar juego
    public void OnClickYes()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Salir del juego
    public void OnClickNo()
    {
        Application.Quit();
    }


}
