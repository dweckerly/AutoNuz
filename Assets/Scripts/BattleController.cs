using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour
{
    public GameObject BattlePanel;

    public event Action OnRunSelected;

    public void PopulateBattleUI(Critter critter)
    {
        Debug.Log(critter.data.CritterName);
    }

    public void StartBattle()
    {

    }

    public void RunFromFight()
    {
        OnRunSelected?.Invoke();
    }
}
