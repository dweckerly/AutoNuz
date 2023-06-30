using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "critter", menuName = "Data/Critter", order = 1)]
public class CritterData : ScriptableObject
{
    public string CritterName;
    public Sprite CritterSprite;
}
