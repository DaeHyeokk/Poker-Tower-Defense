using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{ 
    [SerializeField]
    private Vector3 moveDirection = Vector3.zero;
    private float moveSpeed = 0.0f;
    public float MoveSpeed => moveSpeed;

    private void Update()
    {
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }

    public void setMoveSpeed(float _moveSpeed)
    {
        moveSpeed = _moveSpeed;
    }
    public void MoveTo(Vector3 direction)
    {
        moveDirection = direction;
    }
}

/*
 * File : EnemyMovement.cs
 * First Update : 2022/04/20 WED 14:57
 * Enemy ������Ʈ�� �����Ͽ� ������Ʈ�� �̵��� �����Ѵ�. 
 * 
 * Update : 2022/04/22 FRI 02:20
 * ���庰 ���͸��� �̵��ӵ��� �ٸ��� �����ϱ� ���� setMoveSpeed() �޼��� �߰�
 */