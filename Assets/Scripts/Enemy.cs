using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private int wayPointCount;     // �̵���� ����
    private Transform[] wayPoints;  // �̵���� ��ǥ �迭
    private int currentIndex = 0;   // ���� ��ǥ���� �ε���
    private Movement2D movement2D;  // ������Ʈ �̵� ����

    public void Setup(Transform[] wayPoints)
    {
        movement2D = GetComponent<Movement2D>();

        // Enemy �̵� ��� WayPoints ����
        wayPointCount = wayPoints.Length;
        this.wayPoints = new Transform[wayPointCount];
        this.wayPoints = wayPoints;

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
            // ���� enemy�� ��ġ�� ��ǥ ������ ��ġ ������ �Ÿ��� 0.2���� �����ٸ� ���� ��ǥ ������ Ž���Ѵ�
            // 0.2f�� MoveSpeed�� �����ִ� ������ �̵� �ӵ��� ���� enemy�� ��� �� �����ӿ� 0.2f���� ���� �̵��� ���� �ֱ� ������
            // if ���ǹ��� �ɸ��� �ʰ� ��ǥ ��θ� ���� enemy ������Ʈ�� �߻��� �� �ֱ� �����̴�.
            if (Vector3.Distance(transform.position, wayPoints[currentIndex].position) < 0.2f * movement2D.MoveSpeed)
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
            movement2D.moveTo(direction);
        }
        // ���� ��ġ�� ������ ��ǥ �����̶��
        else
            Destroy(gameObject);
    }
}

/*
 * File : Enemy.cs
 * First Update : 2022/04/20 WED 14:57
 * Enemy ������Ʈ�� ������ �� �̵��� Waypoint ��ǥ�� �����ϰ�,
 * ��ǥ ������ ������� �̵��ϸ� ������ WayPoint�� �������� �� ������Ʈ�� �ı��Ѵ�.
 */
