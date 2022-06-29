using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

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

    private Rotater2D _rotater2D;
    private HorizontalLayoutGroup _levelLayout;
    private SpriteRenderer _towerRenderer;
    private TowerColor _towerColor;
    private TowerLevel _towerLevel;
    private TargetDetector _targetDetector;
    private ProjectileSpawner _projectileSpawner;
    private TowerBuilder _towerBuilder;
    private ColorUpgrade _colorUpgrade;
    private Tile _onTile;
    private WaitForSeconds _attackDelay;
    private float _attackRate;
    private float _maxAttackRate;
    private float _remainAttackDelay;
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

    protected readonly WaitForSeconds _waitForPointFiveSeconds = new(0.5f);

    protected float remainAttackDelay { get; set; }
    protected int attackCount { get; set; }
    protected int specialAttackCount { get; private set; }

    protected List<IEnemyInflictable> baseEnemyInflictorList { get; set; } = new();
    protected List<IEnemyInflictable> specialEnemyInflictorList { get; set; } = new();
    protected List<ITowerInflictable> baseTowerInflictorList { get; set; } = new();
    protected List<ITowerInflictable> specialTowerInflictorList { get; set; } = new();

    protected ProjectileSpawner projectileSpawner => _projectileSpawner;
    protected Transform spawnPoint => _spawnPoint;
    protected Rotater2D rotater2D => _rotater2D;

    public TargetDetector targetDetector => _targetDetector;
    public SpriteRenderer towerRenderer => _towerRenderer;
    public TowerColor towerColor => _towerColor;
    public Sprite normalProjectileSprite => _towerData.normalProjectileSprites[(int)_towerColor.colorType];
    public Sprite specialProjectileSprite => _towerData.specialProjectileSprites[(int)_towerColor.colorType];
    public StringBuilder detailBaseAttackInfo { get; set; } = new();
    public StringBuilder detailSpecialAttackInfo { get; set; } = new();
    public int upgradeCount => _colorUpgrade.colorUpgradeCounts[(int)towerColor.colorType];
    public int level => _towerLevel.level;
    public float baseDamage => _towerData.weapons[level].damage;
    public float upgradeDIP => _towerData.weapons[level].upgradeDIP;
    public float damage => (baseDamage + (upgradeDIP * upgradeCount)) * (1f + (increaseDamageRate * 0.01f));
    public float baseAttackRate => _towerData.weapons[level].rate;
    public float upgradeRIP => _towerData.weapons[level].upgradeRIP;
    public float attackRate 
    {
        get => _attackRate;
        set
        {
            // value 값이 최대 공격속도보다 빠르다면 value값을 최대 공격속도로 변경한다.
            if (value < _maxAttackRate)
                value = _maxAttackRate;

            if (_attackRate == value) return;
            else
            {
                float delaySpent = _attackRate - remainAttackDelay;
                _attackRate = value;
                remainAttackDelay = _attackRate - delaySpent;
            }
        }
    }
    public float range => _towerData.weapons[level].range;
    public int maxTargetCount => _towerData.weapons[level].maxTargetCount;
    public int salesGold => _towerData.weapons[level].salesGold;

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

    public abstract string towerName { get; }
    public abstract int towerIndex { get; }

    protected virtual void Awake()
    {
        _rotater2D = GetComponent<Rotater2D>();
        _towerRenderer = GetComponentInChildren<SpriteRenderer>();
        _levelLayout = GetComponentInChildren<HorizontalLayoutGroup>();
        _projectileSpawner = FindObjectOfType<ProjectileSpawner>();
        _towerBuilder = FindObjectOfType<TowerBuilder>();
        _colorUpgrade = FindObjectOfType<ColorUpgrade>();

        _towerColor = new TowerColor(_towerRenderer);
        _towerLevel = new TowerLevel(_levelLayout);
        _targetDetector = new TargetDetector(this, FindObjectOfType<EnemySpawner>());

        _maxAttackRate = 0.1f;
        specialAttackCount = 10;
    }

    private void Update()
    {
        UpdateAttackRate();

        if (_onTile == null) return;

        _targetDetector.SearchTarget();
        RotateTower();
        DecreaseAttackDelay();
    }

    private void UpdateAttackRate() => attackRate = (baseAttackRate - (upgradeRIP * upgradeCount)) / (1 + increaseAttackRate * 0.01f);

    protected virtual void RotateTower()
    {
        if (_targetDetector.targetList.Count > 0)
            _rotater2D.NaturalLookAtTarget(_targetDetector.targetList[0].transform);
    }

    private void DecreaseAttackDelay()
    {
        remainAttackDelay -= Time.deltaTime;
        if (_targetDetector.targetList.Count == 0) return;
        if (remainAttackDelay <= 0f)
        {
            AttackTarget();
            remainAttackDelay = _attackRate;
        }
    }

    public virtual void Setup()
    {
        _towerLevel.Reset();
        _targetDetector.ResetTarget();

        attackCount = 0;
        increaseAttackRate = 0;
        increaseDamageRate = 0;

        _towerColor.ChangeRandomColor();
        UpdateDetailInfo();
    }

    protected virtual void AttackTarget()
    {
        attackCount++;

        for (int i = 0; i < _targetDetector.targetList.Count; i++)
        {
            if (attackCount < specialAttackCount)
                ShotProjectile(_targetDetector.targetList[i], AttackType.Basic);
            else
                ShotProjectile(_targetDetector.targetList[i], AttackType.Special);
        }

        if (attackCount >= specialAttackCount)
            attackCount = 0;
    }

    protected abstract void ShotProjectile(Enemy target, AttackType attackType);

    protected virtual void BaseInflict(Enemy target)
    {
        for (int i = 0; i < baseEnemyInflictorList.Count; i++)
            if (target.gameObject.activeSelf)
                baseEnemyInflictorList[i].Inflict(target);
    }
    protected virtual void BaseInflict(Tower target)
    {
        for (int i = 0; i < baseTowerInflictorList.Count; i++)
            if (target.gameObject.activeSelf)
                baseTowerInflictorList[i].Inflict(target);
    }

    protected virtual void BaseInflict(Enemy target, float range)
    {
        ParticlePlayer.instance.PlayRangeAttack(target.transform, range, (int)_towerColor.colorType);

        targetDetector.SearchTargetWithinRange(target.transform, range);

        for (int i = 0; i < targetDetector.targetWithinRangeList.Count; i++)
            for (int j = 0; j < baseEnemyInflictorList.Count; j++)
                if (targetDetector.targetWithinRangeList[i].gameObject.activeSelf)
                    baseEnemyInflictorList[j].Inflict(targetDetector.targetWithinRangeList[i]);

        targetDetector.targetWithinRangeList.Clear();
    }

    protected virtual void BaseInflict(Tower target, float range)
    {
        Collider[] collider = Physics.OverlapSphere(target.transform.position, range * 0.5f);

        for (int i = 0; i < collider.Length; i++)
            for (int j = 0; j < baseEnemyInflictorList.Count; j++)
                if (collider[i].gameObject.activeSelf)
                    baseTowerInflictorList[j].Inflict(collider[i].GetComponent<Tower>());
    }

    protected virtual void SpecialInflict(Enemy target)
    {
        for (int i = 0; i < specialEnemyInflictorList.Count; i++)
            if (target.gameObject.activeSelf)
                specialEnemyInflictorList[i].Inflict(target);
    }

    protected virtual void SpecialInflict(Tower target)
    {
        for (int i = 0; i < specialTowerInflictorList.Count; i++)
            if (target.gameObject.activeSelf)
                specialTowerInflictorList[i].Inflict(target);
    }

    protected virtual void SpecialInflict(Enemy target, float range)
    {
        ParticlePlayer.instance.PlayRangeAttack(target.transform, range, (int)_towerColor.colorType);

        targetDetector.SearchTargetWithinRange(target.transform, range);

        for (int i = 0; i < targetDetector.targetWithinRangeList.Count; i++)
            for (int j = 0; j < specialEnemyInflictorList.Count; j++)
                if (targetDetector.targetWithinRangeList[i].gameObject.activeSelf)
                    specialEnemyInflictorList[j].Inflict(targetDetector.targetWithinRangeList[i]);

        targetDetector.targetWithinRangeList.Clear();
    }

    protected virtual void SpecialInflict(Tower target, float range)
    {
        Collider[] collider = Physics.OverlapSphere(target.transform.position, range * 0.5f);

        for (int i = 0; i < collider.Length; i++)
            for (int j = 0; j < specialEnemyInflictorList.Count; j++)
                if (collider[i].gameObject.activeSelf)
                    specialTowerInflictorList[j].Inflict(collider[i].GetComponent<Tower>());
    }

    public void TakeIncreaseAttackRate(float increaseAttackRate, float duration)
    {
        StartCoroutine(IncreaseAttackRateCoroutine(increaseAttackRate, duration));
    }

    private IEnumerator IncreaseAttackRateCoroutine(float increaseAttackRate, float duration)
    {
        this.increaseAttackRate += increaseAttackRate;

        while(duration > 0)
        {
            yield return _waitForPointFiveSeconds;
            duration -= 0.5f;
        }

        this.increaseAttackRate -= increaseAttackRate;
    }

    public void TakeIncreaseDamageRate(float increaseDamageRate, float duration)
    {
        StartCoroutine(IncreaseDamageRateCoroutine(increaseDamageRate, duration));
    }

    private IEnumerator IncreaseDamageRateCoroutine(float increaseDamageRate, float duration)
    {
        this.increaseDamageRate += increaseDamageRate;

        while (duration > 0)
        {
            yield return _waitForPointFiveSeconds;
            duration -= 0.5f;
        }

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
                // 타워 레벨업 시 타워의 컬러를 랜덤으로 변경한다.
                _towerColor.ChangeRandomColor();
                // 타워 레벨업 시 타워의 상세 정보 StringBuilder를 업데이트 한다.
                UpdateDetailInfo();
                mergeTower.ReturnPool();
                return true;
            }
        }

        return false;
    }

    protected virtual void UpdateDetailInfo()
    {
        UpdateDetailInflictorInfo();

        detailBaseAttackInfo.Clear();
        for (int i = 0; i < baseEnemyInflictorList.Count; i++)
        {
            detailBaseAttackInfo.Append(baseEnemyInflictorList[i].inflictorInfo.ToString());
            detailBaseAttackInfo.Append('\n');
        }
        for (int i = 0; i < baseTowerInflictorList.Count; i++)
        {
            detailBaseAttackInfo.Append(baseTowerInflictorList[i].inflictorInfo.ToString());
            detailBaseAttackInfo.Append('\n');
        }

        detailSpecialAttackInfo.Clear();
        for (int i = 0; i < specialEnemyInflictorList.Count; i++)
        {
            detailSpecialAttackInfo.Append(specialEnemyInflictorList[i].inflictorInfo.ToString());
            detailSpecialAttackInfo.Append('\n');
        }
        for (int i = 0; i < specialTowerInflictorList.Count; i++)
        {
            detailSpecialAttackInfo.Append(specialTowerInflictorList[i].inflictorInfo.ToString());
            detailSpecialAttackInfo.Append('\n');
        }
    }

    protected void UpdateDetailInflictorInfo()
    {
        for (int i = 0; i < baseEnemyInflictorList.Count; i++)
            baseEnemyInflictorList[i].UpdateInflictorInfo();

        for (int i = 0; i < baseTowerInflictorList.Count; i++)
            baseTowerInflictorList[i].UpdateInflictorInfo();

        for (int i = 0; i < specialEnemyInflictorList.Count; i++)
            specialEnemyInflictorList[i].UpdateInflictorInfo();

        for (int i = 0; i < specialTowerInflictorList.Count; i++)
            specialTowerInflictorList[i].UpdateInflictorInfo();
    }

    public void MoveTower()
    {
        FollowTower followTower = _towerBuilder.followTower;

        followTower.Setup(this);
        followTower.gameObject.SetActive(true);

        Color color = _towerRenderer.color;
        color.a = onTile != null ? 0.3f : 0f;
        _towerRenderer.color = color;

        UIManager.instance.ShowTowerInfo(this);
    }

    public void StopTower()
    {
        FollowTower followTower = _towerBuilder.followTower;

        followTower.gameObject.SetActive(false);

        _towerRenderer.color = _towerColor.color;

        UIManager.instance.HideTowerInfo();
    }

    public void ReturnPool()
    {
        if (onTile != null)
            onTile = null;

        _towerRenderer.transform.rotation = Quaternion.Euler(0f, 0f, 0f);

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