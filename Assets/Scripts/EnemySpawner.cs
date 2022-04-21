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
        StartCoroutine("SpawnEnemy");
    }

    private IEnumerator SpawnEnemy()
    {
        while(true)
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
 */
