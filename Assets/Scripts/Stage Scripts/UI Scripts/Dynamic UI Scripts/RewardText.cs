using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RewardText : FadeTextObject
{
    [SerializeField]
    private Movement2D _movement2D;

    public Movement2D movement2D => _movement2D;

    public override void StartAnimation() => base.textObjectFadeAnimation.FadeOutText();

    protected override void ReturnPool() => StageUIManager.instance.ReturnMissionRewardText(this);

}

/*
 * File : EnemyDieGoldText.cs
 * First Update : 2022/06/17 FRI 00:10
 * �ΰ��ӿ��� �÷��̾ ȹ���ϴ� ��ȭ�� �����ֱ� ���� ��ũ��Ʈ.
 */