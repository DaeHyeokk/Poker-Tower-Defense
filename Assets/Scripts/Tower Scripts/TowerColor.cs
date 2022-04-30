using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerColor : MonoBehaviour
{
    public enum ColorType { Red, Green, Blue }

    private SpriteRenderer _towerRenderer;

    private Color[] _colorDatas;
    private ColorType _colorType;

    public ColorType colorType => _colorType;

    private void Awake()
    {
        _colorDatas = new Color[3];

        _colorDatas[0] = new Color(180, 0, 0);  // Red
        _colorDatas[1] = new Color(0, 180, 0);  // Green
        _colorDatas[2] = new Color(0, 0, 180);  // Blue
    }

    public void DefaultSetup()
    {
        // 랜덤으로 ColorType 설정
        ColorType type = (ColorType)Random.Range((int)ColorType.Red, (int)ColorType.Blue + 1);
        _colorType = type;

        // 자식오브젝트인 TowerWeapon에서 SpriteRenderer 컴포넌트를 찾아 값을 변경시킴
        _towerRenderer = GetComponentInChildren<SpriteRenderer>();
        _towerRenderer.color = _colorDatas[(int)_colorType];
    }
}


/*
 * File : TowerColor.cs
 * First Update : 2022/04/30 SAT 23:50
 * Tower의 Color 속성을 담당하는 스크립트.
 */