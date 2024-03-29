using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private Transform[] _wayPoints;

    [Header("Round Enemy")]
    [SerializeField]
    private EnemyData[] _roundEnemyDatas;
    [SerializeField]
    private GameObject _roundEnemyPrefab;
    private int _roundEnemyIndex;
    
    [Header("Round Boss Enemy")]
    [SerializeField]
    private BossEnemyData[] _roundBossEnemyDatas;
    [SerializeField]
    private RoundBossEnemy _roundBossEnemy;
    private int _roundBossEnemyIndex;

    [Header("Mission Boss")]
    [SerializeField]
    private BossEnemyData[] _missionBossEnemyDatas;
    [SerializeField]
    private MissionBossEnemy[] _missionBossEnemies;
    [SerializeField]
    private float[] _missionBossRespawnCooltimes;
    [SerializeField]
    private MissionBossUIController _missionBossUIController;

    [Header("Special Boss")]
    [SerializeField]
    private BossEnemyData[] _specialBossDatas;
    [SerializeField]
    private SpecialBossEnemy _specialBossEnemy;
    [SerializeField]
    private int _specialBossMaxLevel;

    private ObjectPool<RoundEnemy> _roundEnemyPool;
    private LinkedList<RoundEnemy> _roundEnemyList = new();

    private readonly WaitForSeconds _waitForOneSeconds = new(1f);

    public Transform[] wayPoints => _wayPoints;
    public ObjectPool<RoundEnemy> roundEnemyPool => _roundEnemyPool;
    public LinkedList<RoundEnemy> roundEnemyList => _roundEnemyList;
    public MissionBossEnemy[] missionBossEnemies => _missionBossEnemies;
    public RoundBossEnemy roundBossEnemy => _roundBossEnemy;
    public SpecialBossEnemy specialBossEnemy => _specialBossEnemy;
    public BossEnemyData[] roundBossEnemyDatas => _roundBossEnemyDatas;
    public BossEnemyData[] missionBossEnemyDatas => _missionBossEnemyDatas;
    public BossEnemyData[] specialBossEnemyDatas => _specialBossDatas;

    private void Awake()
    {
        _roundEnemyPool = new ObjectPool<RoundEnemy>(_roundEnemyPrefab, 80);
        //InstantiateBossEnemy();
        MissionBossCooltimeSetup();
    }

    private void MissionBossCooltimeSetup()
    {
        for (int i = 0; i < 3; i++)
            _missionBossUIController.SetMissionBossCooltimeSlider(i, _missionBossRespawnCooltimes[i]);
    }



    public void SpawnEnemy()
    {
        // 오브젝트가 비활성화된 상태거나, 현재 가리키고 있는 Index가 Round Enemy 배열의 범위를 넘었다면 수행하지 않는다.
        if (!this.gameObject.activeSelf || _roundEnemyIndex >= _roundEnemyDatas.Length) 
            return;
        
        StartCoroutine(SpawnEnemyCoroutine());
    }

    private IEnumerator SpawnEnemyCoroutine()
    {
        int spawnEnemy = 0;

        while (spawnEnemy++ < 40)
        {
            RoundEnemy _enemy = _roundEnemyPool.GetObject();

            _enemy.transform.position = _wayPoints[0].position;
            // Enemy Setup() 메소드의 매개변수로 enemyData 정보를 전달.
            _enemy.Setup(_roundEnemyDatas[_roundEnemyIndex]);
            // _roundEnemyList 리스트에 _enemy가 가지고 있는 노드를 추가함 -> 필드 위에 생성되어 있는 Enemy를 참조하기 위함.
            _roundEnemyList.AddLast(_enemy.roundEnemyNode);

            yield return _waitForOneSeconds;
        }

        // Round Enemy Index를 1 증가시킨다.
        _roundEnemyIndex++;
    }

    public void SpawnRoundBoss()
    {
        // 오브젝트가 비활성화된 상태거나, 현재 가리키고 있는 Index가 Round Boss Enemy 배열의 범위를 넘었다면 수행하지 않는다.
        if (!this.gameObject.activeSelf || _roundBossEnemyIndex >= _roundBossEnemyDatas.Length) 
            return;

        // 보스가 필드에 이미 소환된 상태면 소환하지 않는다.
        // 혹시모를 중복 소환 버그, 치팅 방지
        if (_roundBossEnemy.gameObject.activeSelf)
            return;

        _roundBossEnemy.gameObject.SetActive(true);
        _roundBossEnemy.transform.position = _wayPoints[0].position;
        _roundBossEnemy.Setup(_roundBossEnemyDatas[_roundBossEnemyIndex]);

        // Round Boss Enemy Index를 1 증가시킨다.
        _roundBossEnemyIndex++;
    }

    public void SpawnMissionBoss(int bossLevel)
    {
        // 현재 소환하려는 보스가 필드에 이미 소환된 상태면 소환하지 않는다.
        // 혹시모를 중복 소환 버그, 치팅 방지
        if (_missionBossEnemies[bossLevel].gameObject.activeSelf)
            return;

        _missionBossEnemies[bossLevel].gameObject.SetActive(true);
        _missionBossEnemies[bossLevel].transform.position = _wayPoints[0].position;
        _missionBossEnemies[bossLevel].Setup(_missionBossEnemyDatas[bossLevel]);
  
        _missionBossUIController.StartMissionBossCooltime(bossLevel, _missionBossRespawnCooltimes[bossLevel]);
    }

    public void LevelupSpecialBoss()
    {
        _specialBossEnemy.level++;

        if (_specialBossEnemy.level > _specialBossMaxLevel)
            return;
        else
        {
            _specialBossEnemy.gameObject.SetActive(true);
            _specialBossEnemy.Setup(_specialBossDatas[_specialBossEnemy.level - 1]);
        }
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
 * 
 * Update : 2022/06/10 FRI 20:05
 * onDisable 델리게이트 삭제.
 * 게임상에서 Destroy 됐다고 인식하는 로직을 GameObject.activeSelf 프로퍼티를 통해 현재 활성화 된 상태인지 확인하는 방식으로 변경.
 */
