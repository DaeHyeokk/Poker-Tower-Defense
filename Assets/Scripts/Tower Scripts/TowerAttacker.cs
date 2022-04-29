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
            // target이 있는지 검사 (다른 발사체에 의해 제거 or Goal 지점까지 이동해 삭제됨 등)
            if (_targetDetector.attackTarget == null)
            {
                _targetDetector.StartSearchTarget();
                break;
            }

            // target이 공격범위 내에 있는지 검사 (공격범위를 벗어나게 되면 새로운 적을 탐색)
            float distance = Vector3.Distance(this.transform.position, attackTarget.transform.position);
            if (distance > attackRange)
            {
                _attackTarget = null;
                ChangeState(WeaponState.SearchTarget);
                break;
            }

            // 공격속도 시간만큼 대기 후 공격
            yield return new WaitForSeconds(attackRate);

            SpawnProjectile();
        }
    }
}
