using System.Runtime.ExceptionServices;
using UnityEngine;
using TMPro;
using UnityEditor;
using JetBrains.Annotations;
using UnityEngine.UIElements;
using UnityEditor.iOS;
using UnityEditor.SearchService;


public class SetGameScene : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;
    public GameObject continueButton;
    public GameObject currentScene;
    public static SetGameScene instance;
    public DataBase gameData;
    private bool player1seted;
    private bool player2seted;
    public SceneManagement changescene;
    void Awake() 
    {
        player2.GetComponent<SetPlayer>().titanfaction.GetComponent<FactionSelection>().enabled = false;
        player2.GetComponent<SetPlayer>().humanityfaction.GetComponent<FactionSelection>().enabled = false;
        player2.GetComponent<SetPlayer>().namelabel.GetComponent<NameLabel>().enabled = false;
       
        if (instance == null) 
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } 
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    
    public void Start()
    {
        continueButton.SetActive(false);
    }
    
    // Update is called once per frame
    void Update()
    {
        if(player1.GetComponent<SetPlayer>().faction != "" && player1.GetComponent<SetPlayer>().label != "" && !player1seted)
        {
            player1.GetComponent<SetPlayer>().titanfaction.GetComponent<FactionSelection>().enabled = false;
            player1.GetComponent<SetPlayer>().humanityfaction.GetComponent<FactionSelection>().enabled = false;
            player1.GetComponent<SetPlayer>().namelabel.GetComponent<NameLabel>().enabled = false;      
            player1seted = true;
            player2.GetComponent<SetPlayer>().titanfaction.GetComponent<FactionSelection>().enabled = true;
            player2.GetComponent<SetPlayer>().humanityfaction.GetComponent<FactionSelection>().enabled = true;
            player2.GetComponent<SetPlayer>().namelabel.GetComponent<NameLabel>().enabled = true;
        }
        if(player1.GetComponent<SetPlayer>().faction != "" && player1.GetComponent<SetPlayer>().label != "" && player2.GetComponent<SetPlayer>().faction != "" && player2.GetComponent<SetPlayer>().label != ""  )
        {
            player2seted = true;
            continueButton.SetActive(true);
        }
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            if(player1seted && player2seted)
            {
                player2.GetComponent<SetPlayer>().ResetValues();
                player2.GetComponent<SetPlayer>().faction = "";
                player2seted = false;
                continueButton.SetActive(false);
            }
            else 
            {   
                player1.GetComponent<SetPlayer>().ResetValues();
                player1seted = false;
                player2.GetComponent<SetPlayer>().titanfaction.GetComponent<FactionSelection>().enabled = false;
                player2.GetComponent<SetPlayer>().humanityfaction.GetComponent<FactionSelection>().enabled = false;
                player2.GetComponent<SetPlayer>().namelabel.GetComponent<NameLabel>().enabled = false;      
                player1.GetComponent<SetPlayer>().namelabel.GetComponent<NameLabel>().enabled = true;
                player1.GetComponent<SetPlayer>().faction = "";
                continueButton.SetActive(false);
            }
        }
        
      
    }
    //para el boton de cambio
    public void DisablePlayer()
    {  
        currentScene.SetActive(false);
        gameData.GetComponent<DataBase>().name1 = player1.GetComponent<SetPlayer>().label;
        gameData.GetComponent<DataBase>().faction1 = player1.GetComponent<SetPlayer>().faction;
        gameData.GetComponent<DataBase>().name2 = player2.GetComponent<SetPlayer>().label;
        gameData.GetComponent<DataBase>().faction2 = player2.GetComponent<SetPlayer>().faction;
        changescene.ChangeScene();   
    }
}
