using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Mission : MonoBehaviour
{
    [SerializeField]
    private int _rewardGold;
    [SerializeField]
    private int _rewardChangeChance;
    [SerializeField]
    private int _rewardJokerCard;

    protected readonly RewardStringBuilder _rewardStringBuilder = new();

    protected abstract string missionCompletionString { get; }

    protected virtual void Awake()
    {
        _rewardStringBuilder.Set(_rewardGold, _rewardChangeChance, _rewardJokerCard);
    }

    protected void GiveReward()
    {
        if (_rewardGold > 0)
            GameManager.instance.gold += _rewardGold;
        if (_rewardChangeChance > 0)
            GameManager.instance.changeChance += _rewardChangeChance;
        if (_rewardJokerCard > 0)
            GameManager.instance.jokerCard += _rewardJokerCard;

        UIManager.instance.ShowMissionRewardText(missionCompletionString + _rewardStringBuilder.ToString());
    }

    public abstract void CheckMission();
}
