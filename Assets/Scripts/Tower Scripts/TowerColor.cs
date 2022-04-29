using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerColor : MonoBehaviour
{
    public enum ColorType { Red, Green, Blue }

    [SerializeField]
    private SpriteRenderer _towerRenderer;

    private Color[] _colorDatas;
    private ColorType _colorType;

    public ColorType colorType => _colorType;

    private void Awake()
    {
        _colorDatas = new Color[3];

        _colorDatas[0] = new Color(180, 0, 0);
        _colorDatas[1] = new Color(0, 180, 0);
        _colorDatas[2] = new Color(0, 0, 180);
    }

    private void ColorSetup()
    {
        ColorType type = (ColorType)UnityEngine.Random.Range((int)ColorType.Red, (int)ColorType.Blue + 1);
        _colorType = type;

        _towerRenderer.color = _colorDatas[(int)_colorType];
    }
}
