using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Trigger
{
    OnEnter,
    OnTakeDamage,
    OnDealDamage,
    OnDefeated,
    OnKill,
    OnSelfHP50,
    OnOpponentHP50
}

public enum Target
{
    Self,
    Opponent,
    PlayerTeam,
    OpposingTeam
}

public enum Effector
{
    ATK,
    DEF,
    SPD, 
    HP,
    EVA
}

public enum Effect
{
    Increase,
    Decrease
}

public enum AmountType
{
    Percent,
    PercentDamageTaken,
    PercentDamageDealt
}

[CreateAssetMenu(fileName = "Ability", menuName = "Data/Ability", order = 3)]
public class AbilityData : ScriptableObject
{
    public string AbilityName;
    [TextArea]
    public string Description;
    public Trigger Trigger;
    public Target Target;
    public Effector Effector;
    public Effect Effect;
    public float Amount;
    public AmountType AmountType;
}
