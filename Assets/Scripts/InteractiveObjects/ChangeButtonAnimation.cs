using System.Diagnostics;
using System.Collections;
using UnityEngine;

public class ButtonPulse : MonoBehaviour
{
    public float maxScale = 4; 
    public float minScale = 3.8f;  
    public float pulseSpeed = 1.5f; 

    private Vector3 originalScale; 

    private void Start()
    {
        originalScale = transform.localScale; 
        StartCoroutine(Pulse()); 
    }
    public void Update()
    {
        StartCoroutine(Pulse());
    }

    private IEnumerator Pulse()
    {
    
        while (true) 
        {
            // Animación de aumento de tamaño
            float timer = 0.0f;
            while (timer <= 1.0f)
            {
                timer += Time.deltaTime * pulseSpeed;
                float scale = Mathf.Lerp(minScale, maxScale, timer);
                transform.localScale = new Vector3(scale, scale, scale); 
                yield return null;
            }

            // Animación de disminución de tamaño
            timer = 0.0f;
            while (timer <= 1.0f)
            {
                timer += Time.deltaTime * pulseSpeed;
                float scale = Mathf.Lerp(maxScale, minScale, timer);
                transform.localScale = new Vector3(scale, scale, scale); 
                yield return null;
            }
        }   
    }
}