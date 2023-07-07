using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MapController : MonoBehaviour
{
    const float ULTRA_RARE_CATCH_RATE = 1f;
    const float RARE_CATCH_RATE = 0.9f;
    const float UNCOMMON_CATCH_RATE = 0.6f;
    
    public MapSelectItem[] MapItems;
    public LocationData[] Locations;
    public GameObject MapPanel;
    public GameObject MapSelectParent;
    public GameObject MapSelectItemPrefab;
    public TMP_Text AreaText;

    public delegate void OnLocationSelection(CritterData critterData);
    public event OnLocationSelection LocationSelectionEvent;

    public delegate void OnLocationHover(LocationData locationData);
    public event OnLocationHover LocationHoverEvent;
    public event Action LocationHoverEventEnd;

    public void EnableMap(int areaNumber)
    {
        foreach(MapSelectItem mapSelectItem in MapItems)
        {
            mapSelectItem.LocationSelectEvent += SelectLocationToExplore;
            mapSelectItem.LocationHoverEvent += LocationHover;
            mapSelectItem.LocationHoverEventEnd += LocationHoverEnd;
        }
        GenerateMap(areaNumber);
        MapPanel.SetActive(true);
    }

    public void DisableMap()
    {
        if (MapPanel.activeSelf)
        {
            foreach(MapSelectItem mapSelectItem in MapItems)
            {
                mapSelectItem.LocationSelectEvent -= SelectLocationToExplore;
                mapSelectItem.LocationHoverEvent -= LocationHover;
                mapSelectItem.LocationHoverEventEnd -= LocationHoverEnd;
            }
            MapPanel.SetActive(false);
        }
    }
    
    void GenerateMap(int areaNumber, bool addTown = false)
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
            MapItems[i].locationImage.sprite = selectedLocations[i].LocationSprite;
            MapItems[i].locationName.text = selectedLocations[i].LocationName;
            MapItems[i].locationData = selectedLocations[i];
        }
        AreaText.text = "Area " + areaNumber;
    }

    int SelectMapItem(List<int> ids)
    {
        return ids[UnityEngine.Random.Range(0, ids.Count)];
    }

    void SelectLocationToExplore(LocationData locationData)
    {
        CritterData cd = null;
        float rate = UnityEngine.Random.Range(0f, 1f);
        if (rate >= ULTRA_RARE_CATCH_RATE && CatchTableHasRateValue(locationData, CatchRate.UltraRare))
        {
            cd = FindCritterByCatchRate(locationData, CatchRate.UltraRare);
        }
        else if (rate >= RARE_CATCH_RATE && CatchTableHasRateValue(locationData, CatchRate.Rare))
        {
            cd = FindCritterByCatchRate(locationData, CatchRate.Rare);
        }
        else if (rate >= UNCOMMON_CATCH_RATE && CatchTableHasRateValue(locationData, CatchRate.Uncommon))
        {
            cd = FindCritterByCatchRate(locationData, CatchRate.Uncommon);
        }
        else
        {
            cd = FindCritterByCatchRate(locationData, CatchRate.Common);
        }
        LocationSelectionEvent?.Invoke(cd);
    }

    bool CatchTableHasRateValue(LocationData locationData, CatchRate catchRate)
    {
        for (int i = 0; i < locationData.CatchTable.Length; i++)
        {
            if (locationData.CatchTable[i].CatchRate == catchRate)
                return true;
        }
        return false;
    }

    CritterData FindCritterByCatchRate(LocationData locationData, CatchRate catchRate)
    {
        List<CritterData> possibleCritters = new List<CritterData>();
        for (int i = 0; i < locationData.CatchTable.Length; i++)
        {
            if (locationData.CatchTable[i].CatchRate == catchRate)
            {
                foreach (CritterData c in locationData.CatchTable[i].Critters)
                {
                    possibleCritters.Add(c);
                }
            }  
        }
        int selectedCritterIndex = UnityEngine.Random.Range(0, possibleCritters.Count);
        return possibleCritters[selectedCritterIndex];
    }

    void LocationHover(LocationData locationData)
    {
        LocationHoverEvent?.Invoke(locationData);
    }

    void LocationHoverEnd()
    {
        LocationHoverEventEnd?.Invoke();
    }
}
