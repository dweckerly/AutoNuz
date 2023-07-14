using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Critter
{
    const int GENETIC_MOD_MAX = 101;
    const float PERSONALITY_POSITIVE_MOD = 1.1f;
    const float PERSONALITY_NEGATIVE_MOD = 0.9f;

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
        ResetBattleEffectors();
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
    public bool singleUseAbilityTriggered = false;

    public Dictionary<Effector, float> battleEffectors = new Dictionary<Effector, float>();

    public delegate void OnHPChange(Critter critter);
    public OnHPChange HPChangeEvent;

    public void LevelUp()
    {
        Level += 1;
        neededXp = CalculateXp(Level + 1);
        Xp = 0;
        float[] personalityModifiers = PersonalityModifiers();
        Hp = CalculateStat(data.HpBase, personalityModifiers[0], HpGeneticMod) + 10;
        Attack = CalculateStat(data.AttackBase, personalityModifiers[1], AttackGeneticMod);
        Defense = CalculateStat(data.DefenseBase, personalityModifiers[2], DefenseGeneticMod);
        Speed = CalculateStat(data.SpeedBase, personalityModifiers[3], SpeedGeneticMod);
    }

    public void ResetBattleEffectors()
    {
        battleEffectors.Clear();
        battleEffectors.Add(Effector.ATK, 1f);
        battleEffectors.Add(Effector.DEF, 1f);
        battleEffectors.Add(Effector.SPD, 1f);
    }

    public bool SetBattleEffector(Effector stat, float amount)
    {
        // return true if stat effector exists (!= 1)
        bool exists = battleEffectors[stat] != 1f;
        battleEffectors[stat] += amount / 100;
        return exists;
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
        if (statBase == StatBase.Poor) return 200;
        if (statBase == StatBase.Fair) return 350;
        if (statBase == StatBase.Average) return 500;
        if (statBase == StatBase.Superior) return 650;
        if (statBase == StatBase.Exemplary) return 800;
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
                return new float[4] { 1f, PERSONALITY_POSITIVE_MOD, PERSONALITY_NEGATIVE_MOD, 1f };
            case Personality.Aloof:
                return new float[4] { PERSONALITY_NEGATIVE_MOD, 1f, 1f, PERSONALITY_POSITIVE_MOD };
            case Personality.Balanced:
                return new float[4] { 1f, 1f, 1f, 1f };
            case Personality.Courteous:
                return new float[4] { PERSONALITY_POSITIVE_MOD, PERSONALITY_NEGATIVE_MOD, 1f, 1f };
            case Personality.Cowardly:
                return new float[4] { 1f, PERSONALITY_NEGATIVE_MOD, 1f, PERSONALITY_POSITIVE_MOD };
            case Personality.Gentle:
                return new float[4] { 1f, PERSONALITY_NEGATIVE_MOD, PERSONALITY_POSITIVE_MOD, 1f };
            case Personality.Greedy:
                return new float[4] { 1f, 1f, PERSONALITY_NEGATIVE_MOD, PERSONALITY_POSITIVE_MOD };
            case Personality.Hedonistic:
                return new float[4] { PERSONALITY_POSITIVE_MOD, 1f, PERSONALITY_NEGATIVE_MOD, 1f };
            case Personality.Lazy:
                return new float[4] { 1f, 1f, PERSONALITY_POSITIVE_MOD, PERSONALITY_NEGATIVE_MOD };
            case Personality.Mellow:
                return new float[4] { PERSONALITY_POSITIVE_MOD, 1f, 1f, PERSONALITY_NEGATIVE_MOD };
            case Personality.Obedient:
                return new float[4] { 1f, 1f, 1f, 1f };
            case Personality.Proud:
                return new float[4] { PERSONALITY_NEGATIVE_MOD, 1f, PERSONALITY_POSITIVE_MOD, 1f };
            case Personality.Rowdy:
                return new float[4] { PERSONALITY_NEGATIVE_MOD, PERSONALITY_POSITIVE_MOD, 1f, 1f };
            case Personality.Stoic:
                return new float[4] { 1f, 1f, 1f, 1f };
            case Personality.Stubborn:
                return new float[4] { 1f, PERSONALITY_POSITIVE_MOD, 1f, PERSONALITY_NEGATIVE_MOD };
            default:
                return new float[4] { 1f, 1f, 1f, 1f };
        }
    }

    public void ReceiveXp(int amount)
    {
        Xp += amount;
        if (Xp >= neededXp) LevelUp();
    }

    public void Rest()
    {
        Heal(Mathf.RoundToInt(Hp / 2));
    }

    public void TakeDamage(int amount)
    {
        currentHp -= amount;
        if (currentHp < 0) currentHp = 0;
        HPChangeEvent.Invoke(this);
    }

    public void Heal(int amount)
    {
        currentHp += amount;
        if (currentHp > Hp) currentHp = Hp;
        HPChangeEvent.Invoke(this);
    }

    public float CalculateHealthPercentage()
    {
        return (float) currentHp / (float) Hp;
    }
}
