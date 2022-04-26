using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightTower : TowerWeapon
{
    public override string towerName => "Straight Tower";
    public override void OnSkill()
    {
        return;
    }
}


/*
 * File : StraightTower.cs
 * First Update : 2022/04/26 TUE 10:50
 * 추상클래스 TowerType을 상속받아 추상메서드를 구현하는 컴포넌트
 */