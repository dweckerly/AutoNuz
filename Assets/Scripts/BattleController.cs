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
    const float BASE_BATTLE_SPEED = 50f;
    const float SPEED_LOG_BASE = 1.5f;
    const float SPEED_ADDITIVE_MOD = 20f;
    const float SPEED_MULTI_MOD = 2f;
    const float SPEED_EXPO_MOD = 2.5f;
    public GameObject BattlePanel;
    public CritterBattleUI playerCritterUI;
    public CritterBattleUI wildCritterUI;
    public GameObject FightRunBtns;

    public event Action OnRunSelected;
    public delegate void OnPlayerCritterAttack(Critter playerCritter, Critter opponentCritter, int damage);
    public OnPlayerCritterAttack PlayerCritterAttackEvent;
    public delegate void OnOpponentCritterAttack(Critter playerCritter, Critter opponentCritter, int damage);
    public OnOpponentCritterAttack OpponentCritterAttackEvent;
    public event Action PlayerCritterDamaged;
    public event Action PlayerCritterDefeated;
    public delegate void OnBattleEnd(Critter playerCritter, Critter wildCritter);
    public OnBattleEnd BattleEndEvent;
    public delegate void OnBattleStart(Critter playerCritter, Critter wildCritter);
    public OnBattleStart BattleStartEvent;
    public delegate void OnPlayerCritterBelow50HP(Critter playerCritter, Critter wildCritter);
    public OnPlayerCritterBelow50HP PlayerCritterBelow50HPEvent;
    public delegate void OnOpponentCritterBelow50HP(Critter playerCritter, Critter wildCritter);
    public OnOpponentCritterBelow50HP OpponentCritterBelow50HPEvent;
    public event Action PlayerMissEvent;
    public event Action OpponentMissEvent;

    public Critter playerCritter;
    Critter wildCritter;

    float speedTimePlayer = 0f;
    float speedTimeWild = 0f;

    bool battling = false;

    private TypeMatrix TypeMatrix;

    public void InitializeBattleUI(Critter _playerCritter, Critter _wildCritter)
    {
        PopulatePlayerCritterUI(_playerCritter);
        PopulateWildCritterUI(_wildCritter);
        FightRunBtns.SetActive(true);
    }

    public void SetTypeMatrix(TypeMatrix typeMatrix)
    {
        TypeMatrix = typeMatrix;
    }

    public void PopulatePlayerCritterUI(Critter _playerCritter)
    {
        playerCritter = _playerCritter;
        playerCritterUI.critterSprite.sprite = playerCritter.data.CritterSprite;
        playerCritterUI.healthText.text = playerCritter.currentHp + "/" + playerCritter.Hp;
        playerCritterUI.healthRect.localScale = new Vector3(playerCritter.CalculateHealthPercentage(), 1f, 1f);
        playerCritterUI.speedRect.localScale = new Vector3(0, 1f, 1f);
        playerCritterUI.levelText.text = "LVL: " + playerCritter.Level;
    }

    void PopulateWildCritterUI(Critter _wildCritter)
    {
        wildCritter = _wildCritter;
        wildCritterUI.critterSprite.sprite = wildCritter.data.CritterSprite;
        wildCritterUI.healthText.text = wildCritter.currentHp + "/" + wildCritter.Hp;
        wildCritterUI.healthRect.localScale = new Vector3(wildCritter.CalculateHealthPercentage(), 1f, 1f);
        wildCritterUI.speedRect.localScale = new Vector3(0, 1f, 1f);
        wildCritterUI.levelText.text = "LVL: " + wildCritter.Level;
    }

    public void StartBattle()
    {
        battling = true;
        FightRunBtns.SetActive(false);
        BattleStartEvent?.Invoke(playerCritter, wildCritter);
        StartCoroutine(HandleTurnTime());
    }

    public void RunFromFight()
    {
        OnRunSelected?.Invoke();
    }

    float SpeedCalc(Critter critter)
    {
        if (critter == null) return 0f;
        return (SPEED_MULTI_MOD * (Mathf.Log(Mathf.Pow(critter.Speed, SPEED_EXPO_MOD), SPEED_LOG_BASE) + SPEED_ADDITIVE_MOD)) * Time.deltaTime * critter.battleEffectors[Effector.SPD];
    }

    IEnumerator HandleTurnTime()
    {
        while(battling)
        {
            yield return new WaitForSeconds(0.01f);
            speedTimePlayer += SpeedCalc(playerCritter);
            playerCritterUI.speedRect.localScale = new Vector3(speedTimePlayer / BASE_BATTLE_SPEED, 1f, 1f);
            if (playerCritterUI.speedRect.localScale.x >= 1)
            {
                playerCritterUI.speedRect.localScale = new Vector3(0f, 1f, 1f);
                speedTimePlayer = 0f;
                PlayerCritterAttack();
            }
            speedTimeWild += SpeedCalc(wildCritter);
            wildCritterUI.speedRect.localScale = new Vector3(speedTimeWild / BASE_BATTLE_SPEED, 1f, 1f);
            if (wildCritterUI.speedRect.localScale.x >= 1)
            {
                wildCritterUI.speedRect.localScale = new Vector3(0f, 1f, 1f);
                speedTimeWild = 0f;
                WildCritterAttack();
            }
        }
    }

    public void UpdateCritterHealthUI()
    {
        if (wildCritter != null && playerCritter != null)
        {
            wildCritterUI.healthText.text = wildCritter.currentHp + "/" + wildCritter.Hp;
            wildCritterUI.healthRect.localScale = new Vector3((float)(wildCritter.currentHp) / (float)(wildCritter.Hp), 1f, 1f);
            playerCritterUI.healthText.text = playerCritter.currentHp + "/" + playerCritter.Hp;
            playerCritterUI.healthRect.localScale = new Vector3((float)(playerCritter.currentHp) / (float)(playerCritter.Hp), 1f, 1f);
        }
    }

    void PlayerCritterAttack()
    {
        float hitchance = UnityEngine.Random.Range(0f, 1f);
        if (hitchance > (2f - wildCritter.battleEffectors[Effector.EVA]))
        {
            PlayerMissEvent?.Invoke();
            return;
        }
        int damage = DamageCalc(playerCritter, wildCritter);
        PlayerCritterAttackEvent?.Invoke(playerCritter, wildCritter, damage);
        wildCritter?.TakeDamage(damage);
        CheckCritterHP();
        UpdateCritterHealthUI();
    }

    void WildCritterAttack()
    {
        float hitchance = UnityEngine.Random.Range(0f, 1f);
        if (hitchance > (2f - playerCritter.battleEffectors[Effector.EVA]))
        {
            OpponentMissEvent?.Invoke();
            return;
        }
        int damage = DamageCalc(wildCritter, playerCritter);
        OpponentCritterAttackEvent?.Invoke(playerCritter, wildCritter, damage);
        playerCritter?.TakeDamage(damage);
        CheckCritterHP();
        UpdateCritterHealthUI();
    }

    public void CheckCritterHP()
    {
        CheckPlayerCritterHP();
        CheckWildCritterHP();
    }

    void CheckWildCritterHP()
    {
        if (wildCritter != null)
        {
            if (wildCritter.currentHp <= wildCritter.Hp / 2) OpponentCritterBelow50HPEvent?.Invoke(playerCritter, wildCritter);
            if (wildCritter.currentHp <= 0)
            {
                battling = false;
                speedTimePlayer = 0f;
                speedTimeWild = 0f;
                wildCritter.currentHp = Mathf.RoundToInt(wildCritter.Hp / 2);
                StopAllCoroutines();
                BattleEndEvent?.Invoke(playerCritter, wildCritter);
                playerCritter = null;
                wildCritter = null;
            }
        }
    }

    void CheckPlayerCritterHP()
    {
        PlayerCritterDamaged?.Invoke();
        if (playerCritter != null)
        {
            if (playerCritter.currentHp <= playerCritter.Hp / 2) PlayerCritterBelow50HPEvent?.Invoke(playerCritter, wildCritter);
            if (playerCritter.currentHp <= 0)
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
    }

    int DamageCalc(Critter attackingCritter, Critter defendingCritter)
    {
        float typeMod = DetermineTypeAdvantages(attackingCritter, defendingCritter);
        float baseDamage = ((((2 * attackingCritter.Level) / 5) * (attackingCritter.Attack / defendingCritter.Defense)) / 30) + 10;
        float total = baseDamage * typeMod * attackingCritter.battleEffectors[Effector.ATK] * (2 - defendingCritter.battleEffectors[Effector.DEF]);
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
                            if (defenderTypes == advEType) mod *= 2f;
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
