using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TypeMods
{
    public ElementalType ElementalType;
    public ElementalType[] Advantages;
    public ElementalType[] Disadvantages;
    public ElementalType[] CannotDamage;
}

[CreateAssetMenu(fileName = "TypeMatrix", menuName = "Data/TypeMatrix", order = 3)]
public class TypeMatrix: ScriptableObject
{
    public TypeMods[] TypeModifiers;
}
