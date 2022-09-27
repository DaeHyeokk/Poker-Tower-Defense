using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorUpgrade : MonoBehaviour
{
    [SerializeField]
    private ColorUpgradeUIController _colorUpgradeUIController;

    private int[] _colorUpgradeIncrementCost;
    private int[] _colorUpgradeCounts;
    private int[] _colorUpgradeCosts;

    public int[] colorUpgradeCounts => _colorUpgradeCounts;

    private void Awake()
    {
        _colorUpgradeIncrementCost = new int[3];
        _colorUpgradeCounts = new int[3];
        _colorUpgradeCosts = new int[3];

        for (int i = 0; i < 3; i++)
        {
            _colorUpgradeIncrementCost[i] = 1;
            SetColorUpgradeCount(i, 0);
            SetColorUpgradeCost(i, 5);
        }
    }

    public void UpgradeColor(int index)
    {
        if (_colorUpgradeCosts[index] > StageManager.instance.mineral)
        {
            StageUIManager.instance.ShowSystemMessage(SystemMessage.MessageType.NotEnoughMineral);
            return;
        }

        SoundManager.instance.PlaySFX(SoundFileNameDictionary.colorUpgradeSound);
        StageManager.instance.mineral -= _colorUpgradeCosts[index];

        //SetColorUpgradeCost(index, _colorUpgradeCosts[index] + _colorUpgradeCounts[index]);
        SetColorUpgradeCost(index, _colorUpgradeCosts[index] + 5);
        SetColorUpgradeCount(index, _colorUpgradeCounts[index] + 1);

        _colorUpgradeIncrementCost[index]++;
    }

    private void SetColorUpgradeCount(int index, int value)
    {
        _colorUpgradeCounts[index] = value;
        _colorUpgradeUIController.SetColorUpgradeCountText(index, _colorUpgradeCounts[index]);
    }

    private void SetColorUpgradeCost(int index, int value)
    {
        _colorUpgradeCosts[index] = value;
        _colorUpgradeUIController.SetColorUpgradeCostText(index, _colorUpgradeCosts[index]);
    }
}
