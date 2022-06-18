using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyDieRewardText : FadeText
{
    public override void StartAnimation()
    {
        base.textFadeAnimation.FadeOutText();
    }

    protected override void ReturnPool() => UIManager.instance.enemyDieRewardTextPool.ReturnObject(this);

}

/*
 * File : EnemyDieGoldText.cs
 * First Update : 2022/06/17 FRI 00:10
 * 인게임에서 적이 죽을 때마다 획득하는 골드를 나타내기 위한 스크립트.
 */