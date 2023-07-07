using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CritterSelector : MonoBehaviour
{
    public GameObject SelectCritterItemPrefab;
    List<SelectCritterItem> selectCritterItems = new List<SelectCritterItem>();
    public GameObject SelectionParent;
    public GameObject SelectionPanel;

    public event Action CritterSelectionEvent;

    public delegate void OnCritterHover(Critter critter);
    public event OnCritterHover CritterHoverEvent;
    public event Action CritterHoverEventEnd;

    public void EnableSelectionPanel(CritterData[] critters)
    {
        SelectionParent.SetActive(true);
        for (int i = 0; i < critters.Length; i++)
        {
            SelectCritterItem SelectCritterItem = Instantiate(SelectCritterItemPrefab, SelectionPanel.transform).GetComponent<SelectCritterItem>();
            selectCritterItems.Add(SelectCritterItem);
        }
        for (int i = 0; i < critters.Length; i++)
        {
            selectCritterItems[i].critterImage.sprite = critters[i].CritterSprite;
            selectCritterItems[i].critterName.text = critters[i].CritterName;
            selectCritterItems[i].critter = new Critter(critters[i], 5);
            selectCritterItems[i].CritterHoverEvent += CritterHover;
            selectCritterItems[i].CritterHoverEndEvent += CritterHoverEnd;
            selectCritterItems[i].CritterSelectedEvent += SelectCritter;
        }
    }

    public void DisableSelectionPanel() 
    {
        if (SelectionParent.activeSelf)
        {
            for (int i = 0; i < selectCritterItems.Count; i++)
            {
                selectCritterItems[i].CritterHoverEvent -= CritterHover;
                selectCritterItems[i].CritterHoverEndEvent -= CritterHoverEnd;
                selectCritterItems[i].CritterSelectedEvent -= SelectCritter;
                Destroy(selectCritterItems[i].gameObject);
            }
            SelectionParent.SetActive(false);
            selectCritterItems.Clear();
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
