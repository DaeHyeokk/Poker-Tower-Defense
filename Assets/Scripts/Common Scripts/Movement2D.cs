using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement2D : MonoBehaviour
{
    [SerializeField]
    private Vector3 _moveDirection;
    [SerializeField]
    private float _moveSpeed;

    private bool _isStop;

    public float moveSpeed
    {
        get => _moveSpeed;
        set => _moveSpeed = value;
    }

    private void Update()
    {
        if (_isStop) return;
        transform.position += _moveDirection * moveSpeed * Time.deltaTime;
    }

    public void MoveTo(Vector3 direction)
    {
        _moveDirection = direction;
    }

    public void Stop()
    {
        _isStop = true;
    }

    public void Move()
    {
        _isStop = false;
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