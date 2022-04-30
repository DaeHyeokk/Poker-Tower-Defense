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
 * Ÿ�� ���� �缳��.
 *     => Tower ������Ʈ�� ���� �����ϰ�, Tower ������Ʈ�� TowerWeapon ������Ʈ�� �����ؼ� ����ϴ� ������� ����. (Has-A ����)
 *     => ���� TowerWeapon���� �ѹ��� �����ϴ� ���ҵ��� ����ȭ �Ͽ� Tower ������Ʈ�� ������Ʈ�� ����. (TowerColor, TowerLevel, TargetDetector)
 *     => Tower ������Ʈ�� ��ü���� ���۵��� Tower Ŭ������ ���� �����ϵ��� ������ ����.
 */