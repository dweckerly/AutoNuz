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
        MapController.AreaSelectionEvent += SelectArea;
        BattleController.OnRunSelected += RunAway;
        BattleController.BattleEndEvent += PostBattle;
        BattleController.PlayerDefeated += GameOver;
        HUDController.HideDetails();
        ShowFirstSelectionPanel();
    }

    private void OnDestroy() 
    {
        CritterSelector.CritterHoverEvent -= CritterHover;
        CritterSelector.CritterHoverEventEnd -= CritterHoverEnd;
        CritterSelector.CritterSelectionEvent -= CritterSelected;
        MapController.AreaSelectionEvent -= SelectArea;
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
        HUDController.HideDetails();
    }

    void CritterSelected()
    {
        if (selectedCritter != null)
        {
            HUDController.HideDetails();
            for (int i = 0; i < playerCritters.Length; i++)
            {
                if (playerCritters[i] == null)
                {
                    playerCritters[i] = selectedCritter;
                    selectedCritter = null;
                    ShowMap();
                    return;
                }
            }
        }
        // need some error message here about party being full
    }

    void SelectArea(CritterData critterData)
    {
        ShowBattle();
        Critter critter = new Critter(critterData, Random.Range(2, 5));
        BattleController.PopulateBattleUI(playerCritters[0], critter);
    }

    void RunAway()
    {
        ShowMap();
    }

    void PostBattle(Critter critter)
    {
        BattleController.BattlePanel.SetActive(false);
        MapController.MapPanel.SetActive(false);
        CritterSelector.EnableSelectionPanel(critter);
    }

    void ShowFirstSelectionPanel()
    {
        BattleController.BattlePanel.SetActive(false);
        MapController.MapPanel.SetActive(false);
        CritterSelector.EnableSelectionPanel(starterCritters);
    }

    void ShowMap()
    {
        AreaNumber++;
        CritterSelector.DisableSelectionPanel();
        BattleController.BattlePanel.SetActive(false);
        MapController.MapPanel.SetActive(true);
        MapController.GenerateMap(AreaNumber);
    }

    void ShowBattle()
    {
        CritterSelector.DisableSelectionPanel();
        MapController.MapPanel.SetActive(false);
        BattleController.BattlePanel.SetActive(true);
    }

    void GameOver()
    {
        Debug.Log("Game Over...");
    }
}
