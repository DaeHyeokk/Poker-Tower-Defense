using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuckyDayMission : NonRepeatMission
{ 
    private TowerBuilder _towerBuilder;
    private WaveSystem _waveSystem;
    private readonly string _missionCompletionString = "<color=\"white\">��� ������</color>\n";

    protected override string missionCompletionString => _missionCompletionString;

    protected override void Awake()
    {
        base.Awake();
        _towerBuilder = FindObjectOfType<TowerBuilder>();
        _waveSystem = FindObjectOfType<WaveSystem>();
    }

    protected override void Update()
    {
        // �̼��� �̹� �����ٸ� �������� �ʴ´�.
        if (isEnd) return;

        // 4���̺꿡 �������� ��� �̼� ����.
        if(_waveSystem.wave == 4)
        {
            FailedMission();
            return;
        }

        int towerCount = 0;
        LinkedListNode<Tower> towerListNode = _towerBuilder.towerList.First;

        while(towerListNode != null)
        {
            // ���� ���� Ž������ Ÿ���� ��Ʈ����ƮŸ�� �̻��� ����̶�� towerCount 1 ����. 
            if (towerListNode.Value.towerIndex >= (int)TowerBuilder.towerTypeEnum.��Ʈ����ƮŸ��)
                towerCount++;

            towerListNode = towerListNode.Next;
        }

        // ��Ʈ����ƮŸ�� �̻��� ����� Ÿ���� 3�� �̻� ���� ��� �̼� Ŭ����.
        if (towerCount >= 3)
            CompleteMission();
    }
}
