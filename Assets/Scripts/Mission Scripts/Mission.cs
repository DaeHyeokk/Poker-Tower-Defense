using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class Mission : MonoBehaviour
{
    [SerializeField]
    private int _rewardGold;
    [SerializeField]
    private int _rewardChangeChance;
    [SerializeField]
    private int _rewardJokerCard;
    [SerializeField]
    private TextMeshProUGUI _rewardGoldText;
    [SerializeField]
    private TextMeshProUGUI _rewardChangeChanceText;
    [SerializeField]
    private TextMeshProUGUI _rewardJokerCardText;

    protected readonly RewardStringBuilder _rewardStringBuilder = new();
    protected abstract string missionCompletionString { get; }

    protected virtual void Awake()
    {
        _rewardStringBuilder.Set(_rewardGold, _rewardChangeChance, _rewardJokerCard);

        _rewardGoldText.text = _rewardGold.ToString();
        _rewardChangeChanceText.text = _rewardChangeChance.ToString();
        _rewardJokerCardText.text = _rewardJokerCard.ToString();
    }

    protected void GiveReward()
    {
        if (_rewardGold > 0)
            GameManager.instance.gold += _rewardGold;
        if (_rewardChangeChance > 0)
            GameManager.instance.changeChance += _rewardChangeChance;
        if (_rewardJokerCard > 0)
            GameManager.instance.jokerCard += _rewardJokerCard;

        UIManager.instance.reservateMissionReward("<color=\"red\">�̼�Ŭ����!</color>\n" + missionCompletionString + _rewardStringBuilder.ToString());
    }

    protected abstract void Update();
    protected abstract void CompleteMission();
}