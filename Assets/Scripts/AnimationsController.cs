using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationsController : MonoBehaviour
{
    public Animator BattlePlayerCritterAnimator;
    public Animator BattleOpponentCritterAnimator;

    public void BattleStart() 
    {
        BattlePlayerCritterAnimator.speed = 1f;
        BattleOpponentCritterAnimator.speed = 1f;
        float playerOffset = Random.Range(0, 1f);
        float opponentOffset = Random.Range(0, 1f);
        BattlePlayerCritterAnimator.Play("Idle", 0, playerOffset);
        BattleOpponentCritterAnimator.Play("Idle", 0, opponentOffset);
    }

    public void PlayerAttack()
    {
        BattlePlayerCritterAnimator.SetTrigger("Attack");
    }

    public void OpponentAttack()
    {
        BattleOpponentCritterAnimator.SetTrigger("Attack");
    }

    public void SlowPlayerAnimSpeed()
    {
        BattlePlayerCritterAnimator.speed = 0.5f;
    }

    public void SlowOpponentAnimSpeed()
    {
        BattleOpponentCritterAnimator.speed = 0.5f;
    }

}
