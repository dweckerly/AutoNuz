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
        CritterSelector.CritterHoverEvent += CritterHover;
        CritterSelector.CritterHoverEventEnd += CritterHoverEnd;
        CritterSelector.CritterSelectionEvent += CritterSelected;
        MapController.LocationSelectionEvent += SelectLocation;
        MapController.LocationHoverEvent += LocationHover;
        MapController.LocationHoverEventEnd += LocationHoverEnd;
        BattleController.OnRunSelected += RunAway;
        BattleController.BattleEndEvent += PostBattle;
        BattleController.PlayerDefeated += GameOver;
        HUDController.HideCritterDetails();
        ShowFirstSelectionPanel();
    }

    private void OnDestroy() 
    {
        CritterSelector.CritterHoverEvent -= CritterHover;
        CritterSelector.CritterHoverEventEnd -= CritterHoverEnd;
        CritterSelector.CritterSelectionEvent -= CritterSelected;
        MapController.LocationSelectionEvent -= SelectLocation;
        MapController.LocationHoverEvent -= LocationHover;
        MapController.LocationHoverEventEnd -= LocationHoverEnd;
        BattleController.OnRunSelected -= RunAway;
        BattleController.BattleEndEvent -= PostBattle;
        BattleController.PlayerDefeated -= GameOver;
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
        Critter critter = new Critter(critterData, Random.Range(2, 5));
        BattleController.PopulateBattleUI(playerCritters[0], critter);
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

    void GameOver()
    {
        Debug.Log("Game Over...");
    }
}
