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
            Debug.Log("총알 파괴");
            Destroy(gameObject);
        }
        
    }
}


/*
 * File : Projectile.cs
 * First Update : 2022/04/28 THU 23:55
 * 타워의 총알 발사를 확인하기 위해 간단히 구현한 스크립트.
 * 추후에 Projectile 클래스를 추상클래스로 선언하여 다양한 Projectile을 구현할 예정.
 */