using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class FirstCritterSelection : MonoBehaviour
{
    public CritterData[] starterCritters;
    public SelectCritterItem[] selectCritterItems;
    public GameObject SelectionPanel;

    public event Action CritterSelectionEvent;

    public delegate void OnCritterHover(Critter critter);
    public event OnCritterHover CritterHoverEvent;
    public event Action CritterHoverEventEnd;

    public void EnableSelectionPanel()
    {
        for (int i = 0; i < selectCritterItems.Length; i++)
        {
            selectCritterItems[i].critterImage.sprite = starterCritters[i].CritterSprite;
            selectCritterItems[i].critterName.text = starterCritters[i].CritterName;
            selectCritterItems[i].critter = new Critter(starterCritters[i], 5);
            selectCritterItems[i].CritterHoverEvent += CritterHover;
            selectCritterItems[i].CritterHoverEndEvent += CritterHoverEnd;
            selectCritterItems[i].CritterSelectedEvent += SelectCritter;
        }
        SelectionPanel.SetActive(true);
    }

    public void DisableSelectionPanel() 
    {
        if (SelectionPanel.activeSelf)
        {
            for (int i = 0; i < selectCritterItems.Length; i++)
            {
                selectCritterItems[i].CritterHoverEvent -= CritterHover;
                selectCritterItems[i].CritterHoverEndEvent -= CritterHoverEnd;
                selectCritterItems[i].CritterSelectedEvent -= SelectCritter;
            }
            SelectionPanel.SetActive(false);
        }
    }

    void CritterHover(Critter critter)
    {
        CritterHoverEvent?.Invoke(critter);
    }

    void CritterHoverEnd()
    {
        CritterHoverEventEnd?.Invoke();
    }

    void SelectCritter()
    {
        CritterSelectionEvent?.Invoke();
    }
}
