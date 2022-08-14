using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuckyDayMission : NonRepeatMission
{ 
    private TowerBuilder _towerBuilder;
    private WaveSystem _waveSystem;
    private readonly string _missionCompletionString = "<color=\"white\">운수 좋은날</color>\n";

    protected override string missionCompletionString => _missionCompletionString;

    protected override void Awake()
    {
        base.Awake();
        _towerBuilder = FindObjectOfType<TowerBuilder>();
        _waveSystem = FindObjectOfType<WaveSystem>();
    }

    protected override void Update()
    {
        // 미션이 이미 끝났다면 수행하지 않는다.
        if (isEnd) return;

        // 4웨이브에 도달했을 경우 미션 실패.
        if(_waveSystem.wave == 4)
        {
            FailedMission();
            return;
        }

        int towerCount = 0;
        LinkedListNode<Tower> towerListNode = _towerBuilder.towerList.First;

        while(towerListNode != null)
        {
            // 만약 현재 탐색중인 타워가 스트레이트타워 이상의 등급이라면 towerCount 1 증가. 
            if (towerListNode.Value.towerIndex >= (int)TowerBuilder.towerTypeEnum.스트레이트타워)
                towerCount++;

            towerListNode = towerListNode.Next;
        }

        // 스트레이트타워 이상의 등급인 타워가 3개 이상 있을 경우 미션 클리어.
        if (towerCount >= 3)
            CompleteMission();
    }
}
