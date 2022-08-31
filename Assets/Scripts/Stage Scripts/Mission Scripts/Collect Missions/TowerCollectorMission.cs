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
        // �̼��� �̹� �Ϸ��� ��� �ǳʶڴ�.
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
        // �ش� �÷��� �̼��� �̹� �Ϸ��ߴٸ� �ǳʶڴ�.
        if (_isMissionCompletedes[(int)colorType]) return;

        for(int i=0; i<_checkTowerCollectedes[(int)colorType].Length; i++)
        {
            // ���� �������� ���� Ÿ���� �ִٸ� Ž���� �����Ѵ�.
            if (!_checkTowerCollectedes[(int)colorType][i])
                return;
        }

        CompleteMission(colorType);
    }

    private void CompleteMission(TowerColor.ColorType colorType)
    {
        // �Ϸ��� �÷��� ���� ���ڻ��� �ٸ��� ���.
        switch (colorType)
        {
            case TowerColor.ColorType.Red:
                _missionCompletionString = "<color=\"red\">Ÿ�� ������</color>\n";
                break;

            case TowerColor.ColorType.Green:
                _missionCompletionString = "<color=\"green\">Ÿ�� ������</color>\n";
                break;

            case TowerColor.ColorType.Blue:
                _missionCompletionString = "<color=\"blue\">Ÿ�� ������</color>\n";
                break;
        }

        GiveReward();
        _missionCompleteTexts[(int)colorType].SetActive(true);
        _isMissionCompletedes[(int)colorType] = true;

        // ��� ���� Ÿ���� �̼��� �Ϸ��ߴ��� �˻��ϰ�, ���� �Ϸ����� ���� �̼��� ���� ��� ����.
        for (int i = 0; i < _isMissionCompletedes.Length; i++)
            if (!_isMissionCompletedes[i])
                return;

        // ��� �÷� Ÿ���� �̼��� �Ϸ��ߴٸ� �̼� Ŭ���� �Լ��� �����Ѵ�.
        CompleteMission();
    }

    protected override void CompleteMission()
    {
        completionPanel.SetActive(true);
        missionPanel.transform.SetAsLastSibling();

        isEnd = true;
    }

}
