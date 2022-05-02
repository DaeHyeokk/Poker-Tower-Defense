using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField]
    private Transform _attackRangeUI;
    private TowerColor _towerColor;
    private TowerLevel _towerLevel;
    private TargetDetector _targetDetector;
    private ObjectFollowMousePosition _towerMovement;
    private TowerWeapon _towerWeapon;

    private void Awake()
    {
        _towerColor = GetComponent<TowerColor>();
        _towerLevel = GetComponent<TowerLevel>();
        _targetDetector = GetComponent<TargetDetector>();
        _towerMovement = GetComponent<ObjectFollowMousePosition>();
    }

    public void DefaultSetup(EnemySpawner enemySpawner, ProjectileSpawner projectileSpawner)
    {
        _towerWeapon = GetComponentInChildren<TowerWeapon>();

        _targetDetector.DefaultSetup(enemySpawner);
        _towerLevel.DefaultSetup();
        _towerColor.DefaultSetup();
        _towerWeapon.DefaultSetup(projectileSpawner);

        _targetDetector.SetAttackRangeUIScale();
        _targetDetector.StartSearchTarget();
        _towerWeapon.StartActionToTarget();
    }

    public void MoveTower()
    {
        _towerMovement.StartFollowMousePosition();
        _targetDetector.attackRangeUI.gameObject.SetActive(true);
    }

    public void StopTower()
    {
        _towerMovement.StopFollowMousePosition();
        _targetDetector.attackRangeUI.gameObject.SetActive(false);
    }
}


/*
 * File : Tower.cs
 * First Update : 2022/04/30 SAT 23:50
 * Ÿ�� ���� �缳��.
 *     => Tower ������Ʈ�� ���� �����ϰ�, Tower ������Ʈ�� TowerWeapon ������Ʈ�� �����ؼ� ����ϴ� ������� ����. (Has-A ����)
 *     => ���� TowerWeapon���� �ѹ��� �����ϴ� ���ҵ��� ����ȭ �Ͽ� Tower ������Ʈ�� ������Ʈ�� ����. (TowerColor, TowerLevel, TargetDetector)
 *     => Tower ������Ʈ�� ��ü���� ���۵��� Tower Ŭ������ ���� �����ϵ��� ������ ����.
 *     
 * Update : 2022/05/03 03:10
 * Ÿ���� ���콺 �巡�׿� ���� �����̵��� ObjectFollowMousePosition ������Ʈ�� �����ϴ� ���� ����.
 * Ÿ���� �巡�׷� ������ �� Ÿ���� ��Ÿ��� ǥ�õǵ��� TargetDetector ������Ʈ�� �����ϴ� ���� ����.
 * 
 */