using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyDieRewardText : FadeTextObject
{
    public override void StartAnimation() => base.textObjectFadeAnimation.FadeOutText();

    protected override void ReturnPool() => UIManager.instance.enemyDieRewardTextPool.ReturnObject(this);

}

/*
 * File : EnemyDieGoldText.cs
 * First Update : 2022/06/17 FRI 00:10
 * �ΰ��ӿ��� ���� ���� ������ ȹ���ϴ� ��带 ��Ÿ���� ���� ��ũ��Ʈ.
 */