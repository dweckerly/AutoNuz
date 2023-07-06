using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    Critter[] playerCritters = new Critter[8];

    public HUDController HUDController;
    public FirstCritterSelection FirstCritterSelection;
    public MapMaker MapMaker;
    public BattleController BattleController;
    public PostBattleController PostBattleController;

    private void Start() 
    {
        FirstCritterSelection.CritterHoverEvent += CritterHover;
        FirstCritterSelection.CritterHoverEventEnd += CritterHoverEnd;
        FirstCritterSelection.CritterSelectionEvent += FirstCritterSelected;
        MapMaker.AreaSelectionEvent += SelectArea;
        BattleController.OnRunSelected += RunAway;
        BattleController.BattleEndEvent += PostBattle;
        BattleController.PlayerDefeated += GameOver;
        ShowFirstSelectionPanel();
    }

    private void OnDestroy() 
    {
        FirstCritterSelection.CritterHoverEvent -= CritterHover;
        FirstCritterSelection.CritterHoverEventEnd -= CritterHoverEnd;
        FirstCritterSelection.CritterSelectionEvent -= FirstCritterSelected;
        MapMaker.AreaSelectionEvent -= SelectArea;
        BattleController.OnRunSelected -= RunAway;
        BattleController.BattleEndEvent -= PostBattle;
        BattleController.PlayerDefeated -= GameOver;
    }

    void CritterHover(CritterData critterData)
    {

    }

    void CritterHoverEnd()
    {

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

    void PostBattle(Critter critter)
    {

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
        PostBattleController.PostBattlePanel.SetActive(false);
        MapMaker.MapPanel.SetActive(true);
        MapMaker.GenerateMap();
    }

    void ShowBattle()
    {
        FirstCritterSelection.SelectionPanel.SetActive(false);
        MapMaker.MapPanel.SetActive(false);
        PostBattleController.PostBattlePanel.SetActive(false);
        BattleController.BattlePanel.SetActive(true);
    }

    void ShowPostBattleScreen()
    {
        FirstCritterSelection.SelectionPanel.SetActive(false);
        MapMaker.MapPanel.SetActive(false);
        BattleController.BattlePanel.SetActive(false);
        PostBattleController.PostBattlePanel.SetActive(true);
    }

    void GameOver()
    {
        
    }
}
