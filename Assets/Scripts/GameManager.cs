using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class SelectCritterItem
{
    public Image critterImage;
    public TMP_Text critterName;
}

public class GameManager : MonoBehaviour
{
    Critter[] playerCritters = new Critter[8];
    public CritterData[] starterCritters;
    public SelectCritterItem[] selectCritterItems;
    public GameObject SelectionPanel;
    public GameObject MapPanel;
    public GameObject BattlePanel;

    private void Start() 
    {
        for (int i = 0; i < selectCritterItems.Length; i++)
        {
            selectCritterItems[i].critterImage.sprite = starterCritters[i].CritterSprite;
            selectCritterItems[i].critterName.text = starterCritters[i].CritterName;
        }
    }

    public void SelectStarterCritter(int index)
    {
        playerCritters[0] = new Critter(starterCritters[index], 5);
        ShowMap();
    }

    void ShowMap()
    {
        SelectionPanel.SetActive(false);
        BattlePanel.SetActive(false);
        MapPanel.SetActive(true);
    }
}
