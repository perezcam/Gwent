using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Player player1 = new Player();
    public Player player2 = new Player();
    public DeckManager deckManager = new DeckManager();
    
    public GameManager(string name1, string name2, string faction1, string faction2)
    {
        player1.Initialize(name1, faction1);
        player2.Initialize(name2, faction2);
    }

    public void Start()
    {
        Awake();
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        // else if (instance != this)
        // {
        //     Destroy(gameObject);
        // }
    }

   
    void Update()
    {
        
    }
}
