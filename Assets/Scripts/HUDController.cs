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
    public GameObject HUD;
    public CritterDetails CritterDetails;
    public GameObject DetailsContainer;

    public void UpdateDetails(Critter critter)
    {
        CritterDetails.CritterName.text = critter.data.CritterName;
        CritterDetails.Personality.text = nameof(critter.personality);
        string types = "";
        foreach(ElementalType et in critter.data.Types)
        {
            types += nameof(et) + " ";
        }
        CritterDetails.Types.text = types;
        CritterDetails.HP.text = critter.currentHp + "/" + critter.Hp;
        CritterDetails.ATK.text = critter.Attack.ToString();
        CritterDetails.DEF.text = critter.Defense.ToString();
        CritterDetails.SPD.text = critter.Speed.ToString();
    }
}
