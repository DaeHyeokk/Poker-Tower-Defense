using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SystemMessage : FadeTextUI
{
    public enum MessageType { NotEnoughGold, NotEnoughMineral, NotEnoughChangeChance, NotEnoughJokerCard, AlreadyUsedJokerCard, CompletionColorChange, CompletionTowerSales }

    private readonly WaitForSecondsRealtime _waitForSecondsRealtime = new(0.1f);

    private readonly string _notEnoughGoldString = "��尡 �����մϴ�.";
    private readonly string _notEnoughMineralString = "�̳׶��� �����մϴ�.";
    private readonly string _notEnoughChangeChanceString = "ī�屳ȯ���� �����մϴ�.";
    private readonly string _notEnoughJokerCard = "��Ŀī�尡 �����մϴ�.";
    private readonly string _alreadyUsedJokerCard = "��Ŀī�带 �̹� ����ϼ̽��ϴ�.";
    private readonly string _completionColorChangeString = "�� ���� �Ϸ�!";
    private readonly string _completionTowerSalesString = "�Ǹ� �Ϸ�!";

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
 * �ΰ��ӿ��� �÷��̾�� �ȳ� ������ �����ֱ� ���� ��ũ��Ʈ.
 */