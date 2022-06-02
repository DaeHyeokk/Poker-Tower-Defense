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

            // ���� �ӵ� ���� ��ȭ�߱� ������ �ڷ�ƾ �Լ��� �����̿� ���Ǵ� waitForSeconds ���� ������Ʈ.
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
            // Ÿ�� ���� ��ġ�� ���°� �ƴ϶�� ���� Ž������ �ʴ´�.
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
            // ������ Ÿ���� ���ٸ� �������� �ʴ´�.
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
        // Ÿ���� ����, ����, ������ ��� ���ٸ� true �ƴϸ� false ��ȯ.
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
                // ���� �ӵ� ���� ��ȭ�߱� ������ �ڷ�ƾ �Լ��� �����̿� ���Ǵ� waitForSeconds ���� ������Ʈ.
                _attackDelay = new(attackRate);
                // Ÿ�� ������ �� Ÿ���� �÷��� �������� �����Ѵ�.
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
 * Ÿ�� ���� �缳��.
 *     => Tower ������Ʈ�� ���� �����ϰ�, Tower ������Ʈ�� TowerWeapon ������Ʈ�� �����ؼ� ����ϴ� ������� ����. (Has-A ����)
 *     => ���� TowerWeapon���� �ѹ��� �����ϴ� ���ҵ��� ����ȭ �Ͽ� Tower ������Ʈ�� ������Ʈ�� ����. (TowerColor, TowerLevel, TargetDetector)
 *     => Tower ������Ʈ�� ��ü���� ���۵��� Tower Ŭ������ ���� �����ϵ��� ������ ����.
 *     
 * Update : 2022/05/03 TUE 03:10
 * Ÿ���� ���콺 �巡�׿� ���� �����̵��� ObjectFollowMousePosition ������Ʈ�� �����ϴ� ���� ����.
 * Ÿ���� �巡�׷� ������ �� Ÿ���� ��Ÿ��� ǥ�õǵ��� TargetDetector ������Ʈ�� �����ϴ� ���� ����.
 * 
 * Update : 2022/05/07 SAT 16:20
 * Ÿ�� ���� �ٽ� ����.
 *     => Tower ������Ʈ�� �߻�Ŭ������ �����ϰ� �̸� ��ӹ޾� �����ϴ� ����Ŭ����(Top Tower, Onepair Tower, Twopair Tower etc...)�� �����Ͽ���.
 *     => ���� TowerWeapon Ŭ������ ������. 
 *     => ������ TowerColor, TowerLevel, TargetDetector Ŭ������ Monobehaviour ����� �����ϰ� Tower Ŭ�������� ���� �� �����ϵ��� ����.
 * Ÿ���� ��Ÿ� ���� ���� Ž���ϰ� �����ϴ� ���� ����.    
 * TargetDetector Ŭ�������� ����ϴ� Ÿ�� ��Ÿ� ǥ�� ����� Tower Ŭ�������� �����ϵ��� ����
 * 
 * Update : 2022/05/22 SUN
 * MoveTower(), StopTower() �޼ҵ忡�� Follow Tower ������Ʈ�� �����Ͽ� ����ϴ� ���� ����.
 * Ÿ���� �̵��� �� Ÿ���� ��������Ʈ�� ������������ ���� ����.
 */