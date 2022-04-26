using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    private static ObjectPool _instance;
    public static ObjectPool instance
    {
        get
        {
            if(_instance == null)
            {
                // ������ ObjectPool ������Ʈ�� ã�� �Ҵ�
                _instance = FindObjectOfType<ObjectPool>();
            }

            return _instance;
        }
    }
 
    [SerializeField]
    private GameObject _enemyPrefab;     // ������ƮǮ���� ������ enemy ������
    //[SerializeField]
    //private GameObject _towerPrefab;     // ������ƮǮ���� ������ tower ������

    private Stack<Enemy> _enemyStack;
    // private Stack<Tower> towerStack = new Stack<Tower>();    // ���� Ÿ��������Ʈ�� ��������

    private void Awake()
    {
        if (instance != this)
        {
            Destroy(gameObject);    // �ڽ��� �ı�
            return;
        }

        _enemyStack = new Stack<Enemy>(40);

        Initialize();
    }
    
    private void Initialize()
    {
        int newEnemyCount = 40;
        for (int i = 0; i < newEnemyCount; i++)
            _enemyStack.Push(CreateNewEnemyObject());
    }

    private Enemy CreateNewEnemyObject()
    {
        Enemy newEnemy = Instantiate(_enemyPrefab).GetComponent<Enemy>();
        newEnemy.gameObject.SetActive(false);
        newEnemy.transform.SetParent(this.transform);

        return newEnemy;
    }
    public Enemy GetEnemyObject()
    {
        Enemy retEnemy;
        // enemyStack ���ÿ� ������ Enemy�� �����ִ� ��� ���� �������� �ʰ� ���ÿ��� ����
        if(instance._enemyStack.Count > 0)
        {
            retEnemy = instance._enemyStack.Pop();
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
        instance._enemyStack.Push(enemy);
    }

}

/*
 * File : ObjectPool.cs
 * First Update : 2022/04/21 THU 13:20
 * ��Ÿ�� �߿� ������ �ı��� ����ϰ� �߻��ϰ� �� ���ɼ��� ���� Enemy, Tower ������Ʈ�� �����ϱ� ���� Object Pool ��ũ��Ʈ
 * �̱��� �������� �����Ͽ���, Stack �� �̿��Ͽ� ������Ʈ�� ȿ�������� �������� ����
 * Enemy ������Ʈ�� ��� �� ���̺�� 40������ ���͸� ������ �����̱� ������ Initialize() �� ���� 40���� Enemy ������Ʈ�� �̸� ������.
 */