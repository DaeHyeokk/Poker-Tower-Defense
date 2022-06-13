using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private float _spawnTime;            // 적 생성 주기
    [SerializeField]
    private Transform[] _wayPoints; // 필드 몬스터 이동경로 배열

    [Header("Round Enemy")]
    [SerializeField]
    private EnemyData[] _roundEnemyDatas;
    [SerializeField]
    private GameObject _roundEnemyPrefab;
    [SerializeField]
    private GameObject _roundBossEnemyPrefab;

    [Header("Mission Boss")]
    [SerializeField]
    private EnemyData[] _missionBossEnemyDatas;
    [SerializeField]
    private GameObject[] _missionBossEnemyPrefabs;
    [SerializeField]
    private float[] _missionBossRespawnCooltimes;
    [SerializeField]
    private MissionBossUIController _missionBossUIController;

    [Header("Special Boss")]
    [SerializeField]
    private SpecialBossEnemy _specialBossEnemy;
    [SerializeField]
    private Transform _specialBossSpawnPoint;

    private ObjectPool<RoundEnemy> _roundEnemyPool;
    private RoundBossEnemy _roundBossEnemy;
    private List<FieldEnemy> _roundEnemyList;

    private MissionBossEnemy[] _missionBossEnemies;
    private List<MissionBossEnemy> _missionBossEnemyList;

    private bool _isSpawn;
    private WaitForSeconds _waitSpawnTime;
    private WaitForSeconds _waitUpdateTime;

    public ObjectPool<RoundEnemy> roundEnemyPool => _roundEnemyPool;
    public List<FieldEnemy> roundEnemyList => _roundEnemyList;
    public List<MissionBossEnemy> missionBossEnemyList => _missionBossEnemyList;
    public SpecialBossEnemy specialBossEnemy => _specialBossEnemy;
    public bool isSpawn => _isSpawn;

    private void Awake()
    {
        _roundEnemyPool = new ObjectPool<RoundEnemy>(_roundEnemyPrefab, 20);
        _roundEnemyList = new List<FieldEnemy>();
        _missionBossEnemyList = new List<MissionBossEnemy>();

        InstantiateBossEnemy();
        MissionBossCooltimeSetup();

        _isSpawn = false;
        _waitSpawnTime = new WaitForSeconds(_spawnTime);
        _waitUpdateTime = new WaitForSeconds(1f);
    }

    private void InstantiateBossEnemy()
    {
        _roundBossEnemy = Instantiate(_roundBossEnemyPrefab).GetComponent<RoundBossEnemy>();
        _roundBossEnemy.gameObject.SetActive(false);

        _missionBossEnemies = new MissionBossEnemy[3];
        for (int i = 0; i < 3; i++)
        {
            _missionBossEnemies[i] = Instantiate(_missionBossEnemyPrefabs[i]).GetComponent<MissionBossEnemy>();
            _missionBossEnemies[i].gameObject.SetActive(false);
        }
    }

    private void MissionBossCooltimeSetup()
    {
        for (int i = 0; i < 3; i++)
            _missionBossUIController.SetMissionBossCooltimeSlider(i, _missionBossRespawnCooltimes[i]);
    }

    private void Update()
    {
        // 게임오버 상태일때는 적을 생성하지 않음
        if(GameManager.instance != null && GameManager.instance.isGameover )
            return;

        // 현재 몬스터를 생성 중이 아니고, 적의 숫자가 0마리라면 다음 웨이브 몬스터를 생성
        if (!_isSpawn && _roundEnemyList.Count <= 0 && GameManager.instance.wave < 40)
            SpawnEnemy();

    }

    public void SpawnEnemy()
    {
        GameManager.instance.IncreaseWave();
        StartCoroutine(SpawnEnemyCoroutine());
    }

    private IEnumerator SpawnEnemyCoroutine()
    {
        _isSpawn = true;

        int spawnEnemy = 0;
        int wave = GameManager.instance.wave;

        if (wave % 10 != 0)
        {
            while (spawnEnemy++ < 40)
            {
                FieldEnemy _enemy = _roundEnemyPool.GetObject();
                // Enemy Setup() 메서드의 매개변수로 웨이포인트 정보와 enemyData 정보를 전달
                _enemy.Setup(_wayPoints, _roundEnemyDatas[wave - 1]);
                // _roundEnemyList 리스트에 추가함 -> 필드 위에 남아있는 Enemy의 개수를 알기 위함
                _roundEnemyList.Add(_enemy);

                // _spawnTime 시간 동안 대기
                yield return _waitSpawnTime;
            }
        }
        // 10의 배수 라운드는 보스 라운드임.
        else
        {
            _roundBossEnemy.gameObject.SetActive(true);
            _roundBossEnemy.Setup(_wayPoints, _roundEnemyDatas[wave - 1]);
            _roundEnemyList.Add(_roundBossEnemy);
        }

        _isSpawn = false;
    }

    public void SpawnMissionBoss(int bossLevel)
    {
        // 현재 소환하려는 보스가 필드에 이미 소환된 상태면 소환하지 않는다.
        // 혹시모를 중복 소환 버그 방지
        if (_missionBossEnemies[bossLevel].gameObject.activeInHierarchy)
            return;

        _missionBossEnemies[bossLevel].gameObject.SetActive(true);
        _missionBossEnemies[bossLevel].Setup(_wayPoints, _missionBossEnemyDatas[bossLevel]);
        _missionBossEnemyList.Add(_missionBossEnemies[bossLevel]);

        _missionBossUIController.StartMissionBossCooltime(bossLevel, _missionBossRespawnCooltimes[bossLevel]);
    }
}


