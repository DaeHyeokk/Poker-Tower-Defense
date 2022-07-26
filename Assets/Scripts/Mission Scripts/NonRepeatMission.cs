using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NonRepeatMission : Mission
{
    [SerializeField]
    private GameObject _missionPanel;
    [SerializeField]
    private GameObject _completionPanel;

    protected bool isCompleted { get; set; }

    protected override void GiveReward()
    {
        base.GiveReward();
        isCompleted = true;
        _completionPanel.SetActive(true);
        _missionPanel.transform.SetAsLastSibling();
    }
}
