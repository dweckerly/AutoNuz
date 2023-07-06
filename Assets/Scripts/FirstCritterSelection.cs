using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class FirstCritterSelection : MonoBehaviour
{
    public CritterData[] starterCritters;
    public SelectCritterItem[] selectCritterItems;
    public GameObject SelectionPanel;

    public delegate void OnCritterSelection(Critter critter);
    public event OnCritterSelection CritterSelectionEvent;

    public delegate void OnCritterHover(CritterData critterData);
    public event OnCritterHover CritterHoverEvent;
    public event Action CritterHoverEventEnd;

    public void PopulateSelectionPanel()
    {
        for (int i = 0; i < selectCritterItems.Length; i++)
        {
            selectCritterItems[i].critterImage.sprite = starterCritters[i].CritterSprite;
            selectCritterItems[i].critterName.text = starterCritters[i].CritterName;
            selectCritterItems[i].critterData = starterCritters[i];
            selectCritterItems[i].CritterHoverEvent += CritterHover;
            selectCritterItems[i].CritterHoverEndEvent += CritterHoverEnd;
        }
    }

    private void OnDestroy() {
        for (int i = 0; i < selectCritterItems.Length; i++)
        {
            selectCritterItems[i].CritterHoverEvent -= CritterHover;
            selectCritterItems[i].CritterHoverEndEvent -= CritterHoverEnd;
        }
    }

    void CritterHover(CritterData critterData)
    {
        CritterHoverEvent?.Invoke(critterData);
    }

    void CritterHoverEnd()
    {
        CritterHoverEventEnd?.Invoke();
    }

    public void SelectStarterCritter(int index)
    {
        Critter selectedCritter =  new Critter(starterCritters[index], 5);
        CritterSelectionEvent?.Invoke(selectedCritter);
    }
}
