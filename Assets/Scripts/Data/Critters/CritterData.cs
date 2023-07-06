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
    Balanced,
    Courteous, // +HP -ATK
    Cowardly, // +SPD -ATK
    Gentle, // +DEF -ATK
    Greedy, // +SPD -DEF
    Hedonistic, // +HP -DEF
    Lazy, // +DEF -SPD
    Mellow, // +HP -SPD
    Obedient,
    Proud, // +DEF -HP
    Rowdy, // +ATK -HP
    Stoic,
    Stubborn, // +ATK -SPD
}

public enum ElementalType
{
    Basic,
    Buff,
    Bug,
    Dirt,
    Electric,
    Fire,
    Plant,
    Shady,
    Spooky,
    Water,
    Wind,
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
    public ElementalType[] Types;
}
