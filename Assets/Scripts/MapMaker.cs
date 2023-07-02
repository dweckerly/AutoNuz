using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class MapItem
{
    public Image locationSprites;
    public TMP_Text locationName;
    public LocationData location;
}

public class MapMaker : MonoBehaviour
{
    public MapItem[] MapItems;
    public LocationData[] Locations;

    private void Start() 
    {
        GenerateMap();
    }
    
    public void GenerateMap()
    {
        List<int> locationIDs = new List<int>();
        for(int i  = 0; i < Locations.Length; i++)
        {
            locationIDs.Add(i);
        }
        LocationData[] selectedLocations = new LocationData[MapItems.Length];
        for(int i  = 0; i < MapItems.Length; i++)
        {
            int selectedID = SelectMapItem(locationIDs);
            selectedLocations[i] = Locations[selectedID];
            locationIDs.Remove(selectedID);
        }

        for(int i = 0; i < MapItems.Length; i++)
        {
            MapItems[i].locationSprites.sprite = selectedLocations[i].LocationSprite;
            MapItems[i].locationName.text = selectedLocations[i].LocationName;
            MapItems[i].location = selectedLocations[i];
        }
    }

    public int SelectMapItem(List<int> ids)
    {
        return ids[Random.Range(0, ids.Count)];
    }

    public CritterData SelectAreaToExplore(int index)
    {
        float rate = Random.Range(0, 1);
        if (rate >= 1f)
        {
            if (CatchTableHasRateValue(index, CatchRate.UltraRare))
                return FindCritterByCatchRate(index, CatchRate.UltraRare);
        }
        if (rate >= 0.9f)
        {
            if (CatchTableHasRateValue(index, CatchRate.Rare))
                return FindCritterByCatchRate(index, CatchRate.Rare);
        }
        if (rate >= 0.6f)
        {
            if (CatchTableHasRateValue(index, CatchRate.Uncommon))
                return FindCritterByCatchRate(index, CatchRate.Uncommon);
        }
        return FindCritterByCatchRate(index, CatchRate.Common);
    }

    bool CatchTableHasRateValue(int mapIndex, CatchRate catchRate)
    {
        for (int i = 0; i < MapItems[mapIndex].location.CatchTable.Length; i++)
        {
            if (MapItems[mapIndex].location.CatchTable[i].CatchRate == catchRate)
                return true;
        }
        return false;
    }

    public CritterData FindCritterByCatchRate(int mapIndex, CatchRate catchRate)
    {
        List<CritterData> possibleCritters = new List<CritterData>();
        for (int i = 0; i < MapItems[mapIndex].location.CatchTable.Length; i++)
        {
            if (MapItems[mapIndex].location.CatchTable[i].CatchRate == catchRate)
            {
                foreach (CritterData c in MapItems[mapIndex].location.CatchTable[i].Critters)
                {
                    possibleCritters.Add(c);
                }
            }  
        }
        int selectedCritterIndex = Random.Range(0, possibleCritters.Count);
        return possibleCritters[selectedCritterIndex];
    }
}
