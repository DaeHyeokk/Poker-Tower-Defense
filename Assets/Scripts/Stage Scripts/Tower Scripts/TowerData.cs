using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/TowerData", fileName = "Tower Data")]
public class TowerData : ScriptableObject
{
    public Sprite[] normalProjectileSprites;
    public Sprite[] specialProjectileSprites;
    public Weapon[] weapons;
    public Levelup levelup;

    [System.Serializable]
    public struct Weapon
    {
        public int salesGold; // 판매 가격
        public int maxTargetCount;

        public float damage;
        public float rate;
        public float range;

        public float upgradeDIP;    // DIP : 업그레이드 시 데미지 증가량
        public float upgradeRIP;    // SIP : 업그레이드 시 공격속도 증가량

    }

    [System.Serializable]
    public struct Levelup
    {
        public float damage;
        public float rate;

        public float upgradeDIP;    // DIP : 업그레이드 시 데미지 증가량
        public float upgradeRIP;    // SIP : 업그레이드 시 공격속도 증가량

    }
}


/*
 * File : WeaponData.cs
 * First Update : 2022/04/25 MON 06:45
 * 프로젝트에 사용되는 타워들의 정보를 저장하는 데이터 스크립터블
 * tower prefab과 타워 공격력, 공격속도, 사거리, 업글당 증가하는 값을 저장한다.
 * 
 * Update : 2022/04/25 MON 10:49
 * 타워 공격력, 공격속도, 사거리, 업글당 증가하는 수치 값을 타워의 레벨마다 각각 지정해주기 위해
 * Weapon 구조체를 배열로 선언하여 데이터를 저장하도록 구현하였다.
 * 
 * Update : 2022/04/28 THU 23:40
 * 타워 오브젝트의 오브젝트 풀링을 구현하면서 TowerWeapon Prefab 필드를 제거함.
 * 
 * Update : 2022/06/11 SAT 22:18
 * salesGold 필드 추가.
 */