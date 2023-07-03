using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    Critter[] playerCritters = new Critter[8];
    public GameObject BattlePanel;

    public FirstCritterSelection FirstCritterSelection;
    public MapMaker MapMaker;

    private void Start() 
    {
        FirstCritterSelection.CritterSelectionEvent += FirstCritterSelected;
        MapMaker.AreaSelectionEvent += SelectArea;

        FirstCritterSelection.PopulateSelectionPanel();
    }

    private void OnDestroy() 
    {
        FirstCritterSelection.CritterSelectionEvent -= FirstCritterSelected;
        MapMaker.AreaSelectionEvent -= SelectArea;
    }

    void FirstCritterSelected(Critter critter)
    {
        playerCritters[0] = critter;
        ShowMap();
    }

    void SelectArea(CritterData critterData)
    {
        Debug.Log(critterData.CritterName);
    }

    void ShowMap()
    {
        FirstCritterSelection.SelectionPanel.SetActive(false);
        BattlePanel.SetActive(false);
        MapMaker.MapPanel.SetActive(true);
        MapMaker.GenerateMap();
    }
}
