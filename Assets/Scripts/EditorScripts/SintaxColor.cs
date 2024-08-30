using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;

public class SyntaxHighlighter : MonoBehaviour
{
    public TMP_InputField inputField;
    private Lexer lexer;
    private Dictionary<TokenType, string> tokenColors;
    private string defaultColor;

    void Start()
    {
        lexer = new Lexer(true);
        defaultColor = "#01F9FF";  // Azul como color por defecto
        tokenColors = new Dictionary<TokenType, string>
        {
            { TokenType.Identifier, defaultColor },      
            { TokenType.Effect, "#FFFF00" },  // Amarillo
            { TokenType.EffectKeyword, "#FFFF00" },        
            { TokenType.Card, "#FFFF00" },            
            { TokenType.Number, "#FF0000" },  // Rojo           
            { TokenType.String, "#00FF00" },  // Verde         
        };

        inputField.onValueChanged.AddListener(OnTextChanged);
    }

    public static string RemoveExistingColorTags(string input)
    {
        // Elimina todas las etiquetas <color> de apertura
        input = Regex.Replace(input, @"<\s*color\s*=\s*#?[A-Fa-f0-9]{6}\s*>", "", RegexOptions.IgnoreCase);
        // Elimina todas las etiquetas </color> de cierre
        input = Regex.Replace(input, @"<\/\s*color\s*>", "", RegexOptions.IgnoreCase);
        return input;
    }

    public void OnTextChanged(string input)
    {
        // Desactivar temporalmente el evento para evitar bucles infinitos
        inputField.onValueChanged.RemoveListener(OnTextChanged);

        // Guardar la posición actual del cursor
        int originalCaretPosition = inputField.caretPosition;

        // Limpia el texto de cualquier etiqueta de color existente
        string cleanInput = RemoveExistingColorTags(input);

        // Tokenizar el texto limpio
        var tokens = lexer.Tokenize(cleanInput);

        // Colorea el texto utilizando etiquetas de color
        string highlightedText = ApplySyntaxHighlighting(tokens);

        // Actualizar el contenido del InputField con el texto coloreado
        inputField.text = highlightedText;

        // Ajustar la posición del cursor
        int newCaretPosition = AdjustCaretPosition(originalCaretPosition, cleanInput, highlightedText);
        inputField.caretPosition = Mathf.Clamp(newCaretPosition, 0, highlightedText.Length);

        // Reactivar el evento
        inputField.onValueChanged.AddListener(OnTextChanged);
    }

    private int AdjustCaretPosition(int originalPosition, string originalText, string highlightedText)
    {
        int offset = highlightedText.Length - originalText.Length;
        int caretPosition = originalPosition + offset;

        // Asegurar que el cursor no esté dentro de etiquetas <color>
        string beforeCursor = highlightedText.Substring(0, Mathf.Clamp(caretPosition, 0, highlightedText.Length));
        
        // Verificar si la posición del cursor está dentro de una etiqueta de color
        if (beforeCursor.LastIndexOf("<color=") > beforeCursor.LastIndexOf("</color>"))
        {
            // Mover el cursor justo después de la etiqueta
            caretPosition = highlightedText.IndexOf(">", caretPosition) + 1;
        }
        return caretPosition;
    }


     private string ApplySyntaxHighlighting(List<Token> tokens)
    {
        // Empezamos con un texto vacío
        string highlightedText = "";

        foreach (var token in tokens)
        {
            if (tokenColors.TryGetValue(token.Type, out string color))
            {
                highlightedText += $"<color={color}>{token.Value}</color>";
            }
                
            else
            {
                // Si no hay color asignado, simplemente agregamos el texto sin colorear
                highlightedText += token.Value;
            }
        }
        
        // Asegura que no queden etiquetas sin cerrar o sin abrir
        // highlightedText = highlightedText.Replace("</color></color>", "</color>");
        // highlightedText = highlightedText.Replace("<color=", "</color><color=");

        // Devolvemos el texto con el resaltado aplicado
        return highlightedText;
    }
}
