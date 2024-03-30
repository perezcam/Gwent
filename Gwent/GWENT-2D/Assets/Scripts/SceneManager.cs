using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    public static void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName); 
    }
}
