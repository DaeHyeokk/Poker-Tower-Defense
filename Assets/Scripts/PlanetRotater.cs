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
 * ���� �� �༺ ��������Ʈ�� ���� ������Ʈ�� ȸ���� ����ϴ� ��ũ��Ʈ.
 */