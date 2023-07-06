using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstCritterSelection : MonoBehaviour
{
    public CritterData[] starterCritters;
    public SelectCritterItem[] selectCritterItems;
    public GameObject SelectionPanel;

    public delegate void OnCritterSelection(Critter critter);
    public event OnCritterSelection CritterSelectionEvent;

    public void PopulateSelectionPanel()
    {
        for (int i = 0; i < selectCritterItems.Length; i++)
        {
            selectCritterItems[i].critterImage.sprite = starterCritters[i].CritterSprite;
            selectCritterItems[i].critterName.text = starterCritters[i].CritterName;
            selectCritterItems[i].critterData = starterCritters[i];
        }
    }

    public void SelectStarterCritter(int index)
    {
        Critter selectedCritter =  new Critter(starterCritters[index], 5);
        CritterSelectionEvent?.Invoke(selectedCritter);
    }
}
