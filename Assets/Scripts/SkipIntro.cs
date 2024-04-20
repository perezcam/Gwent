using System.Diagnostics.Tracing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
   
    void Update()
    {
        if (Input.GetMouseButton(1)) 
        {
            SceneManager.LoadScene("PlayerScene"); 
        }
    }
}