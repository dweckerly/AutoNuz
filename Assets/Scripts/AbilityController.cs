using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class EffectorSpriteMap
{
    public Effector stat;
    public Sprite sprite;
}

public class AbilityController : MonoBehaviour
{
    public GameObject PlayerStatusDisplay;
    public GameObject OpponentStatusDisplay;
    public GameObject StatusEffectItem;
    public List<EffectorSpriteMap> effectorSpriteMaps = new List<EffectorSpriteMap>();

    List<GameObject> PlayerEffectDisplays = new List<GameObject>();
    List<GameObject> OpponentEffectDisplays = new List<GameObject>();

    public event Action OnCritterHPCHange;

    public void AddEffectToPlayer(Critter playerCritter, Effector stat, float amount)
    {
        AddEffectToCritter(playerCritter, stat, amount, PlayerStatusDisplay, PlayerEffectDisplays);
    }

    public void AddEffectToOpponent(Critter opponentCritter, Effector stat, float amount)
    {
        AddEffectToCritter(opponentCritter, stat, amount, OpponentStatusDisplay, OpponentEffectDisplays);
    }

    public void AddEffectToCritter(Critter critter, Effector stat, float amount, GameObject display, List<GameObject> displayRefList)
    {
        bool existing = critter.SetBattleEffector(stat, amount);
        if (!existing)
        {
            GameObject effectDisplayItem = Instantiate(StatusEffectItem, display.transform);
            displayRefList.Add(effectDisplayItem);
            StatusEffectItem statusEffectItem = effectDisplayItem.GetComponent<StatusEffectItem>();
            foreach (EffectorSpriteMap esm in effectorSpriteMaps)
            {
                if (stat == esm.stat)
                {
                    statusEffectItem.stat = stat;
                    statusEffectItem.image.sprite = esm.sprite;
                    statusEffectItem.percentAmount.text = amount.ToString() + "%";
                }
            }
            return;
        }
        foreach(GameObject go in displayRefList)
        {
            StatusEffectItem statusEffectItem = go.GetComponent<StatusEffectItem>();
            if (statusEffectItem.stat == stat) statusEffectItem.percentAmount.text = ((critter.battleEffectors[stat] - 1) * 100) + "%";
        }
    }

    public void CheckAbilityTrigger(Critter playerCritter, Critter opponentCritter, Trigger trigger)
    {
        if (playerCritter.data.AbilityData.Trigger == trigger)
        {
            float mod = 1f;
            if (playerCritter.data.AbilityData.Effect == Effect.Decrease) mod *= -1f;
            if (playerCritter.data.AbilityData.Target == Target.Self)
                AddEffectToPlayer(playerCritter, playerCritter.data.AbilityData.Effector, playerCritter.data.AbilityData.Amount * mod);
            if (playerCritter.data.AbilityData.Target == Target.Opponent)
                AddEffectToOpponent(opponentCritter, playerCritter.data.AbilityData.Effector, playerCritter.data.AbilityData.Amount * mod);
        }
        if (opponentCritter.data.AbilityData.Trigger == trigger)
        {
            float mod = 1f;
            if (opponentCritter.data.AbilityData.Effect == Effect.Decrease) mod *= -1f;
            if (opponentCritter.data.AbilityData.Target == Target.Self)
                AddEffectToOpponent(opponentCritter, opponentCritter.data.AbilityData.Effector, opponentCritter.data.AbilityData.Amount * mod);
            if (opponentCritter.data.AbilityData.Target == Target.Opponent)
                AddEffectToPlayer(playerCritter, opponentCritter.data.AbilityData.Effector, opponentCritter.data.AbilityData.Amount * mod);
        }
    }

    public void CheckAbilityTriggerPlayerAttacking(Critter playerCritter, Critter opponentCritter, int damage)
    {
        CheckAbilityTriggerOnHit(playerCritter, opponentCritter, damage, PlayerStatusDisplay, PlayerEffectDisplays, OpponentStatusDisplay, OpponentEffectDisplays);
    }

    public void CheckAbilityTriggerOpponentAttacking(Critter playerCritter, Critter opponentCritter, int damage)
    {
        CheckAbilityTriggerOnHit(opponentCritter, playerCritter, damage, OpponentStatusDisplay, OpponentEffectDisplays, PlayerStatusDisplay, PlayerEffectDisplays);
    }

