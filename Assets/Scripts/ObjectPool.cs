using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    private static ObjectPool m_instance;
    public static ObjectPool instance
    {
        get
        {
            if(m_instance == null)
            {
                // ������ ObjectPool ������Ʈ�� ã�� �Ҵ�
                m_instance = FindObjectOfType<ObjectPool>();
            }

            return m_instance;
        }
    }

    [SerializeField]
    private GameObject enemyPrefab;     // ������ƮǮ���� ������ enemy ������
    [SerializeField]
    private GameObject towerPrefab;     // ������ƮǮ���� ������ tower ������

    private Queue<Enemy> enemyQueue = new Queue<Enemy>();
    // private Queue<Tower> towerQueue = new Queue<Tower>();    // ���� Ÿ��������Ʈ�� ��������

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        int newEnemyCount = 40;
        for (int i = 0; i < newEnemyCount; i++)
        {
            enemyQueue.Enqueue(CreateNewEnemyObject());
        }
    }

    private Enemy CreateNewEnemyObject()
    {
        Enemy newEnemy = Instantiate(enemyPrefab).GetComponent<Enemy>();
        newEnemy.gameObject.SetActive(false);
        newEnemy.transform.SetParent(this.transform);

        return newEnemy;
    }

    public Enemy GetEnemyObject()
    {
        // enemyQueue ť�� ������ Enemy�� �����ִ� ��� ���� �������� �ʰ� ť���� ����
        if(instance.enemyQueue.Count > 0)
        {
            Enemy retEnemy = instance.enemyQueue.Dequeue();
            retEnemy.transform.SetParent(null);
            retEnemy.gameObject.SetActive(true);
            return retEnemy;
        }
        // enemyQueue ť�� ����ִ� ���
        else
        {
            Enemy retEnemy = instance.CreateNewEnemyObject();
            retEnemy.transform.SetParent(null);
            retEnemy.gameObject.SetActive(true);
            return retEnemy;
        }
    }

    public void ReturnObject(Enemy enemy)
    {
        enemy.gameObject.SetActive(false);
        enemy.transform.SetParent(instance.transform);
        instance.enemyQueue.Enqueue(enemy);
    }
}

/*
 * File : ObjectPool.cs
 * First Update : 2022/04/21 THU 13:20
 * ��Ÿ�� �߿� ������ �ı��� ����ϰ� �߻��ϰ� �� ���ɼ��� ���� Enemy, Tower ������Ʈ�� �����ϱ� ���� Object Pool ��ũ��Ʈ
 * �̱��� �������� �����Ͽ���, Queue �� �̿��Ͽ� ������Ʈ�� ȿ�������� �������� ����
 * Enemy ������Ʈ�� ��� �� ���̺�� 40������ ���͸� ������ �����̱� ������ Initialize() �� ���� 40���� Enemy ������Ʈ�� �̸� ������.
 */