/*
 * File : EnemySpawner.cs
 * First Update : 2022/04/20 WED 15:23
 * Enemy 프리팹을 통해 Enemy 오브젝트를 생성하는 EnemySpawner 오브젝트에 부착할 스크립트.
 * SpawnEnemy 코루틴 함수를 통해 _spawnTime 시간을 주기로 Enemy 오브젝트를 생성한다.
 * 현재는 별도의 조건없이 무한으로 적을 생성하기 때문에 추후에 한 웨이브당 n마리 생성 하는 식으로 수정해야 할 듯.
 * 
 * Update : 2022/04/21 THU 13:20
 * 코루틴 메서드 SpawnEnemy()를 호출할 때 생성할 적의 수를 매개변수로 받도록하여 일정수 만큼의 몬스터를 소환하도록 변경
 * 오브젝트 풀링 기법을 적용하기 위해 EnemySpawner에서 Enemy 프리팹을 직접 생성하지 않고 ObjectPool에서 Enemy 오브젝트를 받아오는 로직으로 변경
 * 
 * Update : 2022/04/22 FRI 02:08
 * 게임이 시작되면 3초 후 몬스터가 생성되고 이후로 몬스터가 모두 없어지면 3초 후 다음 라운드 몬스터가 생성되도록 변경
 * 몬스터가 모두 없어지는 것을 판별하는 로직은 List에 enemy 오브젝트를 담아서 List의 원소 갯수를 확인하는 방식으로 구현함
 * EnemyData 배열을 통해 라운드마다 그에 대응하는 배열 인덱스로 접근하여 몬스터의 능력치를 다르게 Setup() 함
 * 플레이어의 타워가 강력하여 Enemy가 리젠되고 다음 Enemy가 나오기 전에 잡아버릴 경우 SpawnEnemy() 메서드를 중복 호출할 우려가 있음
 * 따라서 bool 타입 _isSpawn 변수를 추가하여 _isSpawn 값이 false일 때만 SpawnEnemy() 메서드를 호출하도록 구현하였음 
 * Enemy의 델리게이트 멤버변수 onMissing과 onDeath를 구독하여 _enemyList.Remove(enemy) 구문을 수행하도록 함으로써 
 * 적을 죽이거나, 놓쳤을 때 List에서 해당 enemy 원소를 삭제하도록 구현
 * 
 * Update : 2022/05/01 SUN 15:18
 * 게임상에서 Destroy 됐다고 인식하도록 하는 로직을 Action onDisable 델리게이트에 구독시키는 방식으로 변경.
 */
