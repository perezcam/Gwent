using UnityEngine;
using TMPro;
using System.Threading;
using UnityEngine.SceneManagement;
using UnityEditor;


public class SetGameScene : MonoBehaviour
{
    public TextMeshProUGUI nick1;
   // public TextMeshProUGUI nick2;
    public GameObject faction1;
    //public GameObject FactionName2;
    public GameObject keyButton;
    public static int count=0;
    public GameObject cortainplayer1;
   // public GameObject cortainplayer2;
    public SceneAsset nextScene;
    

    
    // Start is called before the first frame update
    public void Start()
    {
        Debug.Log("Start");
        GameManager game = new (nick1.text, faction1, "Frank", "Titans");
        Debug.Log("Nombre player1: "+game.player1.playerName);
        keyButton.SetActive(false);
        cortainplayer1.SetActive(false);
    //    cortainplayer2.SetActive(true);

    }
    

    // Update is called once per frame
    void Update()
    {
      
        if(FactionSelection.itSelected && NameLabel.isWrited)
        {
            keyButton.SetActive(true);
        }
        else
        {
            keyButton.SetActive(false);
        }
    }
    public void DisablePlayer()
    {
        //Voy a desactivar todo esto por ahora para que al darle al boton solo cambie fde escena con un jugador
        if(count==0)
        {
            count++;
            cortainplayer1.SetActive(true);
            cortainplayer1.SetActive(false);
            keyButton.SetActive(false);
            FactionSelection.itSelected = false;
            NameLabel.isWrited = false;
        }
        else
        {
            SceneManagement.ChangeScene(nextScene.name);
        }
    }
}
