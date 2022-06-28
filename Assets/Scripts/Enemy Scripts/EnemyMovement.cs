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
        // Enemy�� ���� ���°� �ƴ϶�� �̵��Ѵ�.
        if (!_isStopped)
            MoveEnemy();
    }

    private void MoveEnemy()
    {
        //moveDirection�� Vector3.zero ��� Enemy�� ó�� ������ ���̹Ƿ� �ٷ� ���� ��������Ʈ�� ���� �̵��Ѵ�.
        if (_moveDirection == Vector3.zero)
        {
            MoveToNextWayPoint();
            return;
        }

        float nowDistance = Vector2.Distance(transform.position, _waypointPosition);
        float nextMoveDist = moveSpeed * Time.deltaTime;

        // ���� enemy�� ��ġ�� ��ǥ ������ ��ġ ������ �Ÿ��� Enemy�� �ѹ��� �̵��ϴ� �Ÿ����� �����ٸ� ���� ��ǥ ������ Ž���Ѵ�
        // Why? ��ǥ ���������� �Ÿ��� Enemy�� �ѹ��� �̵��ϴ� �Ÿ����� �����ٸ� Enemy�� ��θ� ��Ż�ϰ� �Ǳ� ����.
        if (nowDistance < nextMoveDist)
            MoveToNextWayPoint(nextMoveDist + -nowDistance);
        else
            transform.position += _moveDirection * moveSpeed * Time.deltaTime;
    }

    private void MoveToNextWayPoint(float overDist = 0f)
    {
        Vector3 wayPoint () => _fieldEnemy.enemySpawner.wayPoints[_currentIndex].position;

        // Enemy�� ��ġ�� ��Ȯ�ϰ� ��ǥ ��ġ�� ����
        transform.position = wayPoint();
        _currentIndex++;

        if (_currentIndex >= _fieldEnemy.enemySpawner.wayPoints.Length)
            _currentIndex = 0;

        _waypointPosition = wayPoint();

        Vector3 direction = (_waypointPosition - transform.position).normalized;
        _moveDirection = direction;

        // Enemy�� Waypoint ������ �Ÿ����� �ָ� �̵��ϴ� �Ÿ���ŭ �̵���Ų��. (�ڳʸ� ���� ����)
        transform.position += direction * overDist;
    }
}

/*
 * File : EnemyMovementController.cs
 * First Update : 2022/06/22 WED 23:30
 * 
 * Enemy ������Ʈ���� ���� Movement2D ������Ʈ�� �����ϰ� ������ �ڷ�ƾ �޼ҵ�� Enemy�� �̵�, ��� ���� ������ ������ ��� Enemy�� ��ü ���� �ſ� �������� ���� ��
 * ������尡 �ſ� Ŀ���� �̽��� �ذ��ϱ� ���� ������ ��ũ��Ʈ.
 * 
 * �ʵ忡 ��ȯ�� ��� FieldEnemy�� ����Ʈ�� EnemySpawner���� �޾ƿͼ� FieldEnemy�� �̵� ������ �����Ѵ�. FixedUpdate() �޼ҵ� �ϳ��� ������ ������ �����ϱ� ������
 * ������ ��� FieldEnemy���� ���� OnMoveCoroutine() �޼ҵ带 ������ ������ ������ �ξ� ����.
 * 
 * Update : 2022/06/27 MON 18:55
 * File : EnemyMovement.cs �� ����
 * 
 * OnMoveCoroutine() �� �����ϸ� ������ �������� ������ FieldEnemy ��ü���� ����ؼ� �ڷ�ƾ �����̸� ȣ���ϴ¾��� �������� �����̾���.
 * �ڷ�ƾ�� �ƴ� Update()�� FixedUpdate()�� FieldEnemy�� �̵��� ������ ��� �ϳ��� ��ũ��Ʈ���� FieldEnemy ��ü�� �̵��� �����ϴ°ź��� ȿ���� ����, ������ ���� ���� ������
 * FieldEnemy ��ü���� EnemyMovement ������Ʈ�� �Ҵ�޾� ���� �̵��� �����ϴ� �������� �����Ͽ���.
 */