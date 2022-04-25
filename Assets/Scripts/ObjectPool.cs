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
    //[SerializeField]
    //private GameObject towerPrefab;     // 오브젝트풀에서 관리할 tower 프리팹

    private Stack<Enemy> enemyStack = new Stack<Enemy>(40);
    // private Stack<Tower> towerStack = new Stack<Tower>();    // 아직 타워오브젝트는 구현안함

    private void Awake()
    {
        if (instance != this)
        {
            Destroy(gameObject);    // 자신을 파괴
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
        // enemyStack 스택에 생성된 Enemy가 남아있는 경우 새로 생성하지 않고 스택에서 빼냄
        if(instance.enemyStack.Count > 0)
        {
            retEnemy = instance.enemyStack.Pop();
            retEnemy.transform.SetParent(null);
            retEnemy.gameObject.SetActive(true);
        }
        // enemyStack 스택이 비어있는 경우
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
 * 런타임 중에 생성과 파괴가 빈번하게 발생하게 될 가능성이 높은 Enemy, Tower 오브젝트를 재사용하기 위한 Object Pool 스크립트
 * 싱글톤 패턴으로 구현하였고, Stack 를 이용하여 오브젝트를 효율적으로 관리도록 구현
 * Enemy 오브젝트의 경우 한 웨이브당 40마리의 몬스터를 생성할 예정이기 때문에 Initialize() 를 통해 40개의 Enemy 오브젝트를 미리 생성함.
 */