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
    private Tile _onTile;
    private float _defaultAttackRate;
    private float _increaseAttackRate;
    private float _increaseDamageRate;
    private int _attackCount;
    private int _specialAttackCount;
    private bool _isMove;

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
    public bool isMove
    {
        get => _isMove;
        set
        {
            if(value)
            {
                _isMove = true;
                _towerMovement.StartFollowMousePosition();
                _attackRangeUI.gameObject.SetActive(true);
            }
            else
            {
                _isMove = false;
                _towerMovement.StopFollowMousePosition();
                _attackRangeUI.gameObject.SetActive(false);
            }
        }
    }
    public Tile onTile
    {
        get => _onTile;
        set
        {
            if (value == null)
            {
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

    public abstract String towerName { get; }
    public abstract int towerIndex { get; }


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

        _isMove = false;
        _onTile = null;
    }

    public virtual void Setup()
    {
        _towerColor.ChangeColor();

        _attackCount = 0;
        _increaseAttackRate = 0;
        _increaseDamageRate = 0;

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

            // ������ Ÿ���� ���ų� Ÿ���� �����̰� �ִ� ���¶�� �������� �ʴ´�.
            if (_targetDetector.targetList.Count == 0 || isMove)
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
        Collider2D[] collider2D = Physics2D.OverlapCircleAll(target.transform.position, range * 0.5f);

        for (int i = 0; i < collider2D.Length; i++)
            for (int j = 0; j < basicInflictorList.Count; j++)
                if(collider2D[i].gameObject.activeInHierarchy)
                    basicInflictorList[j].Inflict(collider2D[i].gameObject);

        projectile.ReturnPool();
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
        Collider2D[] collider2D = Physics2D.OverlapCircleAll(target.transform.position, range / 2);

        for (int i = 0; i < collider2D.Length; i++)
            for (int j = 0; j < specialInflictorList.Count; j++)
                if (collider2D[i].gameObject.activeInHierarchy)
                    specialInflictorList[j].Inflict(collider2D[i].gameObject);

        projectile.ReturnPool();
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

    private bool IsCompareTower(Tower compareTower)
    {
        // Ÿ���� ����, ����, ������ ��� ���ٸ� true �ƴϸ� false ��ȯ.
        if ((this.towerIndex == compareTower.towerIndex)
            && (this.colorType == compareTower.colorType)
            && (this.level == compareTower.level))
            return true;

        return false;
    }

    public bool MergeTower(Tower mergeTower)
    {
        if (IsCompareTower(mergeTower))
        {
            if (_towerLevel.LevelUp())
            {
                _towerColor.ChangeColor();
                mergeTower.ReturnPool();
                return true;
            }
        }

        return false;
    }

    private void ReturnPool()
    {
        if (onTile != null)
            onTile = null;

        _towerLevel.Reset();

        _towerBuilder.towerPool.ReturnObject(this, towerIndex);
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
 * Update : 2022/05/07 16:20
 * Ÿ�� ���� �ٽ� ����.
 *     => Tower ������Ʈ�� �߻�Ŭ������ �����ϰ� �̸� ��ӹ޾� �����ϴ� ����Ŭ����(Top Tower, Onepair Tower, Twopair Tower etc...)�� �����Ͽ���.
 *     => ���� TowerWeapon Ŭ������ ������. 
 *     => ������ TowerColor, TowerLevel, TargetDetector Ŭ������ Monobehaviour ����� �����ϰ� Tower Ŭ�������� ���� �� �����ϵ��� ����.
 * Ÿ���� ��Ÿ� ���� ���� Ž���ϰ� �����ϴ� ���� ����.    
 * TargetDetector Ŭ�������� ����ϴ� Ÿ�� ��Ÿ� ǥ�� ����� Tower Ŭ�������� �����ϵ��� ����
 * 
 */