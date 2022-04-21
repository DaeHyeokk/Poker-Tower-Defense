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
                // 씬에서 ObjectPool 오브젝트를 찾아 할당
                m_instance = FindObjectOfType<ObjectPool>();
            }

            return m_instance;
        }
    }

    [SerializeField]
    private GameObject enemyPrefab;     // 오브젝트풀에서 관리할 enemy 프리팹
    [SerializeField]
    private GameObject towerPrefab;     // 오브젝트풀에서 관리할 tower 프리팹

    private Queue<Enemy> enemyQueue = new Queue<Enemy>();
    // private Queue<Tower> towerQueue = new Queue<Tower>();    // 아직 타워오브젝트는 구현안함

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
        // enemyQueue 큐에 생성된 Enemy가 남아있는 경우 새로 생성하지 않고 큐에서 빼냄
        if(instance.enemyQueue.Count > 0)
        {
            Enemy retEnemy = instance.enemyQueue.Dequeue();
            retEnemy.transform.SetParent(null);
            retEnemy.gameObject.SetActive(true);
            return retEnemy;
        }
        // enemyQueue 큐가 비어있는 경우
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
 * 런타임 중에 생성과 파괴가 빈번하게 발생하게 될 가능성이 높은 Enemy, Tower 오브젝트를 재사용하기 위한 Object Pool 스크립트
 * 싱글톤 패턴으로 구현하였고, Queue 를 이용하여 오브젝트를 효율적으로 관리도록 구현
 * Enemy 오브젝트의 경우 한 웨이브당 40마리의 몬스터를 생성할 예정이기 때문에 Initialize() 를 통해 40개의 Enemy 오브젝트를 미리 생성함.
 */