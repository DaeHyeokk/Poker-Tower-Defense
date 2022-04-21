using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab;     // 적 프리팹
    [SerializeField]
    private float spawnTime;            // 적 생성 주기
    [SerializeField]
    private Transform[] wayPoints;      // 현재 스테이지의 이동 경로

    private void Awake()
    {
        StartCoroutine("SpawnEnemy", 40);
    }

    private IEnumerator SpawnEnemy(int createEnemyCount)
    {
        int spawnEnemy = 0;
        while(spawnEnemy++ < createEnemyCount)
        {
            GameObject clone = Instantiate(enemyPrefab);    // 적 오브젝트 생성
            Enemy enemy = clone.GetComponent<Enemy>();      // 생성한 오브젝트에서 Enemy 컴포넌트를 가져옴
            enemy.Setup(wayPoints);                         // wayPoints를 매개변수로 Setup() 메서드 호출

            yield return new WaitForSeconds(spawnTime);     // spawnTime 시간 동안 대기
        }
    }
}


/*
 * File : EnemySpawner.cs
 * First Update : 2022/04/20 WED 15:23
 * Enemy 프리팹을 통해 Enemy 오브젝트를 생성하는 EnemySpawner 오브젝트에 부착할 스크립트.
 * SpawnEnemy 코루틴 함수를 통해 spawnTime 시간을 주기로 Enemy 오브젝트를 생성한다.
 * 현재는 별도의 조건없이 무한으로 적을 생성하기 때문에 추후에 한 웨이브당 n마리 생성 하는 식으로 수정해야 할 듯.
 * 
 * Update : 2022/04/21 THU 09:26
 * 코루틴 메서드 SpawnEnemy()를 호출할 때 생성할 적의 수를 매개변수로 받도록하여 일정수 만큼의 몬스터를 소환하도록 변경
 */
