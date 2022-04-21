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
 * Enemy 오브젝트에 부착하여 오브젝트의 이동을 수행한다. 
 * 
 * Update : 2022/04/22 FRI 02:20
 * 라운드별 몬스터마다 이동속도를 다르게 설정하기 위한 setMoveSpeed() 메서드 추가
 */