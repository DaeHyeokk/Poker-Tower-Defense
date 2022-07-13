using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerColor 
{
    public enum ColorType { Red, Green, Blue }

    private SpriteRenderer _towerRenderer;
    private ColorType _colorType;

    public ColorType colorType => _colorType;
    public Color color
    {
        get
        {
            switch(_colorType)
            {
                case ColorType.Red:
                    return Color.red;

                case ColorType.Green:
                    return Color.green;

                case ColorType.Blue:
                    return Color.blue;

                default:
                    return Color.white;
            }
        }
    }

    public TowerColor(SpriteRenderer towerRenderer)
    {
        _towerRenderer = towerRenderer;
    }

    public void ChangeRandomColor()
    {
        // 랜덤으로 ColorType 설정
        ColorType colorType = (ColorType)Random.Range((int)ColorType.Red, (int)ColorType.Blue + 1);
        _colorType = colorType;

        _towerRenderer.color = color;
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