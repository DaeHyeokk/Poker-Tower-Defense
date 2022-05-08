using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public abstract class Tower : MonoBehaviour
{
    public enum ProjectileType { Normal, Special }

    [SerializeField]
    private TowerData _towerData;
    [SerializeField]
    private Transform _spawnPoint;
    [SerializeField]
    private Transform _attackRangeUI;

    private ObjectFollowMousePosition _towerMovement;
    private HorizontalLayoutGroup _levelLayout;
    private SpriteRenderer _towerRenderer;
    private TowerColor _towerColor;
    private TowerLevel _towerLevel;
    private TargetDetector _targetDetector;
    private ProjectileSpawner _projectileSpawner;
    private TowerBuilder _towerBuilder;
    private WaitForSeconds _attackRateDelay;
    private int _attackCount;
    private ProjectileType _projectileType;

    protected ProjectileSpawner projectileSpawner => _projectileSpawner;
    protected WaitForSeconds attackRateDelay => _attackRateDelay;

    public TargetDetector targetDetector => _targetDetector;
    public SpriteRenderer towerRenderer => _towerRenderer;
    public TowerData towerData => _towerData;
    public Sprite normalProjectileSprite => _towerData.normalProjectileSprites[(int)_towerColor.colorType];
    public Sprite specialProjectileSprite => _towerData.specialProjectileSprites[(int)_towerColor.colorType];
    public Transform spawnPoint => _spawnPoint;

    public float damage => _towerData.weapons[_towerLevel.level].damage;
    public float attackRate => _towerData.weapons[_towerLevel.level].rate;
    public float range => _towerData.weapons[_towerLevel.level].range;
    public int maxTargetCount => _towerData.weapons[_towerLevel.level].maxTargetCount;
    public int level => _towerLevel.level;

    public abstract String towerName { get; }

    protected virtual void Awake()
    {
        _towerMovement = GetComponent<ObjectFollowMousePosition>();
        _towerRenderer = GetComponentInChildren<SpriteRenderer>();
        _levelLayout = GetComponentInChildren<HorizontalLayoutGroup>(true);

        _projectileSpawner = FindObjectOfType<ProjectileSpawner>();
        _towerBuilder = FindObjectOfType<TowerBuilder>();

        _towerColor = new TowerColor(_towerRenderer);
        _towerLevel = new TowerLevel(_levelLayout);
        _targetDetector = new TargetDetector(this, FindObjectOfType<EnemySpawner>());
    }

    public virtual void Setup()
    {
        _towerLevel.Reset();
        _towerColor.ChangeColor();

        _attackRateDelay = new WaitForSeconds(attackRate);
        _attackCount = 0;
        _projectileType = ProjectileType.Normal;

        SetAttackRangeUIScale();
        StartCoroutine(SearchAndAction());
    }

    private void SetAttackRangeUIScale()
    {
        float attackRangeScale = range * 2 / this.transform.lossyScale.x;
        _attackRangeUI.transform.localScale = new Vector3(attackRangeScale, attackRangeScale, 0);
    }

    protected virtual IEnumerator SearchAndAction()
    {
        while (true)
        {
            _targetDetector.SearchTarget();

            if (_targetDetector.targetList.Count == 0)
                yield return null;
            else
            {
                _attackCount++;
                if (_attackCount >= 10)
                {
                    _projectileType = ProjectileType.Special;

                    for (int i = 0; i < _targetDetector.targetList.Count; i++)
                        ShotProjectile(_targetDetector.targetList[i], _projectileType);

                    
                    _projectileType = ProjectileType.Normal;
                    _attackCount = 0;
                }
                else
                {
                    for (int i = 0; i < _targetDetector.targetList.Count; i++)
                        ShotProjectile(_targetDetector.targetList[i], _projectileType);
                }

                yield return attackRateDelay;
            }
        }
    }

    protected virtual void ShotProjectile(Enemy target, ProjectileType projectileType)
    {
        if (projectileType == ProjectileType.Normal)
        {
            Projectile projectile = projectileSpawner.SpawnProjectile(this, spawnPoint, target, normalProjectileSprite);
            projectile.actionOnCollision += () => DoInflict(projectile, target);
        }
        else // (projectileType == ProjectileType.Sepcial)
        {
            Projectile projectile = projectileSpawner.SpawnProjectile(this, spawnPoint, target, specialProjectileSprite);
            projectile.actionOnCollision += () => DoSpecialInflict(projectile, target);
        }
    }

    public virtual void DoInflict(Projectile projectile, Enemy target)
    {
        target.OnDamage(damage);
        projectile.ReturnPool();
    }

    public virtual void DoSpecialInflict(Projectile projectile, Enemy target)
    {
        target.OnDamage(damage * 2);
        projectile.ReturnPool();
    }
    public void MoveTower()
    {
        _towerMovement.StartFollowMousePosition();
        _attackRangeUI.gameObject.SetActive(true);
    }

    public void StopTower()
    {
        _towerMovement.StopFollowMousePosition();
        _attackRangeUI.gameObject.SetActive(false);
    }

    public void ReturnPool()
    {
        _towerBuilder.towerPool.ReturnObject(this);
    }
}


/*
 * File : Tower.cs
 * First Update : 2022/04/30 SAT 23:50
 * 타워 구조 재설계.
 *     => Tower 오브젝트를 새로 정의하고, Tower 오브젝트에 TowerWeapon 오브젝트를 부착해서 사용하는 방식으로 변경. (Has-A 관계)
 *     => 기존 TowerWeapon에서 한번에 수행하던 역할들을 세분화 하여 Tower 오브젝트의 컴포넌트로 구현. (TowerColor, TowerLevel, TargetDetector)
 *     => Tower 오브젝트의 전체적인 동작들을 Tower 클래스를 통해 수행하도록 구현할 예정.
 *     
 * Update : 2022/05/03 03:10
 * 타워가 마우스 드래그에 따라 움직이도록 ObjectFollowMousePosition 컴포넌트를 제어하는 로직 구현.
 * 타워를 드래그로 움직일 때 타워의 사거리가 표시되도록 TargetDetector 컴포넌트를 제어하는 로직 구현.
 * 
 */