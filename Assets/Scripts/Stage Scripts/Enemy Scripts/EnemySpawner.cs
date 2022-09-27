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

    private void Awake()
    {
        _roundEnemyPool = new ObjectPool<RoundEnemy>(_roundEnemyPrefab, 80);
        //InstantiateBossEnemy();
        MissionBossCooltimeSetup();
   
        StageManager.instance.onStageEnd += () => this.gameObject.SetActive(false);
        StageManager.instance.onStageResumed += () => this.gameObject.SetActive(true);
    }

    private void MissionBossCooltimeSetup()
    {
        for (int i = 0; i < 3; i++)
            _missionBossUIController.SetMissionBossCooltimeSlider(i, _missionBossRespawnCooltimes[i]);
    }



    public void SpawnEnemy()
    {
        // ������Ʈ�� ��Ȱ��ȭ�� ���°ų�, ���� ����Ű�� �ִ� Index�� Round Enemy �迭�� ������ �Ѿ��ٸ� �������� �ʴ´�.
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
            // Enemy Setup() �޼ҵ��� �Ű������� enemyData ������ ����.
            _enemy.Setup(_roundEnemyDatas[_roundEnemyIndex]);
            // _roundEnemyList ����Ʈ�� _enemy�� ������ �ִ� ��带 �߰��� -> �ʵ� ���� �����Ǿ� �ִ� Enemy�� �����ϱ� ����.
            _roundEnemyList.AddLast(_enemy.roundEnemyNode);

            yield return _waitForOneSeconds;
        }

        // Round Enemy Index�� 1 ������Ų��.
        _roundEnemyIndex++;
    }

    public void SpawnRoundBoss()
    {
        // ������Ʈ�� ��Ȱ��ȭ�� ���°ų�, ���� ����Ű�� �ִ� Index�� Round Boss Enemy �迭�� ������ �Ѿ��ٸ� �������� �ʴ´�.
        if (!this.gameObject.activeSelf || _roundBossEnemyIndex >= _roundBossEnemyDatas.Length) 
            return;

        // ������ �ʵ忡 �̹� ��ȯ�� ���¸� ��ȯ���� �ʴ´�.
        // Ȥ�ø� �ߺ� ��ȯ ����, ġ�� ����
        if (_roundBossEnemy.gameObject.activeSelf)
            return;

        _roundBossEnemy.gameObject.SetActive(true);
        _roundBossEnemy.transform.position = _wayPoints[0].position;
        _roundBossEnemy.Setup(_roundBossEnemyDatas[_roundBossEnemyIndex]);

        // Round Boss Enemy Index�� 1 ������Ų��.
        _roundBossEnemyIndex++;
    }

    public void SpawnMissionBoss(int bossLevel)
    {
        // ���� ��ȯ�Ϸ��� ������ �ʵ忡 �̹� ��ȯ�� ���¸� ��ȯ���� �ʴ´�.
        // Ȥ�ø� �ߺ� ��ȯ ����, ġ�� ����
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
 * 
 * Update : 2022/06/10 FRI 20:05
 * onDisable ��������Ʈ ����.
 * ���ӻ󿡼� Destroy �ƴٰ� �ν��ϴ� ������ GameObject.activeSelf ������Ƽ�� ���� ���� Ȱ��ȭ �� �������� Ȯ���ϴ� ������� ����.
 */
