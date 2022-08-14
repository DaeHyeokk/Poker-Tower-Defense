using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuckySevenMission : NonRepeatMission
{
    [SerializeField]
    private MissionTower _missionTower;

    private TowerBuilder _towerBuilder;

    private readonly string _missionCompletionString = "<color=\"white\">��Ű ����</color>\n";

    protected override string missionCompletionString => _missionCompletionString;

    protected override void Awake()
    {
        base.Awake();

        _towerBuilder = FindObjectOfType<TowerBuilder>();
    }

    protected override void Update()
    {
        // �̼��� �̹� �Ϸ��� ��� �ǳʶڴ�.
        if (isEnd) return;

        int towerCount = 0;
        LinkedListNode<Tower> towerListNode = _towerBuilder.towerList.First;

        while(towerListNode != null)
        {
            // ���� Ž������ Ÿ���� �̼�Ÿ���� ������ Ÿ����� towerCount�� 1 ������Ų��.
            if (_missionTower.IsCompareTower(towerListNode.Value))
                towerCount++;

            towerListNode = towerListNode.Next;
        }

        // �̼�Ÿ���� ������ Ÿ���� 7�� �̻��� ��� �̼Ǽ���.
        if (towerCount >= 7)
            CompleteMission();
    }
}
