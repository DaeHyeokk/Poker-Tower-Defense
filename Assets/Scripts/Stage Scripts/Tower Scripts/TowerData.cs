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
        public int salesGold; // �Ǹ� ����
        public int maxTargetCount;

        public float damage;
        public float rate;
        public float range;

        public float upgradeDIP;    // DIP : ���׷��̵� �� ������ ������
        public float upgradeRIP;    // SIP : ���׷��̵� �� ���ݼӵ� ������

    }

    [System.Serializable]
    public struct Levelup
    {
        public float damage;
        public float rate;

        public float upgradeDIP;    // DIP : ���׷��̵� �� ������ ������
        public float upgradeRIP;    // SIP : ���׷��̵� �� ���ݼӵ� ������

    }
}


/*
 * File : WeaponData.cs
 * First Update : 2022/04/25 MON 06:45
 * ������Ʈ�� ���Ǵ� Ÿ������ ������ �����ϴ� ������ ��ũ���ͺ�
 * tower prefab�� Ÿ�� ���ݷ�, ���ݼӵ�, ��Ÿ�, ���۴� �����ϴ� ���� �����Ѵ�.
 * 
 * Update : 2022/04/25 MON 10:49
 * Ÿ�� ���ݷ�, ���ݼӵ�, ��Ÿ�, ���۴� �����ϴ� ��ġ ���� Ÿ���� �������� ���� �������ֱ� ����
 * Weapon ����ü�� �迭�� �����Ͽ� �����͸� �����ϵ��� �����Ͽ���.
 * 
 * Update : 2022/04/28 THU 23:40
 * Ÿ�� ������Ʈ�� ������Ʈ Ǯ���� �����ϸ鼭 TowerWeapon Prefab �ʵ带 ������.
 * 
 * Update : 2022/06/11 SAT 22:18
 * salesGold �ʵ� �߰�.
 */