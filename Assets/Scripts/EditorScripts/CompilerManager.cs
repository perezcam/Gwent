using System.Collections;
using System.Collections.Generic;
using Interpeter;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CompilerManager : MonoBehaviour
{
   public  TMP_InputField inputText;
   public  TMP_Text ErrorReporter;
   public static CompilerManager instance;
   public GameObject LeftTagsPanel;
   public GameObject CardVisor;
   public Evaluate evaluator{get;set;}
   public GameObject menu;
   
    void Start()
    {
        CardVisor.SetActive(false);
        ErrorReporter.text = "";
        
        if (instance == null) 
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } 
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        menu.SetActive(true);
    }
    public void RunCompiler()
    {
        ErrorReporter.text = "";
        //elimina todos los anteriores tags
            foreach (Transform child in LeftTagsPanel.transform)
            {
                Destroy(child.gameObject);
            }

        string input = SyntaxHighlighter.RemoveExistingColorTags(inputText.textComponent.text);
        Lexer lexer = new Lexer(false);
        List<Token> tokens = lexer.Tokenize(input);
        bool errors = false;
        
        Parser parser = new Parser(tokens);
        ProgramNode programNode = parser.Parse();
        foreach(ErrorBlockNode error in  parser.errorList)
        {
            ErrorReporter.text += error.Message + "\n";
            errors = true;
        }
        //Si no hay errores detectados en la fase de Parser comienza ChequeoSemantico
        if(parser.errorList.Count == 0)
        {
            ErrorReporter.text = "";
            SemanticChecker checker = new SemanticChecker(programNode);
            foreach(string error in checker.errors)
            {
                ErrorReporter.text += error + "\n";
                errors = true;
            }
        }
        if(errors == false)
            evaluator = new Evaluate(programNode,this);
        
        if(!errors)
            ErrorReporter.text += "COMPILACION COMPLETADA CON EXITO" + "\n";
        else
            ErrorReporter.text += "Errores durante la compilacion: rectifiquelos para continuar"+ "\n";
    }
    public void LoadScene1()
    {
        SceneManager.LoadScene("PlayerScene"); 
    }
   
    void Update()
    {
        
    }
}
