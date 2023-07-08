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

    public CritterData[] starterCritters;

    private int AreaNumber = 0;

    private void Start() 
    {
        HUDController.CritterSwapEvent += SwapCritters;
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
        HUDController.HideCritterDetails();
        ShowFirstSelectionPanel();
    }

    private void OnDestroy() 
    {
        HUDController.CritterSwapEvent -= SwapCritters;
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
    }

    void SwapCritters(Critter c1, Critter c2)
    {
        int index1 = Array.IndexOf(playerCritters, c1);
        int index2 = Array.IndexOf(playerCritters, c2);
        playerCritters[index1] = c2;
        playerCritters[index2] = c1;
        HUDController.UpdateCritterRoster(playerCritters);
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
                    HUDController.AddCritterToRoster(playerCritters[i]);
                    selectedCritter = null;
                    ShowMap();
                    return;
                }
            }
        }
        // need some error message here about party being full
    }

    void SelectLocation(CritterData critterData)
    {
        HUDController.HideLocationDetails();
        ShowBattle();
        Critter wildCritter = new Critter(critterData, UnityEngine.Random.Range(2, 4));
        foreach(Critter playerCritter in playerCritters)
        {
            if (playerCritter != null && playerCritter.Alive)
            {
                BattleController.InitializeBattleUI(playerCritter, wildCritter);
                return;
            }
        }
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

    void PostBattle(Critter critter)
    {
        BattleController.BattlePanel.SetActive(false);
        MapController.DisableMap();
        CritterSelector.EnableSelectionPanel(critter);
    }

    void ShowFirstSelectionPanel()
    {
        BattleController.BattlePanel.SetActive(false);
        MapController.DisableMap();
        CritterSelector.EnableSelectionPanel(starterCritters);
    }

    void ShowMap()
    {
        AreaNumber++;
        CritterSelector.DisableSelectionPanel();
        BattleController.BattlePanel.SetActive(false);
        MapController.EnableMap(AreaNumber);
    }

    void ShowBattle()
    {
        CritterSelector.DisableSelectionPanel();
        MapController.DisableMap();
        BattleController.BattlePanel.SetActive(true);
    }

    void UpdateCritterRosterDisplay()
    {
        HUDController.UpdateCritterRosterHealthDisplays();
    }

    void ChangePlayerBattleCritter()
    {
        for(int i = 0; i < playerCritters.Length; i++)
        {
            if (playerCritters[i] != null && playerCritters[i].Alive)
            {
                BattleController.PopulatePlayerCritterUI(playerCritters[i]);
                BattleController.StartBattle();
                return;
            }
        }
        GameOver();            
    }

    void GameOver()
    {
        Debug.Log("Game Over...");
    }
}
