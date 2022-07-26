using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class RepeatMission : Mission
{
    [SerializeField]
    private TextMeshProUGUI _completionCountText;

    private int _completionCount;

    protected override void GiveReward()
    {
        base.GiveReward();
        _completionCount++;
        _completionCountText.text = _completionCount.ToString() + "회";
    }
}
