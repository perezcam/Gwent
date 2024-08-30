using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScene : MonoBehaviour
{
    public GameObject soundPanel; 
    public AudioSource gameMusic; 
    void Start()
    {
        soundPanel.SetActive(false); 
    }

    public void LoadScene1()
    {
        SceneManager.LoadScene("PlayerScene"); 
    }

    public void LoadScene2()
    {
        SceneManager.LoadScene("CompilerScene"); 
    }

    public void ToggleSoundPanel()
    {
        soundPanel.SetActive(!soundPanel.activeSelf);
    }
    public void OnButtonClick()
    {
        soundPanel.SetActive(false);
    }

    public void IncreaseVolume()
    {
        gameMusic.volume = Mathf.Min(gameMusic.volume + 0.1f, 1.0f); 
    }

    public void DecreaseVolume()
    {
        gameMusic.volume = Mathf.Max(gameMusic.volume - 0.1f, 0.0f); 
    }
}

