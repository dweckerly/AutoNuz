using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class CritterDetails
{
    public TMP_Text CritterName;
    public TMP_Text Ability;
    public TMP_Text Personality;
    public TMP_Text Types;
    public TMP_Text HP;
    public TMP_Text ATK;
    public TMP_Text DEF;
    public TMP_Text SPD;

}

public class HUDController : MonoBehaviour
{
    public Canvas canvas;
    public GameObject HUD;
    public CritterDetails CritterDetails;
    public GameObject CritterDetailsContainer;
    public TMP_Text LocationDetails;
    public GameObject LocationDetailsContainer;
    public GameObject RosterContainer;

    public GameObject RosterCritterPrefab;
    public CritterRosterSlot[] DropSlots;
    public List<CritterRosterItem> rosterItems = new List<CritterRosterItem>();
    public CritterReleaseSlot critterReleaseSlot;

    public delegate void OnCritterSwap(Critter c1, Critter c2);
    public OnCritterSwap CritterSwapEvent;

    public delegate void OnCritterRelease(Critter critter);
    public OnCritterRelease ReleaseCritterEvent;

    private void Awake()
    {
        CritterDetailsContainer.SetActive(false);
        LocationDetailsContainer.SetActive(false);
        foreach(CritterRosterSlot crs in DropSlots)
        {
            crs.CritterSwapEvent += CritterSwap;
        }
        critterReleaseSlot.DropReleaseEvent += DropReleaseCritter;
    }

    public void UpdateCritterRoster(Critter[] critters)
    {
        foreach(CritterRosterItem cri in rosterItems)
        {
            Destroy(cri.gameObject);
        }
        rosterItems.Clear();
        foreach (CritterRosterSlot slot in DropSlots)
        {
            slot.critterRosterItem = null;
        }
        foreach(Critter critter in critters)
        {
            AddCritterToRoster(critter);
        }
    }

    public void AddCritterToRoster(Critter critter)
    {
        CritterRosterItem critterRosterItem = Instantiate(RosterCritterPrefab, RosterContainer.transform).GetComponent<CritterRosterItem>();
        critterRosterItem.canvas = canvas;
        critterRosterItem.PopulateDetails(critter);
        critterRosterItem.OnDragEvent += RosterItemDragEvent;
        critterRosterItem.OnDragEventEnd += RosterItemDragEventEnd;
        foreach (CritterRosterSlot slot in DropSlots)
        {
            if (slot.critterRosterItem == null)
            {
                slot.critterRosterItem = critterRosterItem;
                critterRosterItem.SetAnchoredPosition(slot.rectTransform.anchoredPosition);
                rosterItems.Add(critterRosterItem);
                return;
            }       
        }
    }

    private void OnDestroy() 
    {
        foreach(CritterRosterSlot crs in DropSlots)
        {
            crs.CritterSwapEvent -= CritterSwap;
        }
        foreach(CritterRosterItem cri in rosterItems)
        {
            cri.OnDragEvent -= RosterItemDragEvent;
            cri.OnDragEventEnd -= RosterItemDragEventEnd;
        }
        critterReleaseSlot.DropReleaseEvent += DropReleaseCritter;
    }

    void RosterItemDragEvent()
    {
        foreach (CritterRosterItem cri in rosterItems)
            cri.canvasGroup.blocksRaycasts = false;
    }

    void RosterItemDragEventEnd()
    {
        foreach (CritterRosterItem cri in rosterItems)
            cri.canvasGroup.blocksRaycasts = true;
    }

    public void UpdateAndShowCritterDetails(Critter critter)
    {
        CritterDetails.CritterName.text = critter.data.CritterName;
        CritterDetails.Personality.text = critter.personality.ToString();
        string types = "";
        foreach(ElementalType et in critter.data.Types)
        {
            types += et.ToString() + "/";
        }
        CritterDetails.Types.text = types.Remove(types.Length - 1, 1); ;
        CritterDetails.HP.text = critter.currentHp + "/" + critter.Hp;
        CritterDetails.ATK.text = critter.Attack.ToString();
        CritterDetails.DEF.text = critter.Defense.ToString();
        CritterDetails.SPD.text = critter.Speed.ToString();
        CritterDetailsContainer.SetActive(true);
    }

    public void HideCritterDetails()
    {
        CritterDetailsContainer.SetActive(false);
    }

    public void ShowLocationDetails(LocationData locationData)
    {
        LocationDetails.text = locationData.Description;
        LocationDetailsContainer.SetActive(true);
    }

    public void HideLocationDetails()
    {
        LocationDetailsContainer.SetActive(false);
    }

    void CritterSwap(Critter c1, Critter c2)
    {
        CritterSwapEvent.Invoke(c1, c2);
    } 

    public void UpdateCritterRosterDisplays()
    {
        foreach(CritterRosterItem cri in rosterItems)
        {
            cri.healthBar.localScale = new Vector3((float)cri.critter.currentHp / (float)cri.critter.Hp, 1f, 1f);
            cri.healthText.text = cri.critter.currentHp + "/" + cri.critter.Hp;
            cri.xpBar.localScale = new Vector3((float)cri.critter.Xp / (float)cri.critter.neededXp, 1f, 1f);
            cri.level.text = cri.critter.Level.ToString();
            if (cri.critter.currentHp <= 0) cri.DeathOverlay.SetActive(true);
        }
    }

    void DropReleaseCritter(CritterRosterItem critterRosterItem)
    {
        ReleaseCritterEvent?.Invoke(critterRosterItem.critter);
    }
}
