using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Transform _target;
    private Sprite _projectileSprite;
    private Movement2D _movement2D;

    public void Setup(Transform targetTransfrom)
    {
        _target = targetTransfrom;
       // _projectileSprite = projectileSprite;
        _movement2D = GetComponent<Movement2D>();
        _movement2D.setMoveSpeed(1f);
    }

    private void Update()
    {
        if(_target != null)
        {
            Vector3 direction = (_target.position - this.transform.position).normalized;
            _movement2D.MoveTo(direction);
        }
        else
        {
            Debug.Log("�Ѿ� �ı�");
            Destroy(gameObject);
        }
        
    }
}


/*
 * File : Projectile.cs
 * First Update : 2022/04/28 THU 23:55
 * Ÿ���� �Ѿ� �߻縦 Ȯ���ϱ� ���� ������ ������ ��ũ��Ʈ.
 * ���Ŀ� Projectile Ŭ������ �߻�Ŭ������ �����Ͽ� �پ��� Projectile�� ������ ����.
 */