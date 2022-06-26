using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementController : MonoBehaviour
{
    [SerializeField]
    private Transform[] _wayPoints;
    [SerializeField]
    private EnemySpawner _enemySpawner;

    private FieldEnemy fieldEnemy;

    private bool isStop => fieldEnemy.enemyMovementState.isStop;
    private int currentIndex 
    {
        get => fieldEnemy.enemyMovementState.currentIndex;
        set => fieldEnemy.enemyMovementState.currentIndex = value;
    }

    private float moveSpeed
    {
        get => fieldEnemy.enemyMovementState.moveSpeed;
        set => fieldEnemy.enemyMovementState.moveSpeed = value;
    }

    private Vector3 moveDirection
    {
        get => fieldEnemy.enemyMovementState.moveDirection;
        set => fieldEnemy.enemyMovementState.moveDirection = value;
    }


    private void FixedUpdate()
    {
        if (_enemySpawner.roundBossEnemy.gameObject.activeSelf)
        {
            fieldEnemy = _enemySpawner.roundBossEnemy;
            MoveEnemy();
        }

        for (int i = 0; i < _enemySpawner.missionBossEnemyList.Count; i++)
        {
            fieldEnemy = _enemySpawner.missionBossEnemyList[i];
            MoveEnemy();
        }

        for (int i = 0; i < _enemySpawner.roundEnemyList.Count; i++)
        {
            fieldEnemy = _enemySpawner.roundEnemyList[i];
            MoveEnemy();
        }

    }

    private void MoveEnemy()
    {
        // Enemy가 멈춰있는 상태라면(스턴) 작업을 수행하지 않는다.
        if (isStop) return;
        //moveDirection이 Vector3.zero 라면 Enemy가 처음 생성된 것이므로 첫번째 웨이포인트를 향해 가도록 설정한다.
        if (moveDirection == Vector3.zero)
            NextMoveTo();

        fieldEnemy.transform.position += moveDirection * moveSpeed * Time.fixedDeltaTime;

        float nowDistance = Vector3.Distance(fieldEnemy.transform.position, _wayPoints[currentIndex].position);

        // 현재 enemy의 위치와 목표 지점의 위치 사이의 거리가 Enemy가 한번에 이동하는 거리보다 가깝다면 다음 목표 지점을 탐색한다
        // Why? 목표 지점까지의 거리가 Enemy가 한번에(1 FixedUpdateTime) 이동하는 거리보다 가깝다면 Enemy가 경로를 이탈하게 되기 때문.
        if (nowDistance < moveSpeed * Time.fixedDeltaTime)
            NextMoveTo();
    }

    private void NextMoveTo()
    {
        // Enemy의 위치를 정확하게 목표 위치로 설정
        fieldEnemy.transform.position = _wayPoints[currentIndex].position;
        currentIndex++;

        if (currentIndex >= _wayPoints.Length)
            currentIndex = 0;

        Vector3 direction = (_wayPoints[currentIndex].position - fieldEnemy.transform.position).normalized;
        moveDirection = direction;
    }
}

/*
 * File : EnemyMovementController.cs
 * First Update : 2022/06/22 WED 23:30
 * 
 * Enemy 오브젝트마다 각각 Movement2D 컴포넌트를 부착하고 내부의 코루틴 메소드로 Enemy의 이동, 경로 설정 연산을 수행할 경우 Enemy의 개체 수가 매우 많아지게 됐을 때
 * 오버헤드가 매우 커지는 이슈를 해결하기 위해 생성한 스크립트.
 * 
 * 필드에 소환된 모든 FieldEnemy의 리스트를 EnemySpawner에서 받아와서 FieldEnemy의 이동 연산을 수행한다. FixedUpdate() 메소드 하나만 가지고 연산을 수행하기 때문에
 * 기존에 모든 FieldEnemy에서 각각 OnMoveCoroutine() 메소드를 수행할 때보다 성능이 훨씬 좋다.
 */