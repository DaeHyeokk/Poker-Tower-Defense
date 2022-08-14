using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuckySevenMission : NonRepeatMission
{
    [SerializeField]
    private MissionTower _missionTower;

    private TowerBuilder _towerBuilder;

    private readonly string _missionCompletionString = "<color=\"white\">럭키 세븐</color>\n";

    protected override string missionCompletionString => _missionCompletionString;

    protected override void Awake()
    {
        base.Awake();

        _towerBuilder = FindObjectOfType<TowerBuilder>();
    }

    protected override void Update()
    {
        // 미션을 이미 완료한 경우 건너뛴다.
        if (isEnd) return;

        int towerCount = 0;
        LinkedListNode<Tower> towerListNode = _towerBuilder.towerList.First;

        while(towerListNode != null)
        {
            // 현재 탐색중인 타워가 미션타워와 동일한 타워라면 towerCount를 1 증가시킨다.
            if (_missionTower.IsCompareTower(towerListNode.Value))
                towerCount++;

            towerListNode = towerListNode.Next;
        }

        // 미션타워와 동일한 타워가 7개 이상일 경우 미션성공.
        if (towerCount >= 7)
            CompleteMission();
    }
}
