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
        // Enemy�� �����ִ� ���¶��(����) �۾��� �������� �ʴ´�.
        if (isStop) return;
        //moveDirection�� Vector3.zero ��� Enemy�� ó�� ������ ���̹Ƿ� ù��° ��������Ʈ�� ���� ������ �����Ѵ�.
        if (moveDirection == Vector3.zero)
            NextMoveTo();

        fieldEnemy.transform.position += moveDirection * moveSpeed * Time.fixedDeltaTime;

        float nowDistance = Vector3.Distance(fieldEnemy.transform.position, _wayPoints[currentIndex].position);

        // ���� enemy�� ��ġ�� ��ǥ ������ ��ġ ������ �Ÿ��� Enemy�� �ѹ��� �̵��ϴ� �Ÿ����� �����ٸ� ���� ��ǥ ������ Ž���Ѵ�
        // Why? ��ǥ ���������� �Ÿ��� Enemy�� �ѹ���(1 FixedUpdateTime) �̵��ϴ� �Ÿ����� �����ٸ� Enemy�� ��θ� ��Ż�ϰ� �Ǳ� ����.
        if (nowDistance < moveSpeed * Time.fixedDeltaTime)
            NextMoveTo();
    }

    private void NextMoveTo()
    {
        // Enemy�� ��ġ�� ��Ȯ�ϰ� ��ǥ ��ġ�� ����
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
 * Enemy ������Ʈ���� ���� Movement2D ������Ʈ�� �����ϰ� ������ �ڷ�ƾ �޼ҵ�� Enemy�� �̵�, ��� ���� ������ ������ ��� Enemy�� ��ü ���� �ſ� �������� ���� ��
 * ������尡 �ſ� Ŀ���� �̽��� �ذ��ϱ� ���� ������ ��ũ��Ʈ.
 * 
 * �ʵ忡 ��ȯ�� ��� FieldEnemy�� ����Ʈ�� EnemySpawner���� �޾ƿͼ� FieldEnemy�� �̵� ������ �����Ѵ�. FixedUpdate() �޼ҵ� �ϳ��� ������ ������ �����ϱ� ������
 * ������ ��� FieldEnemy���� ���� OnMoveCoroutine() �޼ҵ带 ������ ������ ������ �ξ� ����.
 */