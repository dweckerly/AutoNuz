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

    float speedTimePlayer = 0f;
    float speedTimeWild = 0f;

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
        StartCoroutine(HandleTurnTIme());
    }

    public void RunFromFight()
    {
        OnRunSelected?.Invoke();
    }

    public float CalculateHealthPercentage(Critter critter)
    {
        return critter.currentHp / critter.Hp;
    }

    IEnumerator HandleTurnTIme()
    {
        while(true)
        {
            yield return new WaitForSeconds(0.01f);
            speedTimePlayer += playerCritter.Speed * Time.deltaTime;
            playerCritterUI.speedRect.localScale = new Vector3(speedTimePlayer / 2f, 1f, 1f);
            if (playerCritterUI.speedRect.localScale.x >= 1)
            {
                playerCritterUI.speedRect.localScale = new Vector3(0f, 1f, 1f);
                speedTimePlayer = 0f;
                Debug.Log("Attack!");
            }
        }
        
    }
}
