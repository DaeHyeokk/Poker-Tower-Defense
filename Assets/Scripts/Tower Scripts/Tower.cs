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
    private Particle _increaseDamageEffect;
    [SerializeField]
    private Particle _increaseAttackRateEffect;

    private HorizontalLayoutGroup _levelLayout;
    private SpriteRenderer _towerRenderer;
    private TowerColor _towerColor;
    private TowerLevel _towerLevel;
    private TargetDetector _targetDetector;
    private ProjectileSpawner _projectileSpawner;
    private TowerBuilder _towerBuilder;
    private Tile _onTile;
    private WaitForSeconds _attackDelay;
    private float _maxAttackRate;
    private float _increaseAttackRate;
    private float _increaseDamageRate;

    private float increaseAttackRate
    {
        get => _increaseAttackRate;
        set
        {
            if (_increaseAttackRate == 0 && value > 0)
                _increaseAttackRateEffect.PlayParticle();
            else if (_increaseAttackRate != 0 && value == 0)
                _increaseAttackRateEffect.StopParticle();

            _increaseAttackRate = value;

            // 공격 속도 값이 변화했기 때문에 코루틴 함수의 딜레이에 사용되는 waitForSeconds 변수 업데이트.
            _attackDelay = new(attackRate);
        }
    }
    private float increaseDamageRate
    {
        get => _increaseDamageRate;
        set
        {
            if (_increaseDamageRate == 0 && value > 0)
                _increaseDamageEffect.PlayParticle();
            else if (_increaseDamageRate != 0 && value == 0)
                _increaseDamageEffect.StopParticle();

            _increaseDamageRate = value;
        }
    }

    protected int attackCount { get; set; }
    protected int specialAttackCount { get; private set; }

    protected List<IEnemyInflictable> basicEnemyInflictorList { get; set; }
    protected List<IEnemyInflictable> specialEnemyInflictorList { get; set; }
    protected List<ITowerInflictable> basicTowerInflictorList { get; set; }
    protected List<ITowerInflictable> specialTowerInflictorList { get; set; }

    protected ProjectileSpawner projectileSpawner => _projectileSpawner;
    protected Transform spawnPoint => _spawnPoint;

    public TargetDetector targetDetector => _targetDetector;
    public SpriteRenderer towerRenderer => _towerRenderer;
    public TowerColor towerColor => _towerColor;
    public TowerLevel towwerLevel => _towerLevel;
    public Sprite normalProjectileSprite => _towerData.normalProjectileSprites[(int)_towerColor.colorType];
    public Sprite specialProjectileSprite => _towerData.specialProjectileSprites[(int)_towerColor.colorType];

    public int upgradeCount => GameManager.instance.colorUpgradeCounts[(int)towerColor.colorType];
    public int level => _towerLevel.level;
    public float damage => (_towerData.weapons[level].damage + (upgradeCount * _towerData.weapons[level].upgradeDIP)) * (1f + (increaseDamageRate * 0.01f));
    public float attackRate 
    {
        get
        {
            float value = _towerData.weapons[level].rate / (1 + increaseAttackRate * 0.01f);
            return value > _maxAttackRate ? value : _maxAttackRate;
        }
    }
    public float range => _towerData.weapons[level].range;
    public int maxTargetCount => _towerData.weapons[level].maxTargetCount;
    public WaitForSeconds attackDelay => _attackDelay;
    public virtual Tile onTile
    {
        get => _onTile;
        set
        {
            if (value == null)
            {
                if (_onTile == null)
                    return;

                _onTile.collocationTower = null;
                _onTile = null;
            }
            else
            {
                if (_onTile != null)
                    _onTile.collocationTower = null;

                _onTile = value;
                _onTile.collocationTower = this;

                this.transform.position = _onTile.transform.position;
            }
        }
    }

    public int salesGold => defaultSalesGold * (int)Mathf.Pow(2, level);

    protected abstract int defaultSalesGold { get; }

    public abstract String towerName { get; }
    public abstract int towerIndex { get; }


    protected virtual void Awake()
    {
        _towerRenderer = GetComponentInChildren<SpriteRenderer>();
        _levelLayout = GetComponentInChildren<HorizontalLayoutGroup>();
        _projectileSpawner = FindObjectOfType<ProjectileSpawner>();
        _towerBuilder = FindObjectOfType<TowerBuilder>();

        _towerColor = new TowerColor(_towerRenderer);
        _towerLevel = new TowerLevel(_levelLayout);
        _targetDetector = new TargetDetector(this, FindObjectOfType<EnemySpawner>());

        basicEnemyInflictorList = new();
        specialEnemyInflictorList = new();
        basicTowerInflictorList = new();
        specialTowerInflictorList = new();
        _maxAttackRate = 0.1f;
        specialAttackCount = 10;

        _onTile = null;
    }

    public virtual void Setup()
    {
        _towerColor.ChangeColor();

        attackCount = 0;
        increaseAttackRate = 0;
        increaseDamageRate = 0;

        StartCoroutine(SearchTarget());
        StartCoroutine(AttackTarget());
    }

    private IEnumerator SearchTarget()
    {
        while(true)
        {
            // 타일 위에 배치된 상태가 아니라면 적을 탐색하지 않는다.
            if (onTile == null)
                yield return null;

            _targetDetector.SearchTarget();
            yield return null;
        }
    }

    protected virtual IEnumerator AttackTarget()
    {
        while (true)
        {
            // 공격할 타겟이 없다면 공격하지 않는다.
            if (_targetDetector.targetList.Count == 0)
                yield return null;
            else
            {
                attackCount++;

                for (int i = 0; i < _targetDetector.targetList.Count; i++)
                {
                    if(attackCount < specialAttackCount)
                        ShotProjectile(_targetDetector.targetList[i], AttackType.Basic);
                    else
                        ShotProjectile(_targetDetector.targetList[i], AttackType.Special);
                }

                if (attackCount >= specialAttackCount) attackCount = 0;

                yield return attackDelay;
            }
        }
    }

    protected abstract void ShotProjectile(Enemy target, AttackType attackType);

    protected virtual void BasicInflict(Enemy target)
    {
        for (int i = 0; i < basicEnemyInflictorList.Count; i++)
            if (target.gameObject.activeInHierarchy)
                basicEnemyInflictorList[i].Inflict(target);
    }
    protected virtual void BasicInflict(Tower target)
    {
        for (int i = 0; i < basicTowerInflictorList.Count; i++)
            basicTowerInflictorList[i].Inflict(target);
    }

    protected virtual void BasicInflict(Enemy target, float range)
    {
        ParticlePlayer.instance.PlayRangeAttack(target.transform, range, (int)_towerColor.colorType);

        Collider2D[] collider2D = Physics2D.OverlapCircleAll(target.transform.position, range * 0.5f);

        for (int i = 0; i < collider2D.Length; i++)
            for (int j = 0; j < basicEnemyInflictorList.Count; j++)
                if(collider2D[i].gameObject.activeInHierarchy)
                    basicEnemyInflictorList[j].Inflict(collider2D[i].GetComponent<Enemy>());
    }

    protected virtual void BasicInflict(Tower target, float range)
    {
        ParticlePlayer.instance.PlayRangeAttack(target.transform, range, (int)_towerColor.colorType);

        Collider[] collider = Physics.OverlapSphere(target.transform.position, range / 2);

        for (int i = 0; i < collider.Length; i++)
            for (int j = 0; j < basicEnemyInflictorList.Count; j++)
                basicTowerInflictorList[j].Inflict(collider[i].GetComponent<Tower>());
    }

    protected virtual void SpecialInflict(Enemy target)
    {
        for (int i = 0; i < specialEnemyInflictorList.Count; i++)
            if (target.gameObject.activeInHierarchy)
                specialEnemyInflictorList[i].Inflict(target);
    }

    protected virtual void SpecialInflict(Tower target)
    {
        for (int i = 0; i < specialTowerInflictorList.Count; i++)
            specialTowerInflictorList[i].Inflict(target);
    }

    protected virtual void SpecialInflict(Enemy target, float range)
    {
        ParticlePlayer.instance.PlayRangeAttack(target.transform, range, (int)_towerColor.colorType);

        Collider2D[] collider2D = Physics2D.OverlapCircleAll(target.transform.position, range / 2);

        for (int i = 0; i < collider2D.Length; i++)
            for (int j = 0; j < specialEnemyInflictorList.Count; j++)
                if (collider2D[i].gameObject.activeInHierarchy)
                    specialEnemyInflictorList[j].Inflict(collider2D[i].GetComponent<Enemy>());
    }

    protected virtual void SpecialInflict(Tower target, float range)
    {
        Collider[] collider = Physics.OverlapSphere(target.transform.position, range / 2);

        for (int i = 0; i < collider.Length; i++)
            for (int j = 0; j < specialEnemyInflictorList.Count; j++)
                specialTowerInflictorList[j].Inflict(collider[i].GetComponent<Tower>());
    }

    public void TakeIncreaseAttackRate(float increaseAttackRate, float duration)
    {
        StartCoroutine(IncreaseAttackRateCoroutine(increaseAttackRate, duration));
    }

    private IEnumerator IncreaseAttackRateCoroutine(float increaseAttackRate, float duration)
    {
        this.increaseAttackRate += increaseAttackRate;

        yield return new WaitForSeconds(duration);

        this.increaseAttackRate -= increaseAttackRate;
    }

    public void TakeIncreaseDamageRate(float increaseDamageRate, float duration)
    {
        StartCoroutine(IncreaseDamageRateCoroutine(increaseDamageRate, duration));
    }

    private IEnumerator IncreaseDamageRateCoroutine(float increaseDamageRate, float duration)
    {
        this.increaseDamageRate += increaseDamageRate;

        yield return new WaitForSeconds(duration);

        this.increaseDamageRate -= increaseDamageRate;
    }

    private bool IsCompareTower(Tower compareTower)
    {
        // 타워의 종류, 색깔, 레벨이 모두 같다면 true 아니면 false 반환.
        if ((this.towerIndex == compareTower.towerIndex)
            && (this._towerColor.colorType == compareTower._towerColor.colorType)
            && (this.level == compareTower.level))
            return true;

        return false;
    }

    public virtual bool MergeTower(Tower mergeTower)
    {
        if (IsCompareTower(mergeTower))
        {
            if (_towerLevel.LevelUp())
            {
                // 공격 속도 값이 변화했기 때문에 코루틴 함수의 딜레이에 사용되는 waitForSeconds 변수 업데이트.
                _attackDelay = new(attackRate);
                // 타워 레벨업 시 타워의 컬러를 랜덤으로 변경한다.
                _towerColor.ChangeColor();
                mergeTower.ReturnPool();
                return true;
            }
        }

        return false;
    }

    public void MoveTower()
    {
        FollowTower followTower = _towerBuilder.followTower;

        followTower.Setup(this);
        followTower.gameObject.SetActive(true);
        followTower.StartFollowMousePosition();

        Color color = _towerRenderer.color;
        color.a = onTile != null ? 0.3f : 0f;
        _towerRenderer.color = color;

        UIManager.instance.ShowTowerInfo(this);
    }

    public void StopTower()
    {
        FollowTower followTower = _towerBuilder.followTower;

        followTower.StopFollowMousePosition();
        followTower.gameObject.SetActive(false);

        _towerRenderer.color = _towerColor.color;

        UIManager.instance.HideTowerInfo();
    }

    public void ReturnPool()
    {
        if (onTile != null)
            onTile = null;

        _towerRenderer.transform.rotation = Quaternion.Euler(0f, 0f, 0f);

        _towerLevel.Reset();
        _targetDetector.ResetTarget();

        _towerBuilder.towerPoolList[towerIndex].ReturnObject(this);
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
 * Update : 2022/05/03 TUE 03:10
 * 타워가 마우스 드래그에 따라 움직이도록 ObjectFollowMousePosition 컴포넌트를 제어하는 로직 구현.
 * 타워를 드래그로 움직일 때 타워의 사거리가 표시되도록 TargetDetector 컴포넌트를 제어하는 로직 구현.
 * 
 * Update : 2022/05/07 SAT 16:20
 * 타워 구조 다시 변경.
 *     => Tower 오브젝트를 추상클래스로 정의하고 이를 상속받아 구현하는 서브클래스(Top Tower, Onepair Tower, Twopair Tower etc...)를 구현하였음.
 *     => 따라서 TowerWeapon 클래스는 삭제됨. 
 *     => 기존의 TowerColor, TowerLevel, TargetDetector 클래스는 Monobehaviour 상속을 제거하고 Tower 클래스에서 생성 및 제어하도록 변경.
 * 타워가 사거리 내의 적을 탐색하고 공격하는 로직 구현.    
 * TargetDetector 클래스에서 담당하던 타워 사거리 표시 기능을 Tower 클래스에서 수행하도록 변경
 * 
 * Update : 2022/05/22 SUN
 * MoveTower(), StopTower() 메소드에서 Follow Tower 오브젝트를 참조하여 사용하는 로직 구현.
 * 타워가 이동할 때 타워의 스프라이트가 반투명해지는 로직 구현.
 */