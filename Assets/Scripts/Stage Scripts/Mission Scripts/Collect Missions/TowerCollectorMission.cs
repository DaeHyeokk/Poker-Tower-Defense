using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerCollectorMission : NonRepeatMission
{
    // 0:Red, 1:Green, 2:Blue
    [SerializeField]
    private GameObject[] _missionCompleteTexts;

    private TowerBuilder _towerBuilder;

    // 0:Red, 1:Green, 2:Blue
    private bool[] _isMissionCompletedes = new bool[3];
    private bool[][] _checkTowerCollectedes = new bool[3][];

    private string _missionCompletionString;

    protected override string missionCompletionString => _missionCompletionString;

    protected override void Awake()
    {
        base.Awake();

        _towerBuilder = FindObjectOfType<TowerBuilder>();

        for (int i = 0; i < _checkTowerCollectedes.Length; i++)
            _checkTowerCollectedes[i] = new bool[Tower.towerTypeNames.Length];
    }

    protected override void Update()
    {
        // 미션을 이미 완료한 경우 건너뛴다.
        if (isEnd) return;

        ResetTowerCheck();

        LinkedListNode<Tower> towerListNode = _towerBuilder.towerList.First;

        while(towerListNode != null)
        {
            int towerIndex = towerListNode.Value.towerIndex;
            TowerColor.ColorType towerColorType = towerListNode.Value.towerColor.colorType;

            _checkTowerCollectedes[(int)towerColorType][towerIndex] = true;

            towerListNode = towerListNode.Next;
        }

        CheckCompleteMission(TowerColor.ColorType.Red);
        CheckCompleteMission(TowerColor.ColorType.Green);
        CheckCompleteMission(TowerColor.ColorType.Blue);
    }

    private void ResetTowerCheck()
    {
        for (int i = 0; i < _checkTowerCollectedes.Length; i++)
            for (int j = 0; j < _checkTowerCollectedes[i].Length; j++)
                _checkTowerCollectedes[i][j] = false;
    }

    private void CheckCompleteMission(TowerColor.ColorType colorType)
    {
        // 해당 컬러의 미션을 이미 완료했다면 건너뛴다.
        if (_isMissionCompletedes[(int)colorType]) return;

        for(int i=0; i<_checkTowerCollectedes[(int)colorType].Length; i++)
        {
            // 만약 수집하지 못한 타워가 있다면 탐색을 종료한다.
            if (!_checkTowerCollectedes[(int)colorType][i])
                return;
        }

        CompleteMission(colorType);
    }

    private void CompleteMission(TowerColor.ColorType colorType)
    {
        // 완료한 컬러에 따라 글자색을 다르게 출력.
        switch (colorType)
        {
            case TowerColor.ColorType.Red:
                _missionCompletionString = "<color=\"red\">타워 수집가</color>\n";
                break;

            case TowerColor.ColorType.Green:
                _missionCompletionString = "<color=\"green\">타워 수집가</color>\n";
                break;

            case TowerColor.ColorType.Blue:
                _missionCompletionString = "<color=\"blue\">타워 수집가</color>\n";
                break;
        }

        GiveReward();
        _missionCompleteTexts[(int)colorType].SetActive(true);
        _isMissionCompletedes[(int)colorType] = true;

        // 모든 컬터 타입의 미션을 완료했는지 검사하고, 아직 완료하지 않은 미션이 있을 경우 종료.
        for (int i = 0; i < _isMissionCompletedes.Length; i++)
            if (!_isMissionCompletedes[i])
                return;

        // 모든 컬러 타입의 미션을 완료했다면 미션 클리어 함수를 수행한다.
        CompleteMission();
    }

    protected override void CompleteMission()
    {
        completionPanel.SetActive(true);
        missionPanel.transform.SetAsLastSibling();

        isEnd = true;
    }

}
