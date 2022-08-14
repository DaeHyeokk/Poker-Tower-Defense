using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomFourTowerCollectMission : NonRepeatMission
{
    [SerializeField]
    private MissionTower[] _missionTowers;

    private TowerBuilder _towerBuilder;
    private bool[] _isCollectedes;
    private readonly string _missionCompletionString = "<color=\"white\">4각4각 4과</color>\n";

    protected override string missionCompletionString => _missionCompletionString;

    protected override void Awake()
    {
        base.Awake();

        _towerBuilder = FindObjectOfType<TowerBuilder>();
        _isCollectedes = new bool[_missionTowers.Length];
    }

    protected override void Update()
    {
        // 미션을 이미 완료했다면 건너뛴다.
        if (isEnd) return;

        ResetIsCollectedes();

        LinkedListNode<Tower> towerListNode = _towerBuilder.towerList.First;

        while (towerListNode != null)
        {
            for (int i = 0; i < _missionTowers.Length; i++)
            {
                // 현재 탐색중인 타워가 이미 수집된 상태가 아니고,
                // 미션타워와 동일하다면 수집된 것으로 바꾸고 반복문을 종료한다. (미션타워가 중복되는 경우 중복체크 방지)
                if (!_isCollectedes[i] && _missionTowers[i].IsCompareTower(towerListNode.Value))
                {
                    _isCollectedes[i] = true;
                    break;
                }
            }

            towerListNode = towerListNode.Next;
        }

        // 수집하지 못한 타워가 있을 경우 수행을 종료한다.
        for (int i = 0; i < _isCollectedes.Length; i++)
            if (!_isCollectedes[i]) return;

        // 모든 미션타워를 수집한 경우 미션 성공.
        CompleteMission();
    }

    private void ResetIsCollectedes()
    {
        for (int i = 0; i < _isCollectedes.Length; i++)
            _isCollectedes[i] = false;
    }
}
