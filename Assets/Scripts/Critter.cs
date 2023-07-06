using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Critter
{
    public Critter (CritterData _data, int _Level)
    {
        if (_Level < 1) _Level = 1;
        Level = _Level;
        data = _data;
        Xp = CalculateXp();
        neededXp = CalculateXp(Level + 1);
        Hp = CalculateStat(data.HpBase) + 10;
        Attack = CalculateStat(data.AttackBase);
        Defense = CalculateStat(data.DefenseBase);
        Speed = CalculateStat(data.SpeedBase);
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

    int CalculateStat(StatBase statBase)
    {
        
        return  (((2 * ReturnBaseStatInt(statBase)) * Level) / 100) + 5;
    }

    int ReturnBaseStatInt(StatBase statBase)
    {
        if (statBase == StatBase.Poor) return 10;
        if (statBase == StatBase.Fair) return 20;
        if (statBase == StatBase.Average) return 30;
        if (statBase == StatBase.Superior) return 40;
        if (statBase == StatBase.Exemplary) return 50;
        return 0;
    }

    void LevelUp()
    {
        Level += 1;
        neededXp = CalculateXp(Level + 1);
        Hp = CalculateStat(data.HpBase);
        Attack = CalculateStat(data.AttackBase);
        Defense = CalculateStat(data.DefenseBase);
        Speed = CalculateStat(data.SpeedBase);
        currentHp = Hp;
    }
}
