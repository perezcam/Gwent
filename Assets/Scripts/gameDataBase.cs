using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;


public class DataBase : MonoBehaviour
{
    public string name1;
    public string name2;
    public string faction1;
    public string faction2;
    public DataBase(string name1, string name2, string faction1, string faction2)
    {
        this.name1=name1;
        this.name2=name2;
        this.faction1=faction1;
        this.faction2=faction2;
    }
}   

