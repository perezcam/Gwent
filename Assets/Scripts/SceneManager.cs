using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEditor;

public class SceneManagement : MonoBehaviour
{
    public VideoPlayer instruct_video; 
    public string nextSceneName;    

    void Start()
    {
        // Configura el video 
        instruct_video.loopPointReached += CheckOver;
    }

    public void PlayVideoAndChangeScene()
    {
        instruct_video.Play();  
    }

    // Cambia de escena cuando el video termina
    void CheckOver(VideoPlayer vp)
    {
        ChangeScene();  
    }

    public void ChangeScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}