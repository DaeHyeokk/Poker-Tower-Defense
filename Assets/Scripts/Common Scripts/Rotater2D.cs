using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotater2D : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _targetSprite;

    public void NaturalRotate()
    {
        _targetSprite.transform.Rotate(Vector3.forward * Time.deltaTime * 50f);
    }

    public void LookAtTarget(Transform target)
    {
        float dx;
        float dy;
        float degree;

        // 원점으로부터의 거리와 수평축으로부터의 각도를 이용해 위치를 구하는 극좌표계 이용
        // 각도 = arctan(y/x)
        // x, y 변위값 구하기
        dx = target.transform.position.x - this.transform.position.x;
        dy = target.transform.position.y - this.transform.position.y;

        // x, y 변위값을 바탕으로 각도 구하기
        // 각도가 radian 단위이기 때문에 Mathf.Rad2Deg를 곱해 도 단위를 구함
        degree = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg;

        _targetSprite.transform.rotation = Quaternion.Euler(0f, 0f, degree);
    }

    public void NaturalLookAtTarget(Transform target)
    {
        float dx;
        float dy;
        float degree;

        // 원점으로부터의 거리와 수평축으로부터의 각도를 이용해 위치를 구하는 극좌표계 이용
        // 각도 = arctan(y/x)
        // x, y 변위값 구하기
        dx = target.transform.position.x - this.transform.position.x;
        dy = target.transform.position.y - this.transform.position.y;

        // x, y 변위값을 바탕으로 각도 구하기
        // 각도가 radian 단위이기 때문에 Mathf.Rad2Deg를 곱해 도 단위를 구함
        degree = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg;

        _targetSprite.transform.rotation = Quaternion.Slerp(_targetSprite.transform.rotation, Quaternion.Euler(0f, 0f, degree), 0.1f);
    }
}


/*
 * File : Rotater2D.cs
 * First Update : 2022/06/16 THU 23:20
 * 2D 오브젝트의 회전을 담당하는 스크립트. 
 * 회전하는 오브젝트에 부착하여 사용한다.
 */