using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private float spawnTime;            // �� ���� �ֱ�
    [SerializeField]
    private Transform[] wayPoints;      // ���� ���������� �̵� ���

    public void ActiveSpawnEnemy()
    {
        StartCoroutine("SpawnEnemy", 60);
    }

    private IEnumerator SpawnEnemy(int spawnEnemyCount)
    {
        int spawnEnemy = 0;
        while(spawnEnemy++ < spawnEnemyCount)
        {
            Enemy enemy = ObjectPool.instance.GetEnemyObject();   // ������ ������Ʈ���� Enemy ������Ʈ�� ������
            enemy.Setup(wayPoints);
            yield return new WaitForSeconds(spawnTime);     // spawnTime �ð� ���� ���
        }
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
 */
