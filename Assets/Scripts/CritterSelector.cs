using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CritterSelector : MonoBehaviour
{
    public GameObject CritterSelectItemPrefab;
    List<CritterSelectItem> critterSelectItems = new List<CritterSelectItem>();
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
            CritterSelectItem CritterSelectItem = Instantiate(CritterSelectItemPrefab, SelectionPanel.transform).GetComponent<CritterSelectItem>();
            critterSelectItems.Add(CritterSelectItem);
        }
        for (int i = 0; i < critters.Length; i++)
        {
            critterSelectItems[i].critterImage.sprite = critters[i].CritterSprite;
            critterSelectItems[i].critterName.text = critters[i].CritterName;
            critterSelectItems[i].critter = new Critter(critters[i], 5);
            critterSelectItems[i].CritterHoverEvent += CritterHover;
            critterSelectItems[i].CritterHoverEndEvent += CritterHoverEnd;
            critterSelectItems[i].CritterSelectedEvent += SelectCritter;
        }
    }

    public void EnableSelectionPanel(Critter critter)
    {
        SelectionParent.SetActive(true);
        CritterSelectItem SelectCritterItem = Instantiate(CritterSelectItemPrefab, SelectionPanel.transform).GetComponent<CritterSelectItem>();
        critterSelectItems.Add(SelectCritterItem);
        critterSelectItems[0].critterImage.sprite = critter.data.CritterSprite;
        critterSelectItems[0].critterName.text = critter.data.CritterName;
        critterSelectItems[0].critter = critter;
        critterSelectItems[0].CritterHoverEvent += CritterHover;
        critterSelectItems[0].CritterHoverEndEvent += CritterHoverEnd;
        critterSelectItems[0].CritterSelectedEvent += SelectCritter;
    }

    public void DisableSelectionPanel() 
    {
        if (SelectionParent.activeSelf)
        {
            for (int i = 0; i < critterSelectItems.Count; i++)
            {
                critterSelectItems[i].CritterHoverEvent -= CritterHover;
                critterSelectItems[i].CritterHoverEndEvent -= CritterHoverEnd;
                critterSelectItems[i].CritterSelectedEvent -= SelectCritter;
                Destroy(critterSelectItems[i].gameObject);
            }
            SelectionParent.SetActive(false);
            critterSelectItems.Clear();
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
