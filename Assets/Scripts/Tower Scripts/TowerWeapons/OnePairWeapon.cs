using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnePairWeapon : TowerWeapon
{
    public override string weaponName => "OnePair";

    public override void OnSkill()
    {
        return;
    }
}


/*
 * File : OnePairTower.cs
 * First Update : 2022/04/26 TUE 10:50
 * �߻�Ŭ���� TowerType�� ��ӹ޾� �߻�޼��带 �����ϴ� ������Ʈ
 *
 * Update : 2022/04/30 SAT
 * TowerWeapon ���� �����丵�� �����ϸ鼭 ���� Tower���� Weapon���� �̸� ����
 */