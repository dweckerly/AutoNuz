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
    public GameObject FightRunBtns;

    public event Action OnRunSelected;
    public event Action EndOfBattle;

    Critter playerCritter;
    Critter wildCritter;

    float speedTimePlayer = 0f;
    float speedTimeWild = 0f;

    bool battling = false;

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
        FightRunBtns.SetActive(true);
    }

    public void StartBattle()
    {
        battling = true;
        FightRunBtns.SetActive(false);
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
        while(battling)
        {
            yield return new WaitForSeconds(0.01f);

            speedTimePlayer += playerCritter.Speed * Time.deltaTime;
            playerCritterUI.speedRect.localScale = new Vector3(speedTimePlayer / 2f, 1f, 1f);
            if (playerCritterUI.speedRect.localScale.x >= 1)
            {
                playerCritterUI.speedRect.localScale = new Vector3(0f, 1f, 1f);
                speedTimePlayer = 0f;
                DamageWildCritter();
            }
            speedTimeWild += wildCritter.Speed * Time.deltaTime;
            wildCritterUI.speedRect.localScale = new Vector3(speedTimeWild / 1f, 1f, 1f);
            if (wildCritterUI.speedRect.localScale.x >= 1)
            {
                wildCritterUI.speedRect.localScale = new Vector3(0f, 1f, 1f);
                speedTimeWild = 0f;
                DamagePlayerCritter();
            }
        }
    }

    void DamageWildCritter()
    {
        int damage = Mathf.Clamp(playerCritter.Attack - wildCritter.Defense, 1, playerCritter.Attack);
        wildCritter.currentHp -= damage;
        if (wildCritter.currentHp < 0) wildCritter.currentHp = 0;
        wildCritterUI.healthText.text = wildCritter.currentHp + "/" + wildCritter.Hp;
        wildCritterUI.healthRect.localScale = new Vector3((float)(wildCritter.currentHp) / (float)(wildCritter.Hp), 1f, 1f);
        if (wildCritter.currentHp == 0) 
        {
            battling = false;
            EndOfBattle?.Invoke();
        }
    }

    void DamagePlayerCritter()
    {
        int damage = Mathf.Clamp(wildCritter.Attack - playerCritter.Defense, 1, wildCritter.Attack);
        playerCritter.currentHp -= damage;
        if (playerCritter.currentHp < 0) playerCritter.currentHp = 0;
        playerCritterUI.healthText.text = playerCritter.currentHp + "/" + playerCritter.Hp;
        playerCritterUI.healthRect.localScale = new Vector3((float)(playerCritter.currentHp) / (float)(playerCritter.Hp), 1f, 1f);
        if (playerCritter.currentHp == 0) 
        {
            battling = false;
            EndOfBattle?.Invoke();
        }
    }
}
