using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    Critter[] playerCritters = new Critter[8];

    public FirstCritterSelection FirstCritterSelection;
    public MapMaker MapMaker;
    public BattleController BattleController;

    private void Start() 
    {
        FirstCritterSelection.CritterSelectionEvent += FirstCritterSelected;
        MapMaker.AreaSelectionEvent += SelectArea;
        BattleController.OnRunSelected += RunAway;
        ShowFirstSelectionPanel();
    }

    private void OnDestroy() 
    {
        FirstCritterSelection.CritterSelectionEvent -= FirstCritterSelected;
        MapMaker.AreaSelectionEvent -= SelectArea;
        BattleController.OnRunSelected -= RunAway;
    }

    void FirstCritterSelected(Critter critter)
    {
        playerCritters[0] = critter;
        ShowMap();
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

    void ShowFirstSelectionPanel()
    {
        BattleController.BattlePanel.SetActive(false);
        MapMaker.MapPanel.SetActive(false);
        FirstCritterSelection.SelectionPanel.SetActive(true);
        FirstCritterSelection.PopulateSelectionPanel();
    }

    void ShowMap()
    {
        FirstCritterSelection.SelectionPanel.SetActive(false);
        BattleController.BattlePanel.SetActive(false);
        MapMaker.MapPanel.SetActive(true);
        MapMaker.GenerateMap();
    }

    void ShowBattle()
    {
        FirstCritterSelection.SelectionPanel.SetActive(false);
        MapMaker.MapPanel.SetActive(false);
        BattleController.BattlePanel.SetActive(true);
    }
}
