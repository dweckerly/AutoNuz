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
        Hp = CalculateStat(data.HpProgressionRate) + 10;
        Attack = CalculateStat(data.AttackProgressionRate);
        Defense = CalculateStat(data.DefenseProgressionRate);
        Speed = CalculateStat(data.SpeedProgressionRate);
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

    int CalculateStat(StatProgressionRate rate)
    {
        if (rate == StatProgressionRate.Poor) return Mathf.RoundToInt(0.25f * Level);
        if (rate == StatProgressionRate.Fair) return Mathf.RoundToInt(0.5f * Level);
        if (rate == StatProgressionRate.Average) return Mathf.RoundToInt(0.75f * Level);
        if (rate == StatProgressionRate.Superior) return Mathf.RoundToInt(1.25f * Level);
        if (rate == StatProgressionRate.Exemplary) return Mathf.RoundToInt(1.5f * Level);
        return  0;
    }

    void LevelUp()
    {
        Level += 1;
        neededXp = CalculateXp(Level + 1);
        Hp = CalculateStat(data.HpProgressionRate);
        Attack = CalculateStat(data.AttackProgressionRate);
        Defense = CalculateStat(data.DefenseProgressionRate);
        Speed = CalculateStat(data.SpeedProgressionRate);
        currentHp = Hp;
    }
}
