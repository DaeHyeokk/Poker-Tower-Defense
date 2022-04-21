using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private float spawnTime;            // �� ���� �ֱ�
    [SerializeField]
    private Transform[] wayPoints;      // ���� ���������� �̵� ���
    [SerializeField]
    private EnemyData[] enemyDatas;

    private List<Enemy> enemies = new List<Enemy>();
    private bool isSpawn = false;
    private void Update()
    {
        // ���ӿ��� �����϶��� ���� �������� ����
        if(GameManager.instance != null && GameManager.instance.isGameover )
        {
            return;
        }

        // ���� ���͸� ���� ���� �ƴϰ�, ���� ���ڰ� 0������� ���� ���̺� ���͸� ����
        if (!isSpawn && enemies.Count <= 0)
            ActiveSpawnEnemy();

    }

    public void ActiveSpawnEnemy()
    {
        GameManager.instance.IncreaseRound();
        StartCoroutine("SpawnEnemy");
    }

    private IEnumerator SpawnEnemy()
    {
        isSpawn = true;

        yield return new WaitForSeconds(3f);    // 3�� �� Enemy ����

        int spawnEnemy = 0;
        int round = GameManager.instance.GetRound();
        while(spawnEnemy++ < 40)
        {
            Enemy enemy = ObjectPool.instance.GetEnemyObject();   // ������ ������Ʈ���� Enemy ������Ʈ�� ������
            enemy.Setup(wayPoints, enemyDatas[round - 1]);  // Enemy Setup() �޼����� �Ű������� ��������Ʈ ������ enemyData ������ ����
            enemies.Add(enemy);                             // enemies ����Ʈ�� �߰��� -> �ʵ� ���� �����ִ� Enemy�� ������ �˱� ����
            enemy.OnMissing += () => enemies.Remove(enemy); // Enemy�� ���� ���ϰ� ��ĥ ��� ����Ʈ���� ����
            enemy.OnDeath += () => enemies.Remove(enemy);   // Enemy�� ���� ��� ����Ʈ���� ����

            yield return new WaitForSeconds(spawnTime);     // spawnTime �ð� ���� ���
        }

        isSpawn = false;
    }
}


/*
 * File : EnemySpawner.cs
 * First Update : 2022/04/20 WED 15:23
 * Enemy �������� ���� Enemy ������Ʈ�� �����ϴ� EnemySpawner ������Ʈ�� ������ ��ũ��Ʈ.
 * SpawnEnemy �ڷ�ƾ �Լ��� ���� spawnTime �ð��� �ֱ�� Enemy ������Ʈ�� �����Ѵ�.
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
 * ���� bool Ÿ�� isSpawn ������ �߰��Ͽ� isSpawn ���� false�� ���� SpawnEnemy() �޼��带 ȣ���ϵ��� �����Ͽ��� 
 * Enemy�� ��������Ʈ ������� OnMissing�� onDeath�� �����Ͽ� enemies.Remove(enemy) ������ �����ϵ��� �����ν� 
 * ���� ���̰ų�, ������ �� List���� �ش� enemy ���Ҹ� �����ϵ��� ����
 */
