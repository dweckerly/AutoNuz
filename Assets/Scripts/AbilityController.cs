using System.Collections;
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

    public void AddEffectToPlayer(Critter playerCritter, Effector stat, float amount)
    {
        AddEffectToCritter(playerCritter, stat, amount, PlayerStatusDisplay);
    }

    public void AddEffectToOpponent(Critter opponentCritter, Effector stat, float amount)
    {
        AddEffectToCritter(opponentCritter, stat, amount, OpponentStatusDisplay);
    }

    public void AddEffectToCritter(Critter critter, Effector stat, float amount, GameObject display)
    {
        critter.SetBattleEffector(stat, amount);
        GameObject effectDisplayItem = Instantiate(StatusEffectItem, display.transform);
        StatusEffectItem statusEffectItem = effectDisplayItem.GetComponent<StatusEffectItem>();
        foreach (EffectorSpriteMap esm in effectorSpriteMaps)
        {
            if (stat == esm.stat)
            {
                statusEffectItem.image.sprite = esm.sprite;
                statusEffectItem.percentAmount.text = amount.ToString() + "%";
            }
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

    public void CheckAbilityTriggerOnHit(Critter attackingCritter, Critter defendingCritter, Trigger trigger, int damage)
    {
        if (defendingCritter.data.AbilityData.AmountType == AmountType.PercentDamageTaken)
        {
            float amount = (float)damage * (defendingCritter.data.AbilityData.Amount / 100);
            float mod = 1f;
            if (defendingCritter.data.AbilityData.Effect == Effect.Decrease) mod *= -1f;
            amount *= mod;
        }
        if (attackingCritter.data.AbilityData.AmountType == AmountType.PercentDamageDealt)
        {
            float amount = (float)damage * (attackingCritter.data.AbilityData.Amount / 100);
            float mod = 1f;
            if (attackingCritter.data.AbilityData.Effect == Effect.Decrease) mod *= -1f;
            amount *= mod;
        }
    }
}
