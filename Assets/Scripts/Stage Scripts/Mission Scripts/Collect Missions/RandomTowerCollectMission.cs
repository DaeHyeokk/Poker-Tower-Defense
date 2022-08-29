using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomTowerCollectMission : NonRepeatMission
{
    [SerializeField]
    private MissionTower[] _missionTowers;

    private TowerBuilder _towerBuilder;
    private bool[] _isCollectedes;
    private string _missionCompletionString;

    protected override string missionCompletionString => _missionCompletionString;

    protected override void Awake()
    {
        base.Awake();

        _towerBuilder = FindObjectOfType<TowerBuilder>();
        _isCollectedes = new bool[_missionTowers.Length];

        SetMissionCompletionString();
    }

    protected override void Update()
    {
        // �̼��� �̹� �Ϸ��ߴٸ� �ǳʶڴ�.
        if (isEnd) return;

        ResetIsCollectedes();

        LinkedListNode<Tower> towerListNode = _towerBuilder.towerList.First;

        while (towerListNode != null)
        {
            for (int i = 0; i < _missionTowers.Length; i++)
            {
                // ���� Ž������ Ÿ���� �̹� ������ ���°� �ƴϰ�,
                // �̼�Ÿ���� �����ϴٸ� ������ ������ �ٲٰ� �ݺ����� �����Ѵ�. (�̼�Ÿ���� �ߺ��Ǵ� ��� �ߺ�üũ ����)
                if (!_isCollectedes[i] && _missionTowers[i].IsCompareTower(towerListNode.Value))
                {
                    _isCollectedes[i] = true;
                    break;
                }
            }

            towerListNode = towerListNode.Next;
        }

        // �������� ���� Ÿ���� ���� ��� ������ �����Ѵ�.
        for (int i = 0; i < _isCollectedes.Length; i++)
            if (!_isCollectedes[i]) return;

        // ��� �̼�Ÿ���� ������ ��� �̼� ����.
        CompleteMission();
    }

    private void SetMissionCompletionString()
    {
        switch(_missionTowers.Length)
        {
            case 3:
                _missionCompletionString = "<color=\"white\">3��3�� 3���</color>\n";
                break;

            case 4:
                _missionCompletionString = "<color=\"white\">4��4�� 4��</color>\n";
                break;

            case 5:
                _missionCompletionString = "<color=\"white\">5��5�� 5¡��</color>\n";
                break;
        }
    }

    private void ResetIsCollectedes()
    {
        for (int i = 0; i < _isCollectedes.Length; i++)
            _isCollectedes[i] = false;
    }
}
