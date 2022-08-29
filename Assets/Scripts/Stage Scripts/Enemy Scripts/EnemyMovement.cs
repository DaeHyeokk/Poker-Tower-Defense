using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private FieldEnemy _fieldEnemy;
    private Vector3 _moveDirection;
    private Vector3 _waypointPosition;
    private int _currentIndex;
    private bool _isStopped;

    public float moveSpeed { get; set; }

    public void Move() => _isStopped = false;
    public void Stop() => _isStopped = true;

    private void Awake()
    {
        _fieldEnemy = GetComponent<FieldEnemy>();
    }

    private void OnEnable()
    {
        _currentIndex = 0;
        _moveDirection = Vector3.zero;
    }

    private void Update()
    {
        // Enemy가 정지 상태가 아니라면 이동한다.
        if (!_isStopped)
            MoveEnemy();
    }

    private void MoveEnemy()
    {
        //moveDirection이 Vector3.zero 라면 Enemy가 처음 생성된 것이므로 바로 다음 웨이포인트를 향해 이동한다.
        if (_moveDirection == Vector3.zero)
        {
            MoveToNextWayPoint();
            return;
        }

        float nowDistance = Vector2.Distance(transform.position, _waypointPosition);
        float nextMoveDist = moveSpeed * Time.deltaTime;

        // 현재 enemy의 위치와 목표 지점의 위치 사이의 거리가 Enemy가 한번에 이동하는 거리보다 가깝다면 다음 목표 지점을 탐색한다
        // Why? 목표 지점까지의 거리가 Enemy가 한번에 이동하는 거리보다 가깝다면 Enemy가 경로를 이탈하게 되기 때문.
        if (nowDistance < nextMoveDist)
            MoveToNextWayPoint(nextMoveDist + -nowDistance);
        else
            transform.position += _moveDirection * moveSpeed * Time.deltaTime;
    }

    private void MoveToNextWayPoint(float overDist = 0f)
    {
        Vector3 wayPoint () => _fieldEnemy.enemySpawner.wayPoints[_currentIndex].position;

        // Enemy의 위치를 정확하게 목표 위치로 설정
        transform.position = wayPoint();
        _currentIndex++;

        if (_currentIndex >= _fieldEnemy.enemySpawner.wayPoints.Length)
            _currentIndex = 0;

        _waypointPosition = wayPoint();

        Vector3 direction = (_waypointPosition - transform.position).normalized;
        _moveDirection = direction;

        // Enemy와 Waypoint 사이의 거리보다 멀리 이동하는 거리만큼 이동시킨다. (코너를 도는 느낌)
        transform.position += direction * overDist;
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
 * 
 * Update : 2022/06/27 MON 18:55
 * File : EnemyMovement.cs 로 변경
 * 
 * OnMoveCoroutine() 을 수행하면 성능이 떨어졌던 이유가 FieldEnemy 개체수에 비례해서 코루틴 딜레이를 호출하는양이 많아지기 때문이었음.
 * 코루틴이 아닌 Update()나 FixedUpdate()로 FieldEnemy의 이동을 수행할 경우 하나의 스크립트에서 FieldEnemy 전체의 이동을 수행하는거보다 효율이 좋고, 가독성 또한 좋기 때문에
 * FieldEnemy 개체마다 EnemyMovement 컴포넌트를 할당받아 각각 이동을 수행하는 로직으로 변경하였음.
 */