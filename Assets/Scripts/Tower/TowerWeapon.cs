using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public abstract class TowerWeapon : MonoBehaviour
{
    public enum ColorType { Red, Green, Blue }

    [SerializeField]
    private HorizontalLayoutGroup _gradeLayout;
    private Image[] _gradeImages;
    private Color[] _colorDatas;
    private SpriteRenderer _towerRenderer;
    private TowerData _towerData;
    private int _towerGrade;
    private ColorType _towerColor;

    public float damage => _towerData.weapons[_towerGrade].damage;
    public float rate => _towerData.weapons[_towerGrade].rate;
    public float range => _towerData.weapons[_towerGrade].range;
    public int towerGrade => _towerGrade;
    public ColorType towerColor => _towerColor;
    public abstract String towerName { get; }
    // public int killCount { get; private set; }// �̱��� : ���ִ� ų�� ī����. ���� ���� �� �÷��̾� �����Ϳ� �����ϴ� �뵵

    private void Awake()
    {

        _gradeLayout.gameObject.SetActive(false);
        _gradeImages = new Image[3];
        _gradeImages = _gradeLayout.GetComponentsInChildren<Image>(true);

        _towerRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();

        _colorDatas = new Color[3];

        _colorDatas[0] = new Color(180, 0, 0);
         _colorDatas[1] = new Color(0, 180, 0);
        _colorDatas[2] = new Color(0, 20, 200);
    }

    // Tower�� �ɷ�ġ, ���, ����Ÿ�� ����
    // ó�� �����Ǵ� Ÿ���̹Ƿ� ����� 0���� ����
    public void Setup(TowerData towerData)
    {
        _towerData = towerData;
        _towerGrade = 0;
        ColorSetup();
    }

    // Color Type�� Red, Green, Blue �� �� �ϳ��� �������� ������
    private void ColorSetup()
    {
        ColorType type = (ColorType)UnityEngine.Random.Range((int)ColorType.Red, (int)ColorType.Blue+1);
        _towerColor = type;

        _towerRenderer.color = _colorDatas[(int)towerColor];
    }

    // Tower�� ����� ���׷��̵� �ϴ� �޼���, Tower�� �ִ� ����� 3�̴�
    public void GradeUp()
    {
        // Tower�� �ִ� ��޿� �������� ��� �۾��� �������� �ʴ´�
        if(_towerGrade >= 3)
        {
            return;
        }
        
        _towerGrade++;
        UpdateWeaponUI();
    }

    private void UpdateWeaponUI()
    {
        // Ÿ������� 1�̶�� => GradeUp()�� ���ʷ� ����ƴٸ�
        if (_towerGrade == 1)
            _gradeLayout.gameObject.SetActive(true);  // layoutGroup UI�� Ȱ��ȭ �Ѵ�

        // ����� ��Ÿ���� �̹��� �Ѱ��� Ȱ��ȭ ��Ų��
        _gradeImages[_towerGrade - 1].gameObject.SetActive(true);
    }
    public virtual void OnAttack(Enemy enemy)
    {    
        // ���⼭ 3�� ���� ���� �������� ���׷��̵� ��ġ�� ���Ƿ� �� ��
        enemy.OnDamage(_towerData.weapons[_towerGrade].damage + _towerData.weapons[_towerGrade].upgradeDIP * 3);
    }

    // OnSkill() �޼���� Weapon Type���� �ٸ��� �����ؾ� �ϱ� ������
    // �ڽ�Ŭ�������� OnSkill() �޼��带 ���� �����ϵ��� �����ϱ� ���� Abstract Method�� ����
    public abstract void OnSkill();
}

/*
 * File : TowerWeapon.cs
 * First Update : 2022/04/25 MON 10:52
 * Ÿ���� ������ ����ϴ� ��ũ��Ʈ.
 * Ÿ���� ����, ���ݷ�, ���ݼӵ� �� ���ݿ� ���õ� �����͸� ������ ������,
 * Ÿ�� �������� ���� �ٸ� ��ų�� �����ؾ� �ϱ� ������ �߻� Ŭ������ �����Ͽ���.
 * TowerWeapon Ŭ������ ��ӹ޴� 9���� ����Ŭ������ ������ ����Ŭ�������� OnSkill() �޼��带 �����Ѵ�.
 */