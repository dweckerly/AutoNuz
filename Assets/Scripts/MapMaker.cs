using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

[System.Serializable]
public class MapItem
{
    public Image locationSprites;
    public TMP_Text locationName;
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
        }
    }

    public int SelectMapItem(List<int> ids)
    {
        return ids[Random.Range(0, ids.Count)];
    }
}
