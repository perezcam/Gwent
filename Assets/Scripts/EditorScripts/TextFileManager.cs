using System.IO;
using UnityEngine;
using TMPro; // Para TMP_Dropdown
using UnityEngine.UI;

public class TextFileManagerDropdown : MonoBehaviour
{
    public TMP_InputField inputField;
    public TMP_Dropdown dropdown; 
    public TMP_Text userFileName;
    private string folderPath = "Assets/Resources/TextFiles"; 
    void Start()
    {
        PopulateDropdown();
        dropdown.onValueChanged.AddListener(delegate { LoadSelectedFile(dropdown.value); });
    }

    void PopulateDropdown()
    {
        dropdown.options.Clear();
        // Obtener todos los archivos de texto en la carpeta
        string[] files = Directory.GetFiles(folderPath, "*.txt");

        foreach (string filePath in files)
        {
            string fileName = Path.GetFileName(filePath);
            dropdown.options.Add(new TMP_Dropdown.OptionData(fileName));
        }
        // Actualizar visualmente el Dropdown
        dropdown.RefreshShownValue(); 
    }

    public void LoadSelectedFile(int index)
    {
        string fileName = dropdown.options[index].text;
        string filePath = Path.Combine(folderPath, fileName);

        if (File.Exists(filePath))
        {
            string loadedText = File.ReadAllText(filePath);
            inputField.text = loadedText;
            Debug.Log("Texto cargado desde " + filePath);
        }
        else
        {   
            Debug.LogWarning("No se encontró el archivo en " + filePath);
        }
    }
    // Método para guardar el archivo con el nombre ingresado por el usuario
    public void SaveFile()
    {
        if (!string.IsNullOrEmpty(userFileName.text))
        {
            string filePath = Path.Combine(folderPath, userFileName.text + ".txt");
            File.WriteAllText(filePath, inputField.text);
            Debug.Log("Archivo guardado como " + userFileName.text);
        }
        else
        {
            Debug.LogWarning("El nombre del archivo no puede estar vacío.");
        }
    }
}
