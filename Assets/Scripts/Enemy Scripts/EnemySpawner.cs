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

    private List<Enemy> _enemyList = new List<Enemy>();
    private bool _isSpawn = false;

    public float spawnTime => _spawnTime;
    public List<Enemy> enemyList => _enemyList;
    public bool isSpawn => _isSpawn;

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

        yield return new WaitForSeconds(3f);    // 3�� �� Enemy ����
        
        int spawnEnemy = 0;
        int round = GameManager.instance.round;
        while(spawnEnemy++ < 40)
        {
            Enemy enemy = ObjectPool.instance.GetEnemyObject();   // ������ ������Ʈ���� Enemy ������Ʈ�� ������
            enemy.Setup(_wayPoints, _enemyDatas[round - 1]);  // Enemy Setup() �޼����� �Ű������� ��������Ʈ ������ enemyData ������ ����
            _enemyList.Add(enemy);                             // _enemyList ����Ʈ�� �߰��� -> �ʵ� ���� �����ִ� Enemy�� ������ �˱� ����
            enemy.actionOnMissing += () => _enemyList.Remove(enemy); // Enemy�� ���� ���ϰ� ��ĥ ��� ����Ʈ���� ����
            enemy.actionOnDeath += () => _enemyList.Remove(enemy);   // Enemy�� ���� ��� ����Ʈ���� ����
            yield return new WaitForSeconds(_spawnTime);     // _spawnTime �ð� ���� ���
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
 * Enemy�� ��������Ʈ ������� OnMissing�� onDeath�� �����Ͽ� _enemyList.Remove(enemy) ������ �����ϵ��� �����ν� 
 * ���� ���̰ų�, ������ �� List���� �ش� enemy ���Ҹ� �����ϵ��� ����
 */
