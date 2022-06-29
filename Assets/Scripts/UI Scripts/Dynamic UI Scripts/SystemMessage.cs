using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SystemMessage : FadeTextUI
{
    public enum MessageType { NotEnoughGold, NotEnoughMineral, NotEnoughChangeChance, NotEnoughJokerCard, AlreadyUsedJokerCard, CompletionColorChange, CompletionTowerSales }

    private readonly WaitForSecondsRealtime _waitForSecondsRealtime = new(0.1f);

    private readonly string _notEnoughGoldString = "골드가 부족합니다.";
    private readonly string _notEnoughMineralString = "미네랄이 부족합니다.";
    private readonly string _notEnoughChangeChanceString = "카드교환권이 부족합니다.";
    private readonly string _notEnoughJokerCard = "조커카드가 부족합니다.";
    private readonly string _alreadyUsedJokerCard = "조커카드를 이미 사용하셨습니다.";
    private readonly string _completionColorChangeString = "색 변경 완료!";
    private readonly string _completionTowerSalesString = "판매 완료!";

    public void Setup(MessageType messageType)
    {
        switch (messageType)
        {
            case SystemMessage.MessageType.NotEnoughGold:
                textMeshProUGUI.color = Color.red;
                textMeshProUGUI.text = _notEnoughGoldString;
                break;

            case SystemMessage.MessageType.NotEnoughMineral:
                textMeshProUGUI.color = Color.red;
                textMeshProUGUI.text = _notEnoughMineralString;
                break;

            case SystemMessage.MessageType.NotEnoughChangeChance:
                textMeshProUGUI.color = Color.red;
                textMeshProUGUI.text = _notEnoughChangeChanceString;
                break;

            case SystemMessage.MessageType.NotEnoughJokerCard:
                textMeshProUGUI.color = Color.red;
                textMeshProUGUI.text = _notEnoughJokerCard;
                break;

            case SystemMessage.MessageType.AlreadyUsedJokerCard:
                textMeshProUGUI.color = Color.red;
                textMeshProUGUI.text = _alreadyUsedJokerCard;
                break;

            case SystemMessage.MessageType.CompletionColorChange:
                textMeshProUGUI.color = Color.white;
                textMeshProUGUI.text = _completionColorChangeString;
                break;

            case SystemMessage.MessageType.CompletionTowerSales:
                textMeshProUGUI.color = Color.white;
                textMeshProUGUI.text = _completionTowerSalesString;
                break;
        }
    }

    public override void StartAnimation()
    {
        StartCoroutine(StartAnimationCoroutine());
    }

    private IEnumerator StartAnimationCoroutine()
    {
        yield return _waitForSecondsRealtime;

        base.textUIFadeAnimation.FadeOutText();
    }
}

/*
 * File : SystemMessage.cs
 * First Update : 2022/06/17 FRI 00:10
 * 인게임에서 플레이어에게 안내 문구를 보여주기 위한 스크립트.
 */