using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Transform[] wayPoints;  // �̵���� ��ǥ �迭

    private int wayPointCount;     // �̵���� ����
    private int currentIndex;   // ���� ��ǥ���� �ε���
    private EnemyMovement movement2D;  // ������Ʈ �̵� ����

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
        // Enemy ���� ��ġ�� ù��° wayPoint ��ǥ�� ����
        transform.position = this.wayPoints[currentIndex].position;
        // �� �̵�/��ǥ ���� ���� �ڷ�ƾ �Լ� ����
        StartCoroutine("OnMove");
    }

    private IEnumerator OnMove()
    {
        NextMoveTo();

        while (true)
        {
            // ���� enemy�� ��ġ�� ��ǥ ������ ��ġ ������ �Ÿ��� 0.1���� �����ٸ� ���� ��ǥ ������ Ž���Ѵ�
            // 0.2f�� MoveSpeed�� �����ִ� ������ �̵� �ӵ��� ���� enemy�� ��� �� �����ӿ� 0.1f���� ���� �̵��� ���� �ֱ� ������
            // if ���ǹ��� �ɸ��� �ʰ� ��ǥ ��θ� ���� enemy ������Ʈ�� �߻��� �� �ֱ� �����̴�.
            if (Vector3.Distance(transform.position, wayPoints[currentIndex].position) < 0.1f * movement2D.MoveSpeed)
                NextMoveTo();

            // 1������ ���
            yield return null;
        }
    }

    private void NextMoveTo()
    {
        // ���� ��ǥ ������ �����ִٸ�
        if (currentIndex < wayPointCount - 1)
        {
            // Enemy�� ��ġ�� ��Ȯ�ϰ� ��ǥ ��ġ�� ����
            transform.position = wayPoints[currentIndex].position;
            currentIndex++;

            Vector3 direction = (wayPoints[currentIndex].position - transform.position).normalized;
            movement2D.MoveTo(direction);
        }
        // ���� ��ġ�� ������ ��ǥ �����̶��
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
 * Enemy ������Ʈ�� ������ �� �̵��� Waypoint ��ǥ�� �����ϰ�,
 * ��ǥ ������ ������� �̵��ϸ� ������ WayPoint�� �������� �� ������Ʈ�� �ı��Ѵ�.
 * 
 * Update : 2022/04/21 THU 13:20
 * ������ƮǮ�� ����� �����Ͽ��� ������ ���� ���忡�� ����� ������Ʈ�� ����ؼ� �����ϰ� ��
 * ���� Setup() �޼��忡�� ������ WayPoints �迭�� �Ҵ�� ���� �ִ��� Ȯ���ϴ� ������ �߰��Ͽ� ���ʿ��� ������ ���� �ʵ��� �����Ͽ���,
 * WayPoints �迭�� ù��° �ε������� �ٽ� �̵��ϱ� ���� ������ ����ߴ� currentIndex ���� 0���� �ٲٴ� ������ �߰���
 */
