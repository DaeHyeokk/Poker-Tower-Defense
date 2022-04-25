using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/TowerData", fileName = "Tower Data")]
public class TowerData : ScriptableObject
{
    public GameObject towerPrefab;
    public Weapon[] weapons;

    [System.Serializable]
    public struct Weapon
    {
        public float damage;
        public float rate;
        public float range;

        public float upgradeDIP;    // DIP : 업그레이드 시 데미지 증가량
        public float upgradeSIP;    // SIP : 업그레이드 시 공격속도 증가량
        public float upgradeRIP;    // RIP : 업그레이드 시 사거리 증가량
    }
}


/*
 * File : TowerData.cs
 * First Update : 2022/04/25 MON 06:45
 * 프로젝트에 사용되는 타워들의 정보를 저장하는 데이터 스크립터블
 * tower prefab과 타워 공격력, 공격속도, 사거리, 업글당 증가하는 값을 저장한다
 * 
 * Update : 2022/04/25 MON 10:49
 * 타워 공격력, 공격속도, 사거리, 업글당 증가하는 수치 값을 타워의 레벨마다 각각 지정해주기 위해
 * Weapon 구조체를 배열로 선언하여 데이터를 저장하도록 구현하였다.
 */