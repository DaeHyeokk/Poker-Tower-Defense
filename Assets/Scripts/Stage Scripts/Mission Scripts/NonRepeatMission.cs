using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class NonRepeatMission : Mission
{
    [SerializeField]
    private GameObject _missionPanel;
    [SerializeField]
    private GameObject _completionPanel;

    protected GameObject missionPanel => _missionPanel;
    protected GameObject completionPanel => _completionPanel;
    protected bool isEnd { get; set; }

    protected override void CompleteMission()
    {
        GiveReward();

        _completionPanel.SetActive(true);
        _missionPanel.transform.SetAsLastSibling();
        isEnd = true;
    }

    protected void FailedMission()
    {
        _completionPanel.SetActive(true);
        _missionPanel.transform.SetAsLastSibling();

        TextMeshProUGUI completionText = _completionPanel.GetComponentInChildren<TextMeshProUGUI>();
        completionText.text = "미션 실패";
        completionText.color = Color.red;

        isEnd = true;
    }
}
