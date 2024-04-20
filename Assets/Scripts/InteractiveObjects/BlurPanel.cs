using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PanelFader : MonoBehaviour
{
    public float fadeDuration = 0.5f; 
    public Button button;
    private Image panelImage;          
    private float originalAlpha;       

    void Awake()
    {
        panelImage = GetComponent<Image>();
        originalAlpha = panelImage.color.a; 
        // Establece el color inicial como transparente (alfa = 0) pero mantén los valores RGB
        panelImage.color = new Color(panelImage.color.r, panelImage.color.g, panelImage.color.b, 0);
    }

    public void FadeToOpaque()
    {
        gameObject.SetActive(true);
        
        Color finalColor = new Color(panelImage.color.r, panelImage.color.g, panelImage.color.b, originalAlpha);
        StartCoroutine(FadePanel(panelImage.color, finalColor, fadeDuration));
    }

    public void FadeToTransparent()
    {
        
        Color transparentColor = new Color(panelImage.color.r, panelImage.color.g, panelImage.color.b, 0);
        StartCoroutine(FadePanel(panelImage.color, transparentColor, fadeDuration));
    }

    private IEnumerator FadePanel(Color startColor, Color targetColor, float duration)
    {
        float time = 0;
        while (time < duration)
        {
            time += Time.deltaTime;
            panelImage.color = Color.Lerp(startColor, targetColor, time / duration);
            yield return null;
        }

        panelImage.color = targetColor;

        
        if (targetColor.a == 0)
        {
            gameObject.SetActive(false);
        }
    }
}
