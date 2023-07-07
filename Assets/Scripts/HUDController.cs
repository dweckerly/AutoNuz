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

    private void Awake()
    {
        CritterDetailsContainer.SetActive(false);
        LocationDetailsContainer.SetActive(false);
    }

    public void AddCritterToRoster(Critter critter)
    {
        CritterRosterItem critterRosterItem = Instantiate(RosterCritterPrefab, RosterContainer.transform).GetComponent<CritterRosterItem>();
        critterRosterItem.canvas = canvas;
        critterRosterItem.PopulateDetails(critter);
        foreach (CritterRosterSlot slot in DropSlots)
        {
            if (slot.critterRosterItem == null)
            {
                critterRosterItem.SetSlot(slot);
                return;
            }       
        }
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
}
