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
 * 움직일 수 있는 오브젝트에 부착하여 오브젝트의 이동을 수행한다. 
 * 
 * Update : 2022/04/22 FRI 02:20
 * 라운드별 몬스터마다 이동속도를 다르게 설정하기 위해 moveSpeed 프로퍼티 추가
 */