using UnityEngine;

public class DisableComponentsOnChildren : MonoBehaviour
{
    void Update()
    {
        foreach (Transform child in transform)
        {
            if (child.GetComponent<DragHandler>() != null && child.GetComponent<DragHandler>().enabled)
            {
                child.GetComponent<DragHandler>().enabled = false;
            }
            if (child.GetComponent<CardDropHandler>() != null && child.GetComponent<CardDropHandler>().enabled)
            {
                child.GetComponent<CardDropHandler>().enabled = false;
            }
        }
    }
}
