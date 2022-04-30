using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripleWeapon : TowerWeapon
{
    public override string weaponName => "Triple";
    public override void OnSkill()
    {
        return;
    }
}


/*
 * File : TripleTower.cs
 * First Update : 2022/04/26 TUE 10:50
 * 추상클래스 TowerType을 상속받아 추상메서드를 구현하는 컴포넌트
 * 
 * Update : 2022/04/30 SAT
 * TowerWeapon 관련 리팩토링을 진행하면서 기존 Tower에서 Weapon으로 이름 변경
 */