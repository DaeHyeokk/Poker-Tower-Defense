using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab;     // �� ������
    [SerializeField]
    private float spawnTime;            // �� ���� �ֱ�
    [SerializeField]
    private Transform[] wayPoints;      // ���� ���������� �̵� ���

    private void Awake()
    {
        StartCoroutine("SpawnEnemy", 40);
    }

    private IEnumerator SpawnEnemy(int createEnemyCount)
    {
        int spawnEnemy = 0;
        while(spawnEnemy++ < createEnemyCount)
        {
            GameObject clone = Instantiate(enemyPrefab);    // �� ������Ʈ ����
            Enemy enemy = clone.GetComponent<Enemy>();      // ������ ������Ʈ���� Enemy ������Ʈ�� ������
            enemy.Setup(wayPoints);                         // wayPoints�� �Ű������� Setup() �޼��� ȣ��

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
 * Update : 2022/04/21 THU 09:26
 * �ڷ�ƾ �޼��� SpawnEnemy()�� ȣ���� �� ������ ���� ���� �Ű������� �޵����Ͽ� ������ ��ŭ�� ���͸� ��ȯ�ϵ��� ����
 */
