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
            _stringBuilder.Append("ī�屳ȯ��");
            _stringBuilder.Append("<color=\"white\">");
            _stringBuilder.Append('+');
            _stringBuilder.Append(cardChangeChance.ToString());
            _stringBuilder.Append("</color>");
        }

        if(towerLevel != -1)
        {
            string towerLevelString;

            if (towerLevel == 0)
                towerLevelString = "0��";
            else if (towerLevel == 1)
                towerLevelString = "1��";
            else if (towerLevel == 2)
                towerLevelString = "2��";
            else if (towerLevel == 3)
                towerLevelString = "3��";
            else
                throw new System.Exception("�������� �����ϴ� Ÿ���� ���� ������ �ʰ��Ͽ����ϴ�.");

            _stringBuilder.Append('\n');
            _stringBuilder.Append("<color=\"orange\">");
            _stringBuilder.Append(towerLevelString);
            _stringBuilder.Append(" ����Ÿ�� +1");
            _stringBuilder.Append("</color>");
        }
    }
}
