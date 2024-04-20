using UnityEngine;
using TMPro;
using UnityEditor;
using UnityEditor.SearchService;
using JetBrains.Annotations;


public class SetGameScene : MonoBehaviour
{
    public TextMeshProUGUI TextName1;
    public TextMeshProUGUI TextName2;
    public string name1;
    public string name2;
    public string faction1;
    public string faction2;
    public GameObject continueButton;
    public static int count=0;
    public GameObject cortainplayer1;
    public GameObject cortainplayer2;
    public SceneAsset nextScene;
    public GameObject currentScene;
    public static SetGameScene instance;
    public DataBase gameData;

    void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else if (instance != this) {
            Destroy(gameObject);
        }
    }
    
    public void Start()
    {
        continueButton.SetActive(false);
        cortainplayer1.SetActive(false);
        cortainplayer2.SetActive(true);
    }
    

    // Update is called once per frame
    void Update()
    {
      
        if(FactionSelection.isSelected && NameLabel.isWrited)
        {
            continueButton.SetActive(true);
        }
        else
        {
            continueButton.SetActive(false);
        }
    }
    public void DisablePlayer()
    {
       
        if(count==0)
        {
            count++;
            cortainplayer1.SetActive(true);
            cortainplayer2.SetActive(false);
            continueButton.SetActive(false);
            FactionSelection.isSelected = false;
            NameLabel.isWrited = false;
        }
        else
        {
            name1=TextName1.text;
            name2=TextName2.text;
            currentScene.SetActive(false);
            gameData = new DataBase(name1,name2,faction1,faction2);
            SceneManagement.ChangeScene(nextScene.name);
        }
    }
}
