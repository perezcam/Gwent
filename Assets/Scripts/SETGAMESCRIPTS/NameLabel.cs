using UnityEngine;
using TMPro; 
using UnityEngine.UI; 
using UnityEngine.EventSystems;
using Unity.VisualScripting;


public class NameLabel : MonoBehaviour
{
    public string nick;
    public TMP_InputField inputField; 
    public TextMeshProUGUI displayNameText; 
    private bool isWriting = false; 


    void Update()
    {
        if (isWriting)
        {
            if(Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                deselectInput();
            }
            inputField.ActivateInputField(); 
        }
    
    }
    public void deselectInput()
    {
        if(displayNameText.text.Length != 1)
        {
            isWriting = false;
        }
        else
        {
            isWriting = true;
        }
    }

    public void UpdateNameLabel()
    {    
         displayNameText.text = inputField.text; 
         nick = displayNameText.text;
    }

    public void SelectInput()
    {
        isWriting = true;
        inputField.text="";
    }
}
