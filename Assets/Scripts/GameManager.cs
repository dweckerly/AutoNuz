using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    Critter[] playerCritters = new Critter[5];
    Critter selectedCritter;

    public HUDController HUDController;
    public CritterSelector CritterSelector;
    public MapController MapController;
    public BattleController BattleController;
    public AbilityController AbilityController;
    public AnimationsController AnimationsController;

    public CritterData[] starterCritters;

    private int DayNumber = 0;

    private void Start() 
    {
        HUDController.CritterSwapEvent += SwapCritters;
        HUDController.ReleaseCritterEvent += ReleaseCritter;
        HUDController.RestOptionSelectedEvent += RestCritters;
        CritterSelector.CritterHoverEvent += CritterHover;
        CritterSelector.CritterHoverEventEnd += CritterHoverEnd;
        CritterSelector.CritterSelectionEvent += CritterSelected;
        MapController.LocationSelectionEvent += SelectLocation;
        MapController.LocationHoverEvent += LocationHover;
        MapController.LocationHoverEventEnd += LocationHoverEnd;
        BattleController.OnRunSelected += RunAway;
        BattleController.BattleEndEvent += PostBattle;
        BattleController.PlayerCritterDefeated += ChangePlayerBattleCritter;
        BattleController.PlayerCritterDamaged += UpdateCritterRosterDisplay;
        BattleController.BattleStartEvent += WildBattleStart;
        BattleController.PlayerCritterAttackEvent += PlayerCritterAttack;
        BattleController.OpponentCritterAttackEvent += OpponentCritterAttack;
        BattleController.PlayerCritterBelow50HPEvent += PlayerBelow50HP;
        BattleController.OpponentCritterBelow50HPEvent += OpponentBelow50HP;
        BattleController.PlayerMissEvent += PlayerMissInBattle;
        BattleController.OpponentMissEvent += OpponentMissInBattle;
        AbilityController.OnCritterHPCHange += UpdateCritterHP;
        HUDController.HideCritterDetails();
        ShowFirstSelectionPanel();
    }

    private void OnDestroy() 
    {
        HUDController.CritterSwapEvent -= SwapCritters;
        HUDController.ReleaseCritterEvent -= ReleaseCritter;
        HUDController.RestOptionSelectedEvent -= RestCritters;
        CritterSelector.CritterHoverEvent -= CritterHover;
        CritterSelector.CritterHoverEventEnd -= CritterHoverEnd;
        CritterSelector.CritterSelectionEvent -= CritterSelected;
        MapController.LocationSelectionEvent -= SelectLocation;
        MapController.LocationHoverEvent -= LocationHover;
        MapController.LocationHoverEventEnd -= LocationHoverEnd;
        BattleController.OnRunSelected -= RunAway;
        BattleController.BattleEndEvent -= PostBattle;
        BattleController.PlayerCritterDefeated -= ChangePlayerBattleCritter;
        BattleController.PlayerCritterDamaged -= UpdateCritterRosterDisplay;
        BattleController.BattleStartEvent -= WildBattleStart;
        BattleController.PlayerCritterAttackEvent -= PlayerCritterAttack;
        BattleController.OpponentCritterAttackEvent -= OpponentCritterAttack;
        BattleController.PlayerCritterBelow50HPEvent -= PlayerBelow50HP;
        BattleController.OpponentCritterBelow50HPEvent -= OpponentBelow50HP;
        BattleController.PlayerMissEvent -= PlayerMissInBattle;
        BattleController.OpponentMissEvent -= OpponentMissInBattle;
        AbilityController.OnCritterHPCHange -= UpdateCritterHP;
    }

    void CritterSelected()
    {
        if (selectedCritter != null)
        {
            HUDController.HideCritterDetails();
            for (int i = 0; i < playerCritters.Length; i++)
            {
                if (playerCritters[i] == null)
                {
                    playerCritters[i] = selectedCritter;
                    playerCritters[i].HPChangeEvent += CritterHPChanged;
                    HUDController.AddCritterToRoster(playerCritters[i]);
                    selectedCritter = null;
                    ShowMap();
                    return;
                }
            }
            HUDController.ShowExpositoryText("Your party is full! Release another critter to keep this one.");
        }
    }

    void ReleaseCritter(Critter critter)
    {
        int critterCount = 0;
        foreach (Critter c in playerCritters)
        {
            if (c != null) critterCount++;
        }
        if (critterCount > 1)
        {
            int index = Array.IndexOf(playerCritters, critter);
            playerCritters[index].HPChangeEvent -= CritterHPChanged;
            playerCritters[index] = null;
            for (int i = 1; i < playerCritters.Length; i++)
            {
                if (playerCritters[i - 1] == null)
                {
                    playerCritters[i - 1] = playerCritters[i];
                    playerCritters[i] = null;
                }
            }
            HUDController.UpdateCritterRoster(playerCritters);
        }
    }

    void SwapCritters(Critter c1, Critter c2)
    {
        int index1 = Array.IndexOf(playerCritters, c1);
        int index2 = Array.IndexOf(playerCritters, c2);
        playerCritters[index1] = c2;
        playerCritters[index2] = c1;
        HUDController.UpdateCritterRoster(playerCritters);
        if (BattleController.BattlePanel.activeSelf)
        {
            foreach (Critter playerCritter in playerCritters)
            {
                if (playerCritter != null && playerCritter.Alive)
                {
                    BattleController.PopulatePlayerCritterUI(playerCritter);
                    return;
                }
            }
        }
    }

    void CritterHover(Critter critter)
    {
        selectedCritter = critter;
        HUDController.UpdateAndShowCritterDetails(selectedCritter);
    }

    void CritterHoverEnd()
    {
        selectedCritter = null;
        HUDController.HideCritterDetails();
    }

    void SelectLocation(CritterData critterData)
    {
        HUDController.HideLocationDetails();
        ShowBattle();
        int[] levelRange = LevelRangeByDayNumber();
        Critter wildCritter = new Critter(critterData, UnityEngine.Random.Range(levelRange[0], levelRange[1]));
        foreach(Critter playerCritter in playerCritters)
        {
            if (playerCritter != null && playerCritter.Alive)
            {
                HUDController.ShowExpositoryText("You encounter a wild " + wildCritter.data.CritterName + "!");
                BattleController.InitializeBattleUI(playerCritter, wildCritter);
                return;
            }
        }
    }

    int[] LevelRangeByDayNumber()
    {
        int[] range = new int[2];
        range[0] = 2 + Mathf.FloorToInt(DayNumber / 4);
        range[1] = 4 + Mathf.FloorToInt(DayNumber / 3);
        return range;
    }

    void LocationHover(LocationData locationData)
    {
        HUDController.ShowLocationDetails(locationData);
    }

    void LocationHoverEnd()
    {
        HUDController.HideLocationDetails();
    }

    void RunAway()
    {
        ShowMap();
    }

    void PostBattle(Critter playerCritter, Critter wildCritter)
    {
        playerCritter.ResetBattleEffectors();
        wildCritter.ResetBattleEffectors();
        AbilityController.DestroyPlayerEffectDisplayItems();
        AbilityController.DestroyOpponentEffectDisplayItems();
        BattleController.BattlePanel.SetActive(false);
        MapController.DisableMap();
        GiveXp(wildCritter.neededXp * 2);
        CritterSelector.EnableSelectionPanel(wildCritter);
        ResetSingleUseAbilityTrigger();
    }

    void ShowFirstSelectionPanel()
    {
        BattleController.BattlePanel.SetActive(false);
        MapController.DisableMap();
        CritterSelector.EnableSelectionPanel(starterCritters, false);
    }

    void ShowMap()
    {
        DayNumber++;
        CritterSelector.DisableSelectionPanel();
        BattleController.BattlePanel.SetActive(false);
        MapController.EnableMap(DayNumber);
    }

    void ShowBattle()
    {
        CritterSelector.DisableSelectionPanel();
        MapController.DisableMap();
        BattleController.BattlePanel.SetActive(true);
        AnimationsController.BattleStart();
        BattleController.CheckCritterHP();
    }

    void PlayerCritterAttack(Critter playerCritter, Critter opponentCritter, int damage)
    {
        AbilityController.CheckAbilityTriggerPlayerAttacking(playerCritter, opponentCritter, damage);
        AnimationsController.PlayerAttack();
    }

    void OpponentCritterAttack(Critter playerCritter, Critter opponentCritter, int damage)
    {
        AbilityController.CheckAbilityTriggerOpponentAttacking(playerCritter, opponentCritter, damage);
        AnimationsController.OpponentAttack();
    }

    void UpdateCritterHP()
    {
        UpdateCritterBattleHPDisplay();
        UpdateCritterRosterDisplay();
    }

    void UpdateCritterBattleHPDisplay()
    {
        BattleController.CheckCritterHP();
        BattleController.UpdateCritterHealthUI();
    }

    void UpdateCritterRosterDisplay()
    {
        HUDController.UpdateCritterRosterDisplays();
    }

    void ChangePlayerBattleCritter()
    {
        for(int i = 0; i < playerCritters.Length; i++)
        {
            if (playerCritters[i] != null && playerCritters[i].Alive)
            {
                AbilityController.DestroyPlayerEffectDisplayItems();
                BattleController.PopulatePlayerCritterUI(playerCritters[i]);
                BattleController.StartBattle();
                return;
            }
        }
        GameOver();            
    }

    void GameOver()
    {
        HUDController.ShowGameOverScreen();
    }

    void GiveXp(int amount)
    {
        int critterCount = 0;
        foreach(Critter c in playerCritters)
        {
            if (c != null && c.Alive) critterCount++;
        }
        int xpAmount = Mathf.CeilToInt((float)amount / (float)critterCount);
        foreach(Critter critter in playerCritters)
        {
            if(critter != null && critter.Alive) critter.ReceiveXp(xpAmount);
        }
        HUDController.UpdateCritterRoster(playerCritters);
    }

    void RestCritters()
    {
        foreach(Critter critter in playerCritters)
        {
            if (critter != null && critter.Alive) critter.Rest();
        }
        HUDController.UpdateCritterRosterDisplays();
        ShowMap();
    }

    void WildBattleStart(Critter playerCritter, Critter wildCritter)
    {
        ResetSingleUseAbilityTrigger();
        AbilityController.CheckAbilityTrigger(playerCritter, wildCritter, Trigger.OnEnter);
        HUDController.HideExpositoryText();
    }

    void PlayerBelow50HP(Critter playerCritter, Critter opponentCritter)
    {
        AnimationsController.SlowPlayerAnimSpeed();
        if (!playerCritter.singleUseAbilityTriggered)
        {
            AbilityController.CheckPlayerCritterAbilityTrigger(playerCritter, opponentCritter, Trigger.OnSelfHP50);
        }
        if (!opponentCritter.singleUseAbilityTriggered)
        {
            AbilityController.CheckOpponentCritterAbilityTrigger(playerCritter, opponentCritter, Trigger.OnOpponentHP50);
        }
    }

    void OpponentBelow50HP(Critter playerCritter, Critter opponentCritter)
    {
        AnimationsController.SlowOpponentAnimSpeed();
        if (!opponentCritter.singleUseAbilityTriggered)
        {
            AbilityController.CheckOpponentCritterAbilityTrigger(playerCritter, opponentCritter, Trigger.OnSelfHP50);
        }
        if (!playerCritter.singleUseAbilityTriggered)
        {
            AbilityController.CheckPlayerCritterAbilityTrigger(playerCritter, opponentCritter, Trigger.OnOpponentHP50);
        }
    }

    void ResetSingleUseAbilityTrigger()
    {
        foreach(Critter critter in playerCritters)
        {
            if (critter != null) critter.singleUseAbilityTriggered = false;
        }
    }

    void PlayerMissInBattle()
    {

    }

    void OpponentMissInBattle()
    {

    }

    void CritterHPChanged(Critter critter)
    {

    }
}
