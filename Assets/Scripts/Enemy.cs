using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Transform[] wayPoints;  // 이동경로 좌표 배열

    private int wayPointCount;     // 이동경로 개수
    private int currentIndex;   // 현재 목표지점 인덱스
    private EnemyMovement movement2D;  // 오브젝트 이동 제어

    private void Awake()
    {
        movement2D = GetComponent<EnemyMovement>();
    }
    public void Setup(Transform[] _wayPoints)
    {
        if(wayPoints == null)
        {
            wayPoints = _wayPoints;
            wayPointCount = wayPoints.Length;
        }

        currentIndex = 0;
        // Enemy 등장 위치를 첫번째 wayPoint 좌표로 설정
        transform.position = this.wayPoints[currentIndex].position;
        // 적 이동/목표 지점 설정 코루틴 함수 시작
        StartCoroutine("OnMove");
    }

    private IEnumerator OnMove()
    {
        NextMoveTo();

        while (true)
        {
            // 현재 enemy의 위치와 목표 지점의 위치 사이의 거리가 0.1보다 가깝다면 다음 목표 지점을 탐색한다
            // 0.2f에 MoveSpeed를 곱해주는 이유는 이동 속도가 빠른 enemy일 경우 한 프레임에 0.1f보다 많이 이동할 수도 있기 때문에
            // if 조건문에 걸리지 않고 목표 경로를 잃은 enemy 오브젝트가 발생할 수 있기 때문이다.
            if (Vector3.Distance(transform.position, wayPoints[currentIndex].position) < 0.1f * movement2D.MoveSpeed)
                NextMoveTo();

            // 1프레임 대기
            yield return null;
        }
    }

    private void NextMoveTo()
    {
        // 다음 목표 지점이 남아있다면
        if (currentIndex < wayPointCount - 1)
        {
            // Enemy의 위치를 정확하게 목표 위치로 설정
            transform.position = wayPoints[currentIndex].position;
            currentIndex++;

            Vector3 direction = (wayPoints[currentIndex].position - transform.position).normalized;
            movement2D.MoveTo(direction);
        }
        // 현재 위치가 마지막 목표 지점이라면
        else
        {
            StopCoroutine("OnMove");
            ObjectPool.instance.ReturnObject(this);
        }
    }
}

/*
 * File : Enemy.cs
 * First Update : 2022/04/20 WED 14:57
 * Enemy 오브젝트가 생성될 때 이동할 Waypoint 좌표를 설정하고,
 * 목표 지점을 순서대로 이동하며 마지막 WayPoint에 도착했을 시 오브젝트를 파괴한다.
 * 
 * Update : 2022/04/21 THU 13:20
 * 오브젝트풀링 기법을 적용하였기 때문에 이전 라운드에서 사용한 오브젝트를 계속해서 재사용하게 됨
 * 따라서 Setup() 메서드에서 이전에 WayPoints 배열에 할당된 값이 있는지 확인하는 로직을 추가하여 불필요한 연산을 하지 않도록 변경하였고,
 * WayPoints 배열의 첫번째 인덱스부터 다시 이동하기 위해 이전에 사용했던 currentIndex 값을 0으로 바꾸는 로직을 추가함
 */
