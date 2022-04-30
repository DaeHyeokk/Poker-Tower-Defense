using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    private TowerColor _towerColor;
    private TowerLevel _towerLevel;
    private TargetDetector _targetDetector;

    private TowerWeapon _towerWeapon;

    private void Awake()
    {
        _towerColor = GetComponent<TowerColor>();
        _towerLevel = GetComponent<TowerLevel>();
        _targetDetector = GetComponent<TargetDetector>();
    }

    public void DefaultSetup(EnemySpawner enemySpawner)
    {
        _towerWeapon = GetComponentInChildren<TowerWeapon>();
        _targetDetector.DefaultSetup(enemySpawner);
        _towerLevel.DefaultSetup();
        _towerColor.DefaultSetup();
        _towerWeapon.DefaultSetup();

        _targetDetector.StartSearchTarget();
        _towerWeapon.StartActionToTarget();
    }
}


/*
 * File : Tower.cs
 * First Update : 2022/04/30 SAT 23:50
 * 타워 구조 재설계.
 *     => Tower 오브젝트를 새로 정의하고, Tower 오브젝트에 TowerWeapon 오브젝트를 부착해서 사용하는 방식으로 변경. (Has-A 관계)
 *     => 기존 TowerWeapon에서 한번에 수행하던 역할들을 세분화 하여 Tower 오브젝트의 컴포넌트로 구현. (TowerColor, TowerLevel, TargetDetector)
 *     => Tower 오브젝트의 전체적인 동작들을 Tower 클래스를 통해 수행하도록 구현할 예정.
 */