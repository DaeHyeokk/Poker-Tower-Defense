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
 * 인게임에서 플레이어가 획득하는 재화를 보여주기 위한 스크립트.
 */