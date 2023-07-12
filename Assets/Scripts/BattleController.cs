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
    const float BASE_BATTLE_SPEED = 20f;
    public GameObject BattlePanel;
    public CritterBattleUI playerCritterUI;
    public CritterBattleUI wildCritterUI;
    public GameObject FightRunBtns;

    public event Action OnRunSelected;
    public event Action PlayerCritterDamaged;
    public event Action PlayerCritterDefeated;
    public delegate void OnBattleEnd(Critter playerCritter, Critter wildCritter);
    public OnBattleEnd BattleEndEvent;
    public event Action BattleStart;

    public Critter playerCritter;
    Critter wildCritter;

    float speedTimePlayer = 0f;
    float speedTimeWild = 0f;

    bool battling = false;

    public TypeMatrix TypeMatrix;

    public void InitializeBattleUI(Critter _playerCritter, Critter _wildCritter)
    {
        PopulatePlayerCritterUI(_playerCritter);
        PopulateWildCritterUI(_wildCritter);
        FightRunBtns.SetActive(true);
    }

    public void PopulatePlayerCritterUI(Critter _playerCritter)
    {
        playerCritter = _playerCritter;
        playerCritterUI.critterSprite.sprite = playerCritter.data.CritterSprite;
        playerCritterUI.healthText.text = playerCritter.currentHp + "/" + playerCritter.Hp;
        playerCritterUI.healthRect.localScale = new Vector3(CalculateHealthPercentage(playerCritter), 1f, 1f);
        playerCritterUI.speedRect.localScale = new Vector3(0, 1f, 1f);
        playerCritterUI.levelText.text = "LVL: " + playerCritter.Level;
    }

    void PopulateWildCritterUI(Critter _wildCritter)
    {
        wildCritter = _wildCritter;
        wildCritterUI.critterSprite.sprite = wildCritter.data.CritterSprite;
        wildCritterUI.healthText.text = wildCritter.currentHp + "/" + wildCritter.Hp;
        wildCritterUI.healthRect.localScale = new Vector3(CalculateHealthPercentage(wildCritter), 1f, 1f);
        wildCritterUI.speedRect.localScale = new Vector3(0, 1f, 1f);
        wildCritterUI.levelText.text = "LVL: " + wildCritter.Level;
    }

    public void StartBattle()
    {
        battling = true;
        FightRunBtns.SetActive(false);
        BattleStart.Invoke();
        StartCoroutine(HandleTurnTime());
    }

    public void RunFromFight()
    {
        OnRunSelected?.Invoke();
    }

    public float CalculateHealthPercentage(Critter critter)
    {
        return (float)critter.currentHp / (float)critter.Hp;
    }

    IEnumerator HandleTurnTime()
    {
        while(battling)
        {
            yield return new WaitForSeconds(0.01f);

            speedTimePlayer += (playerCritter.Speed * playerCritter.battleEffectors[Effector.SPD]) * Time.deltaTime;
            playerCritterUI.speedRect.localScale = new Vector3(speedTimePlayer / BASE_BATTLE_SPEED, 1f, 1f);
            if (playerCritterUI.speedRect.localScale.x >= 1)
            {
                playerCritterUI.speedRect.localScale = new Vector3(0f, 1f, 1f);
                speedTimePlayer = 0f;
                PlayerCritterAttack();
            }
            speedTimeWild += (wildCritter.Speed * wildCritter.battleEffectors[Effector.SPD]) * Time.deltaTime;
            wildCritterUI.speedRect.localScale = new Vector3(speedTimeWild / BASE_BATTLE_SPEED, 1f, 1f);
            if (wildCritterUI.speedRect.localScale.x >= 1)
            {
                wildCritterUI.speedRect.localScale = new Vector3(0f, 1f, 1f);
                speedTimeWild = 0f;
                WildCritterAttack();
            }
        }
    }

    void PlayerCritterAttack()
    {
        int damage = DamageCalc(playerCritter, wildCritter);
        wildCritter.currentHp -= damage;
        if (wildCritter.currentHp < 0) wildCritter.currentHp = 0;
        wildCritterUI.healthText.text = wildCritter.currentHp + "/" + wildCritter.Hp;
        wildCritterUI.healthRect.localScale = new Vector3((float)(wildCritter.currentHp) / (float)(wildCritter.Hp), 1f, 1f);
        if (wildCritter.currentHp == 0) 
        {
            battling = false;
            speedTimePlayer = 0f;
            speedTimeWild = 0f;
            wildCritter.currentHp = Mathf.RoundToInt(wildCritter.Hp / 2);
            StopAllCoroutines();
            BattleEndEvent?.Invoke(playerCritter, wildCritter);
        }
    }

    void WildCritterAttack()
    {
        int damage = DamageCalc(wildCritter, playerCritter);
        playerCritter.currentHp -= damage;
        if (playerCritter.currentHp < 0) playerCritter.currentHp = 0;
        playerCritterUI.healthText.text = playerCritter.currentHp + "/" + playerCritter.Hp;
        playerCritterUI.healthRect.localScale = new Vector3((float)(playerCritter.currentHp) / (float)(playerCritter.Hp), 1f, 1f);
        PlayerCritterDamaged.Invoke();
        if (playerCritter.currentHp == 0) 
        {
            playerCritter.Alive = false;
            battling = false;
            speedTimePlayer = 0f;
            speedTimeWild = 0f;
            StopAllCoroutines();
            playerCritter.ResetBattleEffectors();
            PlayerCritterDefeated?.Invoke();
        }
    }

    int DamageCalc(Critter attackingCritter, Critter defendingCritter)
    {
        float typeMod = DetermineTypeAdvantages(attackingCritter, defendingCritter);
        float baseDamage = ((((2 * attackingCritter.Level) / 5) * ((attackingCritter.Attack * attackingCritter.battleEffectors[Effector.ATK]) / (defendingCritter.Defense * defendingCritter.battleEffectors[Effector.DEF]))) / 30) + 10;
        float total = baseDamage * typeMod;
        if (total < 1) total = 1;
        return Mathf.RoundToInt(total);
    }

    float DetermineTypeAdvantages(Critter attackingCritter, Critter defendingCritter)
    {
        float mod = 1f;
        foreach (ElementalType attackerType in attackingCritter.data.Types)
        {
            foreach (TypeMods tmod in TypeMatrix.TypeModifiers)
            {
                if (tmod.ElementalType == attackerType)
                {
                    foreach(ElementalType defenderTypes in defendingCritter.data.Types)
                    {
                        foreach(ElementalType immuneType in tmod.CannotDamage)
                        {
                            if (defenderTypes == immuneType) return 0;
                        }
                        foreach (ElementalType advEType in tmod.Advantages)
                        {
                            if(defenderTypes == advEType) mod *= 2f;
                        }
                        foreach(ElementalType disEType in tmod.Disadvantages)
                        {
                            if (defenderTypes == disEType) mod *= 0.5f;
                        }
                    }
                }
            }
        }
        return mod;
    }
}
