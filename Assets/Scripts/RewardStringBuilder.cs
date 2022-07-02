using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class RewardStringBuilder
{
    private StringBuilder _rewardStringBuilder = new();

    public override string ToString() => _rewardStringBuilder.ToString();

    public void Set(int gold, int cardChangeChance, int jokerCard)
    {
        if (_rewardStringBuilder != null)
            _rewardStringBuilder.Clear();

        if (gold > 0)
        {
            _rewardStringBuilder.Append("<color=\"yellow\">");
            _rewardStringBuilder.Append('+');
            _rewardStringBuilder.Append(gold.ToString());
            _rewardStringBuilder.Append('G');
            _rewardStringBuilder.Append("</color>");
        }

        if (cardChangeChance > 0)
        {
            _rewardStringBuilder.Append('\n');
            _rewardStringBuilder.Append("<color=\"green\">");
            _rewardStringBuilder.Append("카드교환권");
            _rewardStringBuilder.Append("<color=\"white\">");
            _rewardStringBuilder.Append('+');
            _rewardStringBuilder.Append(cardChangeChance.ToString());
            _rewardStringBuilder.Append("</color>");
        }

        if (jokerCard > 0)
        {
            _rewardStringBuilder.Append('\n');
            _rewardStringBuilder.Append("<color=\"orange\">");
            _rewardStringBuilder.Append("조커카드");
            _rewardStringBuilder.Append("<color=\"white\">");
            _rewardStringBuilder.Append('+');
            _rewardStringBuilder.Append(jokerCard.ToString());
            _rewardStringBuilder.Append("</color>");
        }
    }
}
