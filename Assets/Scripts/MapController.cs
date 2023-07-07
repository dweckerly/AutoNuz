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

public class MapController : MonoBehaviour
{
    const float ULTRA_RARE_CATCH_RATE = 1f;
    const float RARE_CATCH_RATE = 0.9f;
    const float UNCOMMON_CATCH_RATE = 0.6f;
    
    public MapItem[] MapItems;
    public LocationData[] Locations;
    public GameObject MapPanel;
    public TMP_Text AreaText;

    public delegate void OnAreaSelection(CritterData critterData);
    public event OnAreaSelection AreaSelectionEvent;
    
    public void GenerateMap(int areaNumber, bool addTown = false)
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
        AreaText.text = "Area " + areaNumber;
    }

    public int SelectMapItem(List<int> ids)
    {
        return ids[Random.Range(0, ids.Count)];
    }

    public void SelectAreaToExplore(int index)
    {
        CritterData cd = null;
        float rate = Random.Range(0f, 1f);
        if (rate >= ULTRA_RARE_CATCH_RATE && CatchTableHasRateValue(index, CatchRate.UltraRare))
        {
            cd = FindCritterByCatchRate(index, CatchRate.UltraRare);
        }
        else if (rate >= RARE_CATCH_RATE && CatchTableHasRateValue(index, CatchRate.Rare))
        {
            cd = FindCritterByCatchRate(index, CatchRate.Rare);
        }
        else if (rate >= UNCOMMON_CATCH_RATE && CatchTableHasRateValue(index, CatchRate.Uncommon))
        {
            cd = FindCritterByCatchRate(index, CatchRate.Uncommon);
        }
        else
        {
            cd = FindCritterByCatchRate(index, CatchRate.Common);
        }
        AreaSelectionEvent?.Invoke(cd);
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
