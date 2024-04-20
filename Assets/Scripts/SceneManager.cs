using UnityEngine;
using UnityEngine.SceneManagement;

using System.Collections;


public class SceneManagement : MonoBehaviour
{
    public static void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName); 
    }
    
}

