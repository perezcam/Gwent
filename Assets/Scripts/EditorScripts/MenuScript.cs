using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class MenuScripts : MonoBehaviour
{
    public GameObject menu;
    public TMP_InputField archiveName;
    public  TextMeshProUGUI archiveTextName;
    public Button exitButton;

    private void Start()
    {
        if (archiveName != null)
        {
            archiveName.onEndEdit.AddListener(UpdateText);
        }
    }
    private void UpdateText(string newText)
    {
        archiveTextName.text = newText;
    }

    public void OnBottonCLick()
    {
        menu.SetActive(false);
    }
}

