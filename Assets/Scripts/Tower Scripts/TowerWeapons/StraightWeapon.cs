using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightWeapon : TowerWeapon
{
    public override string weaponName => "Straight";
    public override void OnSkill()
    {
        return;
    }
}


/*
 * File : StraightTower.cs
 * First Update : 2022/04/26 TUE 10:50
 * �߻�Ŭ���� TowerType�� ��ӹ޾� �߻�޼��带 �����ϴ� ������Ʈ
 * 
 * Update : 2022/04/30 SAT
 * TowerWeapon ���� �����丵�� �����ϸ鼭 ���� Tower���� Weapon���� �̸� ����
 */