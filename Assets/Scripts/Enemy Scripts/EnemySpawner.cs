using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private float _spawnTime;            // �� ���� �ֱ�
    [SerializeField]
    private Transform[] _wayPoints;      // ���� ���������� �̵� ���
    [SerializeField]
    private EnemyData[] _enemyDatas;
    [SerializeField]
    private GameObject _enemyPrefab;

   // private Enemy _enemy;
    private ObjectPool<Enemy> _enemyPool;
    private List<Enemy> _enemyList;
    private bool _isSpawn;

    private WaitForSeconds _waitSpawnTime;

    public ObjectPool<Enemy> enemyPool => _enemyPool;
    public float spawnTime => _spawnTime;
    public List<Enemy> enemyList => _enemyList;
    public bool isSpawn => _isSpawn;

    private void Awake()
    {
        _enemyPool = new ObjectPool<Enemy>(_enemyPrefab, 40);
        _enemyList = new List<Enemy>();
        _isSpawn = false;
        _waitSpawnTime = new WaitForSeconds(spawnTime);
    }

    private void Update()
    {
        // ���ӿ��� �����϶��� ���� �������� ����
        if(GameManager.instance != null && GameManager.instance.isGameover )
        {
            return;
        }

        // ���� ���͸� ���� ���� �ƴϰ�, ���� ���ڰ� 0������� ���� ���̺� ���͸� ����
        if (!_isSpawn && _enemyList.Count <= 0)
            ActiveSpawnEnemy();

    }

    public void ActiveSpawnEnemy()
    {
        GameManager.instance.IncreaseRound();
        StartCoroutine("SpawnEnemy");
    }

    private IEnumerator SpawnEnemy()
    {
        _isSpawn = true;

        // 3�� �� Enemy ����
        yield return new WaitForSeconds(3f);

        int spawnEnemy = 0;
        int round = GameManager.instance.round;
        while(spawnEnemy++ < 40)
        {
            Enemy _enemy = _enemyPool.GetObject();
            // Enemy Setup() �޼����� �Ű������� ��������Ʈ ������ enemyData ������ ����
            _enemy.Setup(_wayPoints, _enemyDatas[round - 1]);
            // _enemyList ����Ʈ�� �߰��� -> �ʵ� ���� �����ִ� Enemy�� ������ �˱� ����
            _enemyList.Add(_enemy);

            // _spawnTime �ð� ���� ���
            yield return _waitSpawnTime;
        }

        _isSpawn = false;
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
