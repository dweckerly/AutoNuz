using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "location", menuName = "Data/Location", order = 2)]
public class LocationData : ScriptableObject
{
    public string LocationName;
    public Sprite LocationSprite;
    public CatchEntry[] CatchTable;
}
