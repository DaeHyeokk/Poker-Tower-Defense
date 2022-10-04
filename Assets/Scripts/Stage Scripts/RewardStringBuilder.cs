using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class RewardStringBuilder
{
    private StringBuilder _stringBuilder = new();

    public override string ToString() => _stringBuilder.ToString();

    public void Set(int gold = 0, int cardChangeChance = 0, int towerLevel = -1)
    {
        if (_stringBuilder != null)
            _stringBuilder.Clear();

        if (gold > 0)
        {
            _stringBuilder.Append("<color=\"yellow\">");
            _stringBuilder.Append('+');
            _stringBuilder.Append(gold.ToString());
            _stringBuilder.Append('G');
            _stringBuilder.Append("</color>");
        }

        if (cardChangeChance > 0)
        {
            _stringBuilder.Append('\n');
            _stringBuilder.Append("<color=\"green\">");
            _stringBuilder.Append("카드교환권");
            _stringBuilder.Append("<color=\"white\">");
            _stringBuilder.Append('+');
            _stringBuilder.Append(cardChangeChance.ToString());
            _stringBuilder.Append("</color>");
        }

        if(towerLevel != -1)
        {
            string towerLevelString;

            if (towerLevel == 0)
                towerLevelString = "0성";
            else if (towerLevel == 1)
                towerLevelString = "1성";
            else if (towerLevel == 2)
                towerLevelString = "2성";
            else if (towerLevel == 3)
                towerLevelString = "3성";
            else
                throw new System.Exception("보상으로 지급하는 타워의 레벨 범위를 초과하였습니다.");

            _stringBuilder.Append('\n');
            _stringBuilder.Append("<color=\"orange\">");
            _stringBuilder.Append(towerLevelString);
            _stringBuilder.Append(" 랜덤타워 +1");
            _stringBuilder.Append("</color>");
        }
    }
}
