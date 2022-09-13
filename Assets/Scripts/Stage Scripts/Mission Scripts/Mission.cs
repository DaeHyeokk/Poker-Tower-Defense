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

        _rewardGoldText.text = _rewardGold.ToString() + 'G';
        _rewardChangeChanceText.text = _rewardChangeChance.ToString();
        _rewardJokerCardText.text = _rewardJokerCard.ToString();
    }

    protected void GiveReward()
    {
        if (_rewardGold > 0)
            StageManager.instance.gold += _rewardGold;
        if (_rewardChangeChance > 0)
            StageManager.instance.changeChance += _rewardChangeChance;
        if (_rewardJokerCard > 0)
            StageManager.instance.jokerCard += _rewardJokerCard;

        StageUIManager.instance.reservateMissionReward("<color=\"red\">미션클리어!</color>\n" + missionCompletionString + _rewardStringBuilder.ToString());
    }

    protected abstract void Update();
    protected abstract void CompleteMission();
}
