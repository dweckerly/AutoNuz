using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AnimationsController : MonoBehaviour
{
    public Animator BattlePlayerCritterAnimator;
    public Animator BattleOpponentCritterAnimator;
    public TMP_Text PlayerNotificationText;
    public TMP_Text OpponentNotificationText;

    public void BattleStart() 
    {
        BattlePlayerCritterAnimator.speed = 1f;
        BattleOpponentCritterAnimator.speed = 1f;
        float playerOffset = Random.Range(0, 1f);
        float opponentOffset = Random.Range(0, 1f);
        BattlePlayerCritterAnimator.Play("BattleIdle", 0, playerOffset);
        BattleOpponentCritterAnimator.Play("BattleIdle", 0, opponentOffset);
    }

    public void PlayerAttack()
    {
        BattlePlayerCritterAnimator.Play("PlayerAttack");
    }

    public void OpponentAttack()
    {
        BattleOpponentCritterAnimator.Play("OpponentAttack");
    }

    public void SlowPlayerAnimSpeed()
    {
        BattlePlayerCritterAnimator.speed = 0.5f;
    }

    public void SlowOpponentAnimSpeed()
    {
        BattleOpponentCritterAnimator.speed = 0.5f;
    }

    public void PlayerMissAnim()
    {
        PlayerNotificationText.text = "Miss";
        BattlePlayerCritterAnimator.Play("Notification", 1);
    }

    public void OpponentMissAnim()
    {
        OpponentNotificationText.text = "Miss";
        BattleOpponentCritterAnimator.Play("Notification", 1);
    }

}
