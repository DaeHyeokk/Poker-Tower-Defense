using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAttacker : MonoBehaviour
{
    public enum TowerState { SearchTarget, AttackToTarget }

    private TargetDetector _targetDetector;
    private TowerGrade _towerGrade;
    private TowerColor _towerColor;
    private TowerWeapon _towerWeapon;

    private TowerState _towerState;

    public TowerWeapon towerWeapon => _towerWeapon;
    public TowerState towerState
    {
        get => _towerState;
        set => _towerState = value;
    }

    private void Awake()
    {
        _targetDetector = GetComponent<TargetDetector>();
        _towerGrade = GetComponent<TowerGrade>();
        _towerColor = GetComponent<TowerColor>();
        _towerState = TowerState.SearchTarget;
        
        _targetDetector.StartSearchTarget();
    }

    public void ChangeState(TowerState newTowerState)
    {
        if(newTowerState == TowerState.AttackToTarget)
        {
            
            StartCoroutine(AttackToTarget());
        }
    }

    public void StartAttackToTarget() => StartCoroutine(AttackToTarget());

    protected virtual IEnumerator AttackToTarget()
    {
        while (true)
        {
            // target�� �ִ��� �˻� (�ٸ� �߻�ü�� ���� ���� or Goal �������� �̵��� ������ ��)
            if (_targetDetector.attackTarget == null)
            {
                _targetDetector.StartSearchTarget();
                break;
            }

            // target�� ���ݹ��� ���� �ִ��� �˻� (���ݹ����� ����� �Ǹ� ���ο� ���� Ž��)
            float distance = Vector3.Distance(this.transform.position, attackTarget.transform.position);
            if (distance > attackRange)
            {
                _attackTarget = null;
                ChangeState(WeaponState.SearchTarget);
                break;
            }

            // ���ݼӵ� �ð���ŭ ��� �� ����
            yield return new WaitForSeconds(attackRate);

            SpawnProjectile();
        }
    }
}
