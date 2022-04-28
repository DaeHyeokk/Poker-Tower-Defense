using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public abstract class TowerWeapon : MonoBehaviour
{
    public enum ColorType { Red, Green, Blue }
    public enum WeaponState { SearchTarget, AttackToTarget }

    [SerializeField]
    private GameObject _projectile;
    [SerializeField]
    private Transform _spawnPoint;
    [SerializeField]
    private HorizontalLayoutGroup _gradeLayout;
    private EnemySpawner _enemySpawner;
    private Image[] _gradeImages;
    private Color[] _colorDatas;
    private SpriteRenderer _towerRenderer;
    private TowerData _towerData;
    private int _towerGrade;
    private Transform _attackTarget;
    private ColorType _towerColor;
    private WeaponState _weaponState = WeaponState.SearchTarget;

    public float attackDamage => _towerData.weapons[_towerGrade].damage;
    public float attackRate => _towerData.weapons[_towerGrade].rate;
    public float attackRange => _towerData.weapons[_towerGrade].range;
    public int towerGrade => _towerGrade;
    public ColorType towerColor => _towerColor;
    public WeaponState weaponState => _weaponState;
    public Transform attackTarget => _attackTarget;
    public abstract String towerName { get; }
    // public int killCount { get; private set; }// �̱��� : ���ִ� ų�� ī����. ���� ���� �� �÷��̾� �����Ϳ� �����ϴ� �뵵

    private void Awake()
    {

        _gradeLayout.gameObject.SetActive(false);
        _gradeImages = new Image[3];
        _gradeImages = _gradeLayout.GetComponentsInChildren<Image>(true);
        _towerRenderer = GetComponentInChildren<SpriteRenderer>();

        _colorDatas = new Color[3];

        _colorDatas[0] = new Color(180, 0, 0);
        _colorDatas[1] = new Color(0, 180, 0);
        _colorDatas[2] = new Color(0, 0, 180);
    }

    // Tower�� �ɷ�ġ, ���, ����Ÿ�� ����
    // ó�� �����Ǵ� Ÿ���̹Ƿ� ����� 0���� ����
    public void Setup(TowerData towerData, EnemySpawner enemySpawner)
    {
        _towerData = towerData;
        _towerGrade = 0;
        _attackTarget = null;
        _enemySpawner = enemySpawner;
        ColorSetup();
        ChangeState(WeaponState.SearchTarget);
    }

    // Ÿ���� ���¸� �Ű����� ������ ����
    private void ChangeState(WeaponState newWeaponState)
    {
        // ������ �������̴� ���� ����
        StopCoroutine(_weaponState.ToString());
        // ���� ����
        _weaponState = newWeaponState;
        // ���ο� ���� ����
        StartCoroutine(_weaponState.ToString());
    }

    // Color Type�� Red, Green, Blue �� �� �ϳ��� �������� ������
    private void ColorSetup()
    {
        ColorType type = (ColorType)UnityEngine.Random.Range((int)ColorType.Red, (int)ColorType.Blue+1);
        _towerColor = type;

        _towerRenderer.color = _colorDatas[(int)towerColor];
    }

    private void Update()
    {
        if (attackTarget != null)
            ActionToTarget();
    }
    protected virtual void ActionToTarget()
    {
        // �������κ����� �Ÿ��� ���������κ����� ������ �̿��� ��ġ�� ���ϴ� ����ǥ�� �̿�
        // ���� = arctan(y/x)
        // x, y ������ ���ϱ�
        float dx = attackTarget.position.x - transform.position.x;
        float dy = attackTarget.position.y - transform.position.y;
        // x, y �������� �������� ���� ���ϱ�
        // ������ radian �����̱� ������ Mathf.Rad2Deg�� ���� �� ������ ����
        float degree = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg;
        _towerRenderer.transform.rotation = Quaternion.Euler(0, 0, degree);
    }

    protected virtual IEnumerator SearchTarget()
    {
        while( true )
        {
            float closestDistSqr = Mathf.Infinity;
            
            for(int i=0; i<_enemySpawner.enemyList.Count; i++)
            {
                float distance = Vector3.Distance(this.transform.position, _enemySpawner.enemyList[i].transform.position);

                if(distance <= attackRange && distance <= closestDistSqr)
                {
                    closestDistSqr = distance;
                    _attackTarget = _enemySpawner.enemyList[i].transform;
                }
            }

            if(_attackTarget != null)
            {
                ChangeState(WeaponState.AttackToTarget);
            }

            yield return null;
        }
    }

    protected virtual IEnumerator AttackToTarget()
    {
        while( true )
        {
            // target�� �ִ��� �˻� (�ٸ� �߻�ü�� ���� ���� or Goal �������� �̵��� ������ ��)
            if(_attackTarget == null)
            {
                ChangeState(WeaponState.SearchTarget);
                break;
            }

            // target�� ���ݹ��� ���� �ִ��� �˻� (���ݹ����� ����� �Ǹ� ���ο� ���� Ž��)
            float distance = Vector3.Distance(this.transform.position, attackTarget.transform.position);
            if(distance > attackRange)
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

    private void SpawnProjectile()
    {
        Instantiate(_projectile, _spawnPoint.position, Quaternion.identity);
    }
    // Tower�� ����� ���׷��̵� �ϴ� �޼���, Tower�� �ִ� ����� 3�̴�
    public void GradeUp()
    {
        // Tower�� �ִ� ��޿� �������� ��� �۾��� �������� �ʴ´�
        if(_towerGrade >= 3)
        {
            return;
        }
        
        _towerGrade++;
        UpdateWeaponUI();
    }

    private void UpdateWeaponUI()
    {
        // Ÿ������� 1�̶�� => GradeUp()�� ���ʷ� ����ƴٸ�
        if (_towerGrade == 1)
            _gradeLayout.gameObject.SetActive(true);  // layoutGroup UI�� Ȱ��ȭ �Ѵ�

        // ����� ��Ÿ���� �̹��� �Ѱ��� Ȱ��ȭ ��Ų��
        _gradeImages[_towerGrade - 1].gameObject.SetActive(true);
    }
    public virtual void OnAttack(Enemy enemy)
    {    
        // ���⼭ 3�� ���� ���� �������� ���׷��̵� ��ġ�� ���Ƿ� �� ��
        enemy.OnDamage(_towerData.weapons[_towerGrade].damage + _towerData.weapons[_towerGrade].upgradeDIP * 3);
    }

    // OnSkill() �޼���� Weapon Type���� �ٸ��� �����ؾ� �ϱ� ������
    // �ڽ�Ŭ�������� OnSkill() �޼��带 ���� �����ϵ��� �����ϱ� ���� Abstract Method�� ����
    public abstract void OnSkill();
}

/*
 * File : TowerWeapon.cs
 * First Update : 2022/04/25 MON 10:52
 * Ÿ���� ������ ����ϴ� ��ũ��Ʈ.
 * Ÿ���� ����, ���ݷ�, ���ݼӵ� �� ���ݿ� ���õ� �����͸� ������ ������,
 * Ÿ�� �������� ���� �ٸ� ��ų�� �����ؾ� �ϱ� ������ �߻� Ŭ������ �����Ͽ���.
 * TowerWeapon Ŭ������ ��ӹ޴� 9���� ����Ŭ������ ������ ����Ŭ�������� OnSkill() �޼��带 �����Ѵ�.
 */