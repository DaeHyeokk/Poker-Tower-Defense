using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TowerColorChanger : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _colorChangeCostText;

    private Tower _tower;
    private int[] _changeCosts = new int[4] { 50, 100, 200, 400 };

    public void Setup(Tower tower)
    {
        _tower = tower;
        _colorChangeCostText.text = _changeCosts[tower.level].ToString() + 'G';
    }

    public void ChangeColor()
    {
        if(StageManager.instance.gold < _changeCosts[_tower.level])
        {
            StageUIManager.instance.ShowSystemMessage(SystemMessage.MessageType.NotEnoughGold);
            return;
        }

        StageManager.instance.gold -= _changeCosts[_tower.level];
        _tower.towerColor.ChangeRandomColor();
        StageUIManager.instance.ShowSystemMessage(SystemMessage.MessageType.CompletionColorChange);
        SoundManager.instance.PlaySFX(SoundFileNameDictionary.towerColorChangeSound);
    }
}
