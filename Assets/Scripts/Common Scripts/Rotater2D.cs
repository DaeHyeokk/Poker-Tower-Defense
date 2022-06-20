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

        // �������κ����� �Ÿ��� ���������κ����� ������ �̿��� ��ġ�� ���ϴ� ����ǥ�� �̿�
        // ���� = arctan(y/x)
        // x, y ������ ���ϱ�
        dx = target.transform.position.x - this.transform.position.x;
        dy = target.transform.position.y - this.transform.position.y;

        // x, y �������� �������� ���� ���ϱ�
        // ������ radian �����̱� ������ Mathf.Rad2Deg�� ���� �� ������ ����
        degree = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg;

        _targetSprite.transform.rotation = Quaternion.Euler(0f, 0f, degree);
    }

    public void NaturalLookAtTarget(Transform target)
    {
        float dx;
        float dy;
        float degree;

        // �������κ����� �Ÿ��� ���������κ����� ������ �̿��� ��ġ�� ���ϴ� ����ǥ�� �̿�
        // ���� = arctan(y/x)
        // x, y ������ ���ϱ�
        dx = target.transform.position.x - this.transform.position.x;
        dy = target.transform.position.y - this.transform.position.y;

        // x, y �������� �������� ���� ���ϱ�
        // ������ radian �����̱� ������ Mathf.Rad2Deg�� ���� �� ������ ����
        degree = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg;

        _targetSprite.transform.rotation = Quaternion.Slerp(_targetSprite.transform.rotation, Quaternion.Euler(0f, 0f, degree), 0.1f);
    }
}


/*
 * File : Rotater2D.cs
 * First Update : 2022/06/16 THU 23:20
 * 2D ������Ʈ�� ȸ���� ����ϴ� ��ũ��Ʈ. 
 * ȸ���ϴ� ������Ʈ�� �����Ͽ� ����Ѵ�.
 */