using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class SetPlayer : MonoBehaviour
{
    public GameObject titanfaction;
    public GameObject humanityfaction;
    public GameObject namelabel;
    public string faction;
    public string label;
    void Update()
    {
        if(titanfaction.GetComponent<FactionSelection>().isSelected)
        {
            titanfaction.GetComponent<FactionSelection>().enabled = false;
            humanityfaction.SetActive(false);
            faction = titanfaction.GetComponent<FactionSelection>().FactionName;
        }
        else if (humanityfaction.GetComponent<FactionSelection>().isSelected)
        {
            humanityfaction.GetComponent<FactionSelection>().enabled = false;
            titanfaction.SetActive(false);
            faction = humanityfaction.GetComponent<FactionSelection>().FactionName;
        }
        
        label = namelabel.GetComponent<NameLabel>().nick;
    }
    public void ResetValues()
    {
        humanityfaction.SetActive(true);
        titanfaction.SetActive(true);
        humanityfaction.GetComponent<FactionSelection>().InitialStatus();
        titanfaction.GetComponent<FactionSelection>().InitialStatus();
        humanityfaction.GetComponent<FactionSelection>().enabled = true;
        titanfaction.GetComponent<FactionSelection>().enabled = true;
        humanityfaction.GetComponent<FactionSelection>().isSelected = false;
        titanfaction.GetComponent<FactionSelection>().isSelected = false;
    }
}
