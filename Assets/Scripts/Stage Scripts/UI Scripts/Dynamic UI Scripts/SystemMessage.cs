using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SystemMessage : FadeTextObject
{
    public enum MessageType 
    { 
        NotEnoughGold, 
        NotEnoughMineral, 
        NotEnoughChangeChance, 
        NotEnoughJokerCard, 
        AlreadyUsedJokerCard, 
        CompletionColorChange,
        MissingBossPenalty,
        GetJokerCardRewardedAd
    }

    [SerializeField]
    private Movement2D _movement2D;
    [SerializeField]
    private float _penaltyMessagefadeStartDelay;

    private float _fadeStartDelay;

    private readonly string _notEnoughGoldString = "��尡 �����մϴ�.";
    private readonly string _notEnoughMineralString = "�̳׶��� �����մϴ�.";
    private readonly string _notEnoughChangeChanceString = "ī�屳ȯ���� �����մϴ�.";
    private readonly string _notEnoughJokerCard = "��Ŀī�尡 �����մϴ�.";
    private readonly string _alreadyUsedJokerCard = "��Ŀī�带 �̹� ����ϼ̽��ϴ�.";
    private readonly string _completionColorChangeString = "�� ���� �Ϸ�!";
    private readonly string _missingBossPenaltyString = "������ ��ġ�̽��ϴ�.\n���� ���� ȹ���� ���ѵ˴ϴ�.";

    public void Setup(MessageType messageType)
    {
        _movement2D.Move();
        _fadeStartDelay = 0f;

        switch (messageType)
        {
            case SystemMessage.MessageType.NotEnoughGold:
                this.transform.position = Vector3.zero;
                textMeshPro.color = Color.yellow;
                textMeshPro.text = _notEnoughGoldString;
                SoundManager.instance.PlayErrorSound();
                break;

            case SystemMessage.MessageType.NotEnoughMineral:
                this.transform.position = Vector3.zero;
                textMeshPro.color = Color.cyan;
                textMeshPro.text = _notEnoughMineralString;
                SoundManager.instance.PlayErrorSound();
                break;

            case SystemMessage.MessageType.NotEnoughChangeChance:
                this.transform.position = Vector3.zero;
                textMeshPro.color = Color.red;
                textMeshPro.text = _notEnoughChangeChanceString;
                SoundManager.instance.PlayErrorSound();
                break;

            case SystemMessage.MessageType.NotEnoughJokerCard:
                this.transform.position = Vector3.zero;
                textMeshPro.color = Color.red;
                textMeshPro.text = _notEnoughJokerCard;
                SoundManager.instance.PlayErrorSound();
                break;

            case SystemMessage.MessageType.AlreadyUsedJokerCard:
                this.transform.position = Vector3.zero;
                textMeshPro.color = Color.red;
                textMeshPro.text = _alreadyUsedJokerCard;
                SoundManager.instance.PlayErrorSound();
                break;

            case SystemMessage.MessageType.CompletionColorChange:
                this.transform.position = Vector3.zero;
                textMeshPro.color = Color.white;
                textMeshPro.text = _completionColorChangeString;
                break;

            /*
            case SystemMessage.MessageType.CompletionTowerSales:
                this.transform.position = Vector3.zero;
                textMeshPro.color = Color.white;
                textMeshPro.text = _completionTowerSalesString;
                break;
            */

            case SystemMessage.MessageType.MissingBossPenalty:
                _movement2D.Stop();
                _fadeStartDelay = _penaltyMessagefadeStartDelay;
                this.transform.position = new Vector3(0f, 1f, 0f);
                textMeshPro.color = Color.red;
                textMeshPro.text = _missingBossPenaltyString;
                break;
        }
    }

    public override void StartAnimation()
    {
        StartCoroutine(StartAnimationCoroutine());
    }

    private IEnumerator StartAnimationCoroutine()
    {
        while(_fadeStartDelay > 0f)
        {
            yield return null;
            _fadeStartDelay -= Time.unscaledDeltaTime;
        }

        base.textObjectFadeAnimation.FadeOutText();
    }

    protected override void ReturnPool() => StageUIManager.instance.systemMessagePool.ReturnObject(this);
}

/*
 * File : SystemMessage.cs
 * First Update : 2022/06/17 FRI 00:10
 * �ΰ��ӿ��� �÷��̾�� �ȳ� ������ �����ֱ� ���� ��ũ��Ʈ.
 */