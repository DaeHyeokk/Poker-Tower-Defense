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
    //[SerializeField]
    //private GameObject towerPrefab;     // ������ƮǮ���� ������ tower ������

    private Stack<Enemy> enemyStack = new Stack<Enemy>(40);
    // private Stack<Tower> towerStack = new Stack<Tower>();    // ���� Ÿ��������Ʈ�� ��������

    private void Awake()
    {
        if (instance != this)
        {
            Destroy(gameObject);    // �ڽ��� �ı�
            return;
        }
        Initialize();
    }
    
    private void Initialize()
    {
        int newEnemyCount = 40;
        for (int i = 0; i < newEnemyCount; i++)
        {
            enemyStack.Push(CreateNewEnemyObject());
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
        Enemy retEnemy;
        // enemyStack ���ÿ� ������ Enemy�� �����ִ� ��� ���� �������� �ʰ� ���ÿ��� ����
        if(instance.enemyStack.Count > 0)
        {
            retEnemy = instance.enemyStack.Pop();
            retEnemy.transform.SetParent(null);
            retEnemy.gameObject.SetActive(true);
        }
        // enemyStack ������ ����ִ� ���
        else
        {
            retEnemy = instance.CreateNewEnemyObject();
            retEnemy.transform.SetParent(null);
            retEnemy.gameObject.SetActive(true);
        }

        return retEnemy;
    }

    public void ReturnObject(Enemy enemy)
    {
        enemy.gameObject.SetActive(false);
        enemy.transform.SetParent(instance.transform);
        instance.enemyStack.Push(enemy);
    }

}

/*
 * File : ObjectPool.cs
 * First Update : 2022/04/21 THU 13:20
 * ��Ÿ�� �߿� ������ �ı��� ����ϰ� �߻��ϰ� �� ���ɼ��� ���� Enemy, Tower ������Ʈ�� �����ϱ� ���� Object Pool ��ũ��Ʈ
 * �̱��� �������� �����Ͽ���, Stack �� �̿��Ͽ� ������Ʈ�� ȿ�������� �������� ����
 * Enemy ������Ʈ�� ��� �� ���̺�� 40������ ���͸� ������ �����̱� ������ Initialize() �� ���� 40���� Enemy ������Ʈ�� �̸� ������.
 */