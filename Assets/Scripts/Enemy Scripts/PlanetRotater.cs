using UnityEngine;

public class PlanetRotater : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _targetSprite;

    private void Update()
    {
        _targetSprite.transform.Rotate(Vector3.forward * Time.deltaTime * 50f);
    }
}


/*
 * File : PlanetRotater.cs
 * First Update : 2022/05/10 TUE 23:21
 * 게임 속 행성 스프라이트를 가진 오브젝트의 회전을 담당하는 스크립트.
 */