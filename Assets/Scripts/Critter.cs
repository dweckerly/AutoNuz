using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Critter
{
    const int GENETIC_MOD_MAX = 31;

    public Critter (CritterData _data, int _Level)
    {
        if (_Level < 1) _Level = 1;
        Level = _Level;
        data = _data;
        Xp = 0;
        neededXp = CalculateXp(Level + 1);
        personality = SelectPersonality();
        HpGeneticMod = UnityEngine.Random.Range(0, GENETIC_MOD_MAX);
        AttackGeneticMod = UnityEngine.Random.Range(0, GENETIC_MOD_MAX);
        DefenseGeneticMod = UnityEngine.Random.Range(0, GENETIC_MOD_MAX);
        SpeedGeneticMod = UnityEngine.Random.Range(0, GENETIC_MOD_MAX);
        float[] personalityModifiers = PersonalityModifiers();
        Hp = CalculateStat(data.HpBase, personalityModifiers[0], HpGeneticMod) + 10;
        Attack = CalculateStat(data.AttackBase, personalityModifiers[1], AttackGeneticMod);
        Defense = CalculateStat(data.DefenseBase, personalityModifiers[2], DefenseGeneticMod);
        Speed = CalculateStat(data.SpeedBase, personalityModifiers[3], SpeedGeneticMod);
        currentHp = Hp;
    }

    public int Level;
    public CritterData data;
    public int Hp;
    public int Attack;
    public int Defense;
    public int Speed;
    public int Xp;
    public int currentHp;
    public int neededXp;
    public Personality personality;
    int HpGeneticMod;
    int AttackGeneticMod;
    int DefenseGeneticMod;
    int SpeedGeneticMod;
    public bool Alive = true;

    public void LevelUp()
    {
        Level += 1;
        neededXp = CalculateXp(Level + 1);
        Xp = 0;
        float[] personalityModifiers = PersonalityModifiers();
        Hp = CalculateStat(data.HpBase, personalityModifiers[0], HpGeneticMod);
        Attack = CalculateStat(data.AttackBase, personalityModifiers[1], AttackGeneticMod);
        Defense = CalculateStat(data.DefenseBase, personalityModifiers[2], DefenseGeneticMod);
        Speed = CalculateStat(data.SpeedBase, personalityModifiers[3], SpeedGeneticMod);
        currentHp = Hp;
    }

    int CalculateXp()
    {
        if (data.levelingRate == LevelProgressionRate.Slow)
            return Mathf.RoundToInt(1.25f * Mathf.Pow(Level, 3));
        if (data.levelingRate == LevelProgressionRate.Medium)
            return Mathf.RoundToInt(Mathf.Pow(Level, 3));
        if (data.levelingRate == LevelProgressionRate.Fast)
            return Mathf.RoundToInt(0.75f * Mathf.Pow(Level, 3));
        return 0;
    }

    int CalculateXp(int level)
    {
        if (data.levelingRate == LevelProgressionRate.Slow)
            return Mathf.RoundToInt(1.25f * Mathf.Pow(level, 3));
        if (data.levelingRate == LevelProgressionRate.Medium)
            return Mathf.RoundToInt(Mathf.Pow(level, 3));
        if (data.levelingRate == LevelProgressionRate.Fast)
            return Mathf.RoundToInt(0.75f * Mathf.Pow(level, 3));
        return 0;
    }

    int CalculateStat(StatBase statBase, float personalityMod, int geneticMod)
    {
        return  Mathf.RoundToInt(((((2 * BaseStatInt(statBase) + geneticMod) * Level) / 100) + 5) * personalityMod);
    }

    int BaseStatInt(StatBase statBase)
    {
        if (statBase == StatBase.Poor) return 10;
        if (statBase == StatBase.Fair) return 35;
        if (statBase == StatBase.Average) return 50;
        if (statBase == StatBase.Superior) return 65;
        if (statBase == StatBase.Exemplary) return 90;
        return 0;
    }

    Personality SelectPersonality()
    {
        int count = Enum.GetValues( typeof(Personality)).Length;
        int index = UnityEngine.Random.Range(0, count);
        return (Personality) index;
    }

    float[] PersonalityModifiers()
    {
        switch (personality)
        {
            case Personality.Aggressive:
                return new float[4] { 1f, 1.1f, 0.9f, 1f };
            case Personality.Aloof:
                return new float[4] { 0.9f, 1f, 1f, 1.1f };
            case Personality.Balanced:
                return new float[4] { 1f, 1f, 1f, 1f };
            case Personality.Courteous:
                return new float[4] { 1.1f, 0.9f, 1f, 1f };
            case Personality.Cowardly:
                return new float[4] { 1f, 0.9f, 1f, 1.1f };
            case Personality.Gentle:
                return new float[4] { 1f, 0.9f, 1.1f, 1f };
            case Personality.Greedy:
                return new float[4] { 1f, 1f, 0.9f, 1.1f };
            case Personality.Hedonistic:
                return new float[4] { 1.1f, 1f, 0.9f, 1f };
            case Personality.Lazy:
                return new float[4] { 1f, 1f, 1.1f, 0.9f };
            case Personality.Mellow:
                return new float[4] { 1.1f, 1f, 1f, 0.9f };
            case Personality.Obedient:
                return new float[4] { 1f, 1f, 1f, 1f };
            case Personality.Proud:
                return new float[4] { 0.9f, 1f, 1.1f, 1f };
            case Personality.Rowdy:
                return new float[4] { 0.9f, 1.1f, 1f, 1f };
            case Personality.Stoic:
                return new float[4] { 1f, 1f, 1f, 1f };
            case Personality.Stubborn:
                return new float[4] { 1f, 1.1f, 1f, 0.9f };
            default:
                return new float[4] { 1, 1, 1, 1 };
        }
    }

    public void ReceiveXp(int amount)
    {
        Xp += amount;
        if (Xp >= neededXp) LevelUp();
    }
}
