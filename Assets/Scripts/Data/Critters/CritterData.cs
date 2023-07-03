using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatProgressionRate
{
    Poor,
    Fair,
    Average,
    Superior,
    Exemplary
}

public enum LevelProgressionRate
{
    Slow,
    Medium,
    Fast
}

[CreateAssetMenu(fileName = "critter", menuName = "Data/Critter", order = 1)]
public class CritterData : ScriptableObject
{
    public string CritterName;
    public Sprite CritterSprite;
    public LevelProgressionRate levelingRate;
    public StatProgressionRate HpProgressionRate;
    public StatProgressionRate AttackProgressionRate;
    public StatProgressionRate DefenseProgressionRate;
    public StatProgressionRate SpeedProgressionRate;
}