    public void CheckAbilityTriggerOnHit(Critter attackingCritter, Critter defendingCritter, int damage, GameObject attackerDisplay, List<GameObject> attackerDisplayRefList, GameObject defenderDisplay, List<GameObject> defenderDisplayRefList)
    {
        if (defendingCritter.data.AbilityData.Trigger == Trigger.OnTakeDamage)
        {
            float amount = defendingCritter.data.AbilityData.Amount;
            float mod = 1f;
            if (defendingCritter.data.AbilityData.Effect == Effect.Decrease) mod *= -1f;
            amount *= mod;
            if (defendingCritter.data.AbilityData.Target == Target.Self)
            {
                if (defendingCritter.data.AbilityData.Effector == Effector.HP)
                {
                    if (defendingCritter.data.AbilityData.AmountType == AmountType.PercentDamageTaken)
                    {
                        amount = (float)damage * (defendingCritter.data.AbilityData.Amount / 100);
                        defendingCritter.currentHp += Mathf.RoundToInt(amount);
                        OnCritterHPCHange?.Invoke();
                    }
                    if (defendingCritter.data.AbilityData.AmountType == AmountType.Percent)
                    {
                        defendingCritter.currentHp += Mathf.RoundToInt((amount / 100) * defendingCritter.Hp);
                        OnCritterHPCHange?.Invoke();
                    }
                }
                else
                {
                    AddEffectToCritter(defendingCritter, defendingCritter.data.AbilityData.Effector, amount, defenderDisplay, defenderDisplayRefList);
                }
            }
            else
            {
                if (defendingCritter.data.AbilityData.Effector == Effector.HP)
                {
                    if (defendingCritter.data.AbilityData.AmountType == AmountType.PercentDamageTaken)
                    {
                        amount = (float)damage * (defendingCritter.data.AbilityData.Amount / 100);
                        attackingCritter.currentHp += Mathf.RoundToInt(amount);
                        OnCritterHPCHange?.Invoke();
                    }
                    if (defendingCritter.data.AbilityData.AmountType == AmountType.Percent)
                    {
                        attackingCritter.currentHp += Mathf.RoundToInt((amount / 100) * defendingCritter.Hp);
                        OnCritterHPCHange?.Invoke();
                    }
                }
                else
                {
                    AddEffectToCritter(attackingCritter, defendingCritter.data.AbilityData.Effector, amount, attackerDisplay, attackerDisplayRefList);
                }
            }
            
        }
        if (attackingCritter.data.AbilityData.Trigger == Trigger.OnDealDamage)
        {
            float amount = attackingCritter.data.AbilityData.Amount;
            float mod = 1f;
            if (attackingCritter.data.AbilityData.Effect == Effect.Decrease) mod *= -1f;
            amount *= mod;
            if (attackingCritter.data.AbilityData.Target == Target.Self)
            {
                if (attackingCritter.data.AbilityData.Effector == Effector.HP)
                {
                    if (attackingCritter.data.AbilityData.AmountType == AmountType.PercentDamageDealt)
                    {
                        amount = (float)damage * (amount / 100);
                        attackingCritter.currentHp += Mathf.RoundToInt(amount);
                        OnCritterHPCHange?.Invoke();
                    }
                    if (attackingCritter.data.AbilityData.AmountType == AmountType.Percent)
                    {
                        attackingCritter.currentHp += Mathf.RoundToInt((amount / 100) * attackingCritter.Hp);
                        OnCritterHPCHange?.Invoke();
                    }
                }
                else
                {
                    AddEffectToCritter(attackingCritter, attackingCritter.data.AbilityData.Effector, amount, attackerDisplay, attackerDisplayRefList);
                }
            }
            else
            {
                if (attackingCritter.data.AbilityData.Effector == Effector.HP)
                {
                    if (attackingCritter.data.AbilityData.AmountType == AmountType.PercentDamageDealt)
                    {
                        amount = (float)damage * (amount / 100);
                        defendingCritter.currentHp += Mathf.RoundToInt(amount);
                        OnCritterHPCHange?.Invoke();
                    }
                    if (attackingCritter.data.AbilityData.AmountType == AmountType.Percent)
                    {
                        defendingCritter.currentHp += Mathf.RoundToInt((amount / 100) * attackingCritter.Hp);
                        OnCritterHPCHange?.Invoke();
                    }   
                }
                else
                {
                    AddEffectToCritter(defendingCritter, attackingCritter.data.AbilityData.Effector, amount, defenderDisplay, defenderDisplayRefList);
                }
            }
        }
    }

    public void DestroyPlayerEffectDisplayItems()
    {
        for (int i = PlayerEffectDisplays.Count - 1; i > -1; i--)
        {
            Destroy(PlayerEffectDisplays[i]);
        }
        PlayerEffectDisplays.Clear();
    }

    public void DestroyOpponentEffectDisplayItems()
    {
        for (int i = OpponentEffectDisplays.Count - 1; i > -1; i--)
        {
            Destroy(OpponentEffectDisplays[i]);
        }
        OpponentEffectDisplays.Clear();
    }
}
