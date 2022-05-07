using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerColor 
{
    public enum ColorType { Red, Green, Blue }

    private SpriteRenderer _towerRenderer;

    private Color[] _colorDatas;
    private ColorType _colorType;

    public ColorType colorType => _colorType;

    public TowerColor(SpriteRenderer towerRenderer)
    {
        _towerRenderer = towerRenderer;

        _colorDatas = new Color[3];

        _colorDatas[0] = new Color(180, 0, 0);  // Red
        _colorDatas[1] = new Color(0, 180, 0);  // Green
        _colorDatas[2] = new Color(0, 0, 180);  // Blue
    }

    public void ChangeColor()
    {
        // 랜덤으로 ColorType 설정
        ColorType type = (ColorType)Random.Range((int)ColorType.Red, (int)ColorType.Blue + 1);
        _colorType = type;

        _towerRenderer.color = _colorDatas[(int)_colorType];
    }
}


/*
 * File : TowerColor.cs
 * First Update : 2022/04/30 SAT 23:50
 * Tower의 Color 속성을 담당하는 스크립트.
 * 
 * Update : 2022/05/05 THU 23:05
 * 컴포넌트에서 클래스로 변경. (?)
 */