using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public abstract class Tower : MonoBehaviour
{
    public enum AttackType { Basic, Special }

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
    private float _defaultAttackRate;
    private float _increaseAttackRate;
    private float _increaseDamageRate;
    private int _attackCount;
    private int _specialAttackCount;

    protected List<IInflictable> basicInflictorList { get; set; }
    protected List<IInflictable> specialInflictorList { get; set; }

    protected ProjectileSpawner projectileSpawner => _projectileSpawner;
    protected Transform spawnPoint => _spawnPoint;

    public TargetDetector targetDetector => _targetDetector;
    public SpriteRenderer towerRenderer => _towerRenderer;
    public Sprite normalProjectileSprite => _towerData.normalProjectileSprites[(int)_towerColor.colorType];
    public Sprite specialProjectileSprite => _towerData.specialProjectileSprites[(int)_towerColor.colorType];

    public TowerColor.ColorType colorType => _towerColor.colorType;
    public int upgradeCount => GameManager.instance.colorUpgradeCounts[(int)colorType];
    public int level => _towerLevel.level;
    public float damage => (_towerData.weapons[level].damage + (upgradeCount * _towerData.weapons[level].upgradeDIP)) * (1f + (_increaseDamageRate * 0.01f));
    public float attackRate => _defaultAttackRate - (_towerData.weapons[level].rate + (_towerData.weapons[level].rate * (_increaseAttackRate * 0.01f)));
    public float range => _towerData.weapons[level].range;
    public int maxTargetCount => _towerData.weapons[level].maxTargetCount;
    public int attackCount => _attackCount;
    public bool isOnTile { get; set; }
    public Tile onTile { get; set; }
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

        basicInflictorList = new List<IInflictable>();
        specialInflictorList = new List<IInflictable>();

        _defaultAttackRate = 3f;
        _specialAttackCount = 10;
    }

    public virtual void Setup()
    {
        _towerLevel.Reset();
        _towerColor.ChangeColor();

        _attackCount = 0;
        _increaseAttackRate = 0;
        _increaseDamageRate = 0;

        isOnTile = false;
        onTile = null;

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

            // 공격할 타겟이 없거나 타워가 타일 위에 있는 상태가 아니라면 공격하지 않는다.
            if (_targetDetector.targetList.Count == 0 || !isOnTile)
                yield return null;
            else
            {
                _attackCount++;

                for (int i = 0; i < _targetDetector.targetList.Count; i++)
                {
                    if(_attackCount < _specialAttackCount)
                        ShotProjectile(_targetDetector.targetList[i], AttackType.Basic);
                    else
                        ShotProjectile(_targetDetector.targetList[i], AttackType.Special);
                }

                if (_attackCount >= _specialAttackCount) _attackCount = 0;

                yield return new WaitForSeconds(attackRate);
            }
        }
    }

    protected abstract void ShotProjectile(Enemy target, AttackType attackType);

    protected virtual void BasicInflict(Projectile projectile, Enemy target)
    {
        for (int i = 0; i < basicInflictorList.Count; i++)
            if (target.gameObject.activeInHierarchy)
                basicInflictorList[i].Inflict(target.gameObject);

        projectile.ReturnPool();
    }

    protected virtual void BasicInflict(Tower target)
    {
        for (int i = 0; i < basicInflictorList.Count; i++)
            basicInflictorList[i].Inflict(target.gameObject);
    }

    protected virtual void BasicInflict(Projectile projectile, Enemy target, float range)
    {
        Vector3 tempScale = projectile.transform.localScale;
        projectile.transform.localScale = new Vector3(5f, 5f, 0f);

        Collider2D[] collider2D = Physics2D.OverlapCircleAll(target.transform.position, range * 0.5f);

        for (int i = 0; i < collider2D.Length; i++)
            for (int j = 0; j < basicInflictorList.Count; j++)
                if(collider2D[i].gameObject.activeInHierarchy)
                    basicInflictorList[j].Inflict(collider2D[i].gameObject);

        projectile.DelayReturnPool(0.2f);
        projectile.transform.localScale = tempScale;
    }

    protected virtual void BasicInflict(Tower target, float range)
    {
        Collider2D[] collider2D = Physics2D.OverlapCircleAll(target.transform.position, range / 2);

        for (int i = 0; i < collider2D.Length; i++)
            for (int j = 0; j < basicInflictorList.Count; j++)
                basicInflictorList[j].Inflict(collider2D[i].gameObject);
    }

    protected virtual void SpecialInflict(Projectile projectile, Enemy target)
    {
        for (int i = 0; i < specialInflictorList.Count; i++)
            if (target.gameObject.activeInHierarchy)
                specialInflictorList[i].Inflict(target.gameObject);

        projectile.ReturnPool();
    }

    protected virtual void SpecialInflict(Tower target)
    {
        for (int i = 0; i < specialInflictorList.Count; i++)
            specialInflictorList[i].Inflict(target.gameObject);
    }

    protected virtual void SpecialInflict(Projectile projectile, Enemy target, float range)
    {
        Vector3 tempScale = projectile.transform.localScale;
        projectile.transform.localScale = new Vector3(5f, 5f, 0f);

        Collider2D[] collider2D = Physics2D.OverlapCircleAll(target.transform.position, range / 2);

        for (int i = 0; i < collider2D.Length; i++)
            for (int j = 0; j < specialInflictorList.Count; j++)
                if (collider2D[i].gameObject.activeInHierarchy)
                    specialInflictorList[j].Inflict(collider2D[i].gameObject);

        projectile.DelayReturnPool(0.2f);
        projectile.transform.localScale = tempScale;
    }

    protected virtual void SpecialInflict(Tower target, float range)
    {
        Collider2D[] collider2D = Physics2D.OverlapCircleAll(target.transform.position, range / 2);

        for (int i = 0; i < collider2D.Length; i++)
            for (int j = 0; j < specialInflictorList.Count; j++)
                specialInflictorList[j].Inflict(collider2D[i].gameObject);
    }

    public void TakeIncreaseAttackRate(float increaseAttackRate, float duration)
    {
        StartCoroutine(IncreaseAttackRateCoroutine(increaseAttackRate, duration));
    }

    private IEnumerator IncreaseAttackRateCoroutine(float increaseAttackRate, float duration)
    {
        _increaseAttackRate += increaseAttackRate;

        yield return new WaitForSeconds(duration);

        _increaseAttackRate -= increaseAttackRate;
    }

    public void TakeIncreaseDamageRate(float increaseDamageRate, float duration)
    {
        StartCoroutine(IncreaseDamageRateCoroutine(increaseDamageRate, duration));
    }

    private IEnumerator IncreaseDamageRateCoroutine(float increaseDamageRate, float duration)
    {
        _increaseDamageRate += increaseDamageRate;

        yield return new WaitForSeconds(duration);

        _increaseDamageRate -= increaseDamageRate;
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
 * Update : 2022/05/07 16:20
 * 타워 구조 다시 변경.
 *     => Tower 오브젝트를 추상클래스로 정의하고 이를 상속받아 구현하는 서브클래스(Top Tower, Onepair Tower, Twopair Tower etc...)를 구현하였음.
 *     => 따라서 TowerWeapon 클래스는 삭제됨. 
 *     => 기존의 TowerColor, TowerLevel, TargetDetector 클래스는 Monobehaviour 상속을 제거하고 Tower 클래스에서 생성 및 제어하도록 변경.
 * 타워가 사거리 내의 적을 탐색하고 공격하는 로직 구현.    
 * TargetDetector 클래스에서 담당하던 타워 사거리 표시 기능을 Tower 클래스에서 수행하도록 변경
 * 
 */