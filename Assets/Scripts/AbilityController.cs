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
}
