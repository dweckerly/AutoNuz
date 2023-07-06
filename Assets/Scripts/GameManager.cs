using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    Critter[] playerCritters = new Critter[5];
    Critter selectedCritter;

    public HUDController HUDController;
    public FirstCritterSelection FirstCritterSelection;
    public MapController MapController;
    public BattleController BattleController;
    public PostBattleController PostBattleController;

    private int AreaNumber = 0;

    private void Start() 
    {
        FirstCritterSelection.CritterHoverEvent += CritterHover;
        FirstCritterSelection.CritterHoverEventEnd += CritterHoverEnd;
        FirstCritterSelection.CritterSelectionEvent += CritterSelected;
        MapController.AreaSelectionEvent += SelectArea;
        BattleController.OnRunSelected += RunAway;
        BattleController.BattleEndEvent += PostBattle;
        BattleController.PlayerDefeated += GameOver;
        HUDController.HideDetails();
        ShowFirstSelectionPanel();
    }

    private void OnDestroy() 
    {
        FirstCritterSelection.CritterHoverEvent -= CritterHover;
        FirstCritterSelection.CritterHoverEventEnd -= CritterHoverEnd;
        FirstCritterSelection.CritterSelectionEvent -= CritterSelected;
        MapController.AreaSelectionEvent -= SelectArea;
        BattleController.OnRunSelected -= RunAway;
        BattleController.BattleEndEvent -= PostBattle;
        BattleController.PlayerDefeated -= GameOver;
    }

    void CritterHover(Critter critter)
    {
        selectedCritter = critter;
        HUDController.UpdateAndShowDetails(selectedCritter);
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

    }

    void ShowFirstSelectionPanel()
    {
        BattleController.BattlePanel.SetActive(false);
        MapController.MapPanel.SetActive(false);
        FirstCritterSelection.EnableSelectionPanel();
    }

    void ShowMap()
    {
        AreaNumber++;
        FirstCritterSelection.DisableSelectionPanel();
        BattleController.BattlePanel.SetActive(false);
        MapController.MapPanel.SetActive(true);
        MapController.GenerateMap(AreaNumber);
    }

    void ShowBattle()
    {
        FirstCritterSelection.DisableSelectionPanel();
        MapController.MapPanel.SetActive(false);
        BattleController.BattlePanel.SetActive(true);
    }

    void GameOver()
    {
        Debug.Log("Game Over...");
    }
}
