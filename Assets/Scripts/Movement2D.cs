using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement2D : MonoBehaviour
{
    private Vector3 _moveDirection = Vector3.zero;
    public float moveSpeed { get; set; }
    public Vector3 moveDirection => _moveDirection;

    private void Update()
    {
        transform.position += _moveDirection * moveSpeed * Time.deltaTime;
    }

    public void MoveTo(Vector3 direction)
    {
        _moveDirection = direction;
    }
}

/*
 * File : Movement2D.cs
 * First Update : 2022/04/20 WED 14:57
 * ������ �� �ִ� ������Ʈ�� �����Ͽ� ������Ʈ�� �̵��� �����Ѵ�. 
 * 
 * Update : 2022/04/22 FRI 02:20
 * ���庰 ���͸��� �̵��ӵ��� �ٸ��� �����ϱ� ���� moveSpeed ������Ƽ �߰�
 */