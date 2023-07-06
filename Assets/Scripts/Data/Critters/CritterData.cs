using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatBase
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

public enum Personality
{
    Aggressive, // +ATK -DEF
    Aloof, // +SPD -HP
    Courteous, // +HP -ATK
    Cowardly, // +SPD -ATK
    Daring, // +ATK -HP
    Greedy, // +SPD -DEF
    Hedonistic, // +HP -DEF
    Mellow, // +HP -SPD


}

[CreateAssetMenu(fileName = "critter", menuName = "Data/Critter", order = 1)]
public class CritterData : ScriptableObject
{
    public string CritterName;
    public Sprite CritterSprite;
    public LevelProgressionRate levelingRate;
    public StatBase HpBase;
    public StatBase AttackBase;
    public StatBase DefenseBase;
    public StatBase SpeedBase;
}
