using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class CritterBattleUI
{
    public Image critterSprite;
    public RectTransform healthRect;
    public RectTransform speedRect;
    public TMP_Text healthText;
    public TMP_Text levelText;
}

public class BattleController : MonoBehaviour
{
    public GameObject BattlePanel;
    public CritterBattleUI playerCritterUI;
    public CritterBattleUI wildCritterUI;

    public event Action OnRunSelected;

    Critter playerCritter;
    Critter wildCritter;

    public void PopulateBattleUI(Critter _playerCritter, Critter _wildCritter)
    {
        playerCritter = _playerCritter;
        wildCritter = _wildCritter;
        playerCritterUI.critterSprite.sprite = playerCritter.data.CritterSprite;
        playerCritterUI.healthText.text = playerCritter.currentHp + "/" + playerCritter.Hp;
        playerCritterUI.healthRect.localScale = new Vector3(CalculateHealthPercentage(playerCritter), 1f, 1f);
        playerCritterUI.speedRect.localScale = new Vector3(0, 1f, 1f);
        playerCritterUI.levelText.text = "LVL: " + playerCritter.Level;

        wildCritterUI.critterSprite.sprite = wildCritter.data.CritterSprite;
        wildCritterUI.healthText.text = wildCritter.currentHp + "/" + wildCritter.Hp;
        wildCritterUI.healthRect.localScale = new Vector3(CalculateHealthPercentage(wildCritter), 1f, 1f);
        wildCritterUI.speedRect.localScale = new Vector3(0, 1f, 1f);
        wildCritterUI.levelText.text = "LVL: " + wildCritter.Level;
    }

    public void StartBattle()
    {
        
    }

    public void RunFromFight()
    {
        OnRunSelected?.Invoke();
    }

    public float CalculateHealthPercentage(Critter critter)
    {
        return critter.currentHp / critter.Hp;
    }
}
