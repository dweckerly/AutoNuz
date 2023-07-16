using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyBonusController : MonoBehaviour
{
    public Dictionary<ElementalType, int> PartyTypeBonuses = new Dictionary<ElementalType, int>();

    void InstantiateBonuses()
    {
        PartyTypeBonuses.Clear();
        foreach(ElementalType e in Enum.GetValues(typeof(ElementalType)))
        {
            PartyTypeBonuses.Add(e, 0);
        }
    }

    public void SetBonuses(Critter[] critters)
    {
        InstantiateBonuses();
        foreach(Critter critter in critters)
        {
            if (critter != null)
            {
                foreach(ElementalType e in critter.data.Types)
                {
                    PartyTypeBonuses[e]++;
                }
            }
        }
    }
}
