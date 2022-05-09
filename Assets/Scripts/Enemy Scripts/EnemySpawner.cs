using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private float _spawnTime;            // �� ���� �ֱ�
    [Header("Round Enemy")]
    [SerializeField]
    private EnemyData[] _roundEnemyDatas;
    [SerializeField]
    private GameObject _roundEnemyPrefab;

    [Header("Round Boss")]
    [SerializeField]
    private EnemyData[] _roundBossEnemyDatas;
    [SerializeField]
    private GameObject _roundBossEnemyPrefab;

    [Header("Mission Boss")]
    [SerializeField]
    private EnemyData[] _missionBossEnemyDatas;
    [SerializeField]
    private GameObject _missionBossEnemyPrefab;

    [Header("Planet Boss")]
    private Enemy _planetBossEnemy;


    private ObjectPool<Enemy> _roundEnemyPool;
    private List<Enemy> _roundEnemyList;

    private Enemy _roundBossEnemy;
    private Enemy[] _missionBossEnemies;
    private List<Enemy> _missionBossEnemyList;

    private bool _isSpawn;

    private WaitForSeconds _waitSpawnTime;

    public ObjectPool<Enemy> roundEnemyPool => _roundEnemyPool;
    public List<Enemy> roundEnemyList => _roundEnemyList;
    public List<Enemy> missionBossEnemyList => _missionBossEnemyList;
    public Enemy planetBossEnemy => _planetBossEnemy;
    public bool isSpawn => _isSpawn;

    private void Awake()
    {
        _roundEnemyPool = new ObjectPool<Enemy>(_roundEnemyPrefab, 20);
        _roundEnemyList = new List<Enemy>();
        _missionBossEnemyList = new List<Enemy>();

        InstantiateBossEnemy();

        _isSpawn = false;
        _waitSpawnTime = new WaitForSeconds(_spawnTime);
    }

    private void InstantiateBossEnemy()
    {
        _roundBossEnemy = Instantiate(_roundBossEnemyPrefab).GetComponent<Enemy>();
        _roundBossEnemy.gameObject.SetActive(false);

        _missionBossEnemies = new Enemy[3];
        for (int i = 0; i < 3; i++)
        {
            _missionBossEnemies[i] = Instantiate(_missionBossEnemyPrefab).GetComponent<Enemy>();
            _missionBossEnemies[i].gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        // ���ӿ��� �����϶��� ���� �������� ����
        if(GameManager.instance != null && GameManager.instance.isGameover )
        {
            return;
        }

        // ���� ���͸� ���� ���� �ƴϰ�, ���� ���ڰ� 0������� ���� ���̺� ���͸� ����
        if (!_isSpawn && _roundEnemyList.Count <= 0)
            SpawnEnemy();

    }

    public void SpawnEnemy()
    {
        GameManager.instance.IncreaseRound();
        StartCoroutine(SpawnEnemyCoroutine());
    }

    private IEnumerator SpawnEnemyCoroutine()
    {
        _isSpawn = true;

        // 3�� �� Enemy ����
        yield return new WaitForSeconds(3f);

        int spawnEnemy = 0;
        int round = GameManager.instance.round;

        if (round % 10 != 0)
        {
            while (spawnEnemy++ < 40)
            {
                Enemy _enemy = _roundEnemyPool.GetObject();
                // Enemy Setup() �޼����� �Ű������� ��������Ʈ ������ enemyData ������ ����
                _enemy.Setup(_roundEnemyDatas[round - 1]);
                // _enemyList ����Ʈ�� �߰��� -> �ʵ� ���� �����ִ� Enemy�� ������ �˱� ����
                _roundEnemyList.Add(_enemy);

                // _spawnTime �ð� ���� ���
                yield return _waitSpawnTime;
            }
        }
        // 10�� ��� ����� ���� ������.
        else
        {
            _roundBossEnemy.gameObject.SetActive(true);
            _roundBossEnemy.Setup(_roundBossEnemyDatas[round / 11]);
            _roundEnemyList.Add(_roundBossEnemy);
        }

        _isSpawn = false;
    }

    public void SpawnMissionBoss(int bossLevel)
    {
        // ���� ��ȯ�Ϸ��� ������ �ʵ忡 �̹� ��ȯ�� ���¸� ��ȯ���� �ʴ´�.
        // Ȥ�ø� �ߺ� ��ȯ ���� ����
        if (_missionBossEnemies[bossLevel].gameObject.activeInHierarchy)
            return;

        _missionBossEnemies[bossLevel].gameObject.SetActive(true);
        _missionBossEnemies[bossLevel].Setup(_missionBossEnemyDatas[bossLevel]);
        _missionBossEnemyList.Add(_missionBossEnemies[bossLevel]);
    }

}


/*
 * File : EnemySpawner.cs
 * First Update : 2022/04/20 WED 15:23
 * Enemy �������� ���� Enemy ������Ʈ�� �����ϴ� EnemySpawner ������Ʈ�� ������ ��ũ��Ʈ.
 * SpawnEnemy �ڷ�ƾ �Լ��� ���� _spawnTime �ð��� �ֱ�� Enemy ������Ʈ�� �����Ѵ�.
 * ����� ������ ���Ǿ��� �������� ���� �����ϱ� ������ ���Ŀ� �� ���̺�� n���� ���� �ϴ� ������ �����ؾ� �� ��.
 * 
 * Update : 2022/04/21 THU 13:20
 * �ڷ�ƾ �޼��� SpawnEnemy()�� ȣ���� �� ������ ���� ���� �Ű������� �޵����Ͽ� ������ ��ŭ�� ���͸� ��ȯ�ϵ��� ����
 * ������Ʈ Ǯ�� ����� �����ϱ� ���� EnemySpawner���� Enemy �������� ���� �������� �ʰ� ObjectPool���� Enemy ������Ʈ�� �޾ƿ��� �������� ����
 * 
 * Update : 2022/04/22 FRI 02:08
 * ������ ���۵Ǹ� 3�� �� ���Ͱ� �����ǰ� ���ķ� ���Ͱ� ��� �������� 3�� �� ���� ���� ���Ͱ� �����ǵ��� ����
 * ���Ͱ� ��� �������� ���� �Ǻ��ϴ� ������ List�� enemy ������Ʈ�� ��Ƽ� List�� ���� ������ Ȯ���ϴ� ������� ������
 * EnemyData �迭�� ���� ���帶�� �׿� �����ϴ� �迭 �ε����� �����Ͽ� ������ �ɷ�ġ�� �ٸ��� Setup() ��
 * �÷��̾��� Ÿ���� �����Ͽ� Enemy�� �����ǰ� ���� Enemy�� ������ ���� ��ƹ��� ��� SpawnEnemy() �޼��带 �ߺ� ȣ���� ����� ����
 * ���� bool Ÿ�� _isSpawn ������ �߰��Ͽ� _isSpawn ���� false�� ���� SpawnEnemy() �޼��带 ȣ���ϵ��� �����Ͽ��� 
 * Enemy�� ��������Ʈ ������� onMissing�� onDeath�� �����Ͽ� _enemyList.Remove(enemy) ������ �����ϵ��� �����ν� 
 * ���� ���̰ų�, ������ �� List���� �ش� enemy ���Ҹ� �����ϵ��� ����
 * 
 * Update : 2022/05/01 SUN 15:18
 * ���ӻ󿡼� Destroy �ƴٰ� �ν��ϵ��� �ϴ� ������ Action onDisable ��������Ʈ�� ������Ű�� ������� ����.
 */
