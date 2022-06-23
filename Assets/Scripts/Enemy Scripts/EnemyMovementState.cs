using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementState
{
    public int currentIndex { get; set; }
    public float moveSpeed { get; set; }
    public bool isStop { get; set; } = false;
    public Vector3 moveDirection { get; set; } = Vector3.zero;

    public void Setup(float moveSpeed)
    {
        this.moveSpeed = moveSpeed;
        currentIndex = 0;
        isStop = false;
        moveDirection = Vector3.zero;
    }
}
