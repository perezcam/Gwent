using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class YesandNoButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject Indicator;
    public AudioSource pop;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerEnter != null)
        {
            pop.Play();
            Indicator.SetActive(true);
        }
    }

 
    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerEnter != null)
        {
            Indicator.SetActive(false);
        }
        
    }
}
