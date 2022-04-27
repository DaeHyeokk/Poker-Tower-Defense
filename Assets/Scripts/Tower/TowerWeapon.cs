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
    // public int killCount { get; private set; }// 미구현 : 유닛당 킬수 카운팅. 게임 종료 시 플레이어 데이터에 누적하는 용도

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

    // Tower의 능력치, 등급, 색상타입 세팅
    // 처음 생성되는 타워이므로 등급을 0으로 설정
    public void Setup(TowerData towerData)
    {
        _towerData = towerData;
        _towerGrade = 0;
        ColorSetup();
    }

    // Color Type은 Red, Green, Blue 셋 중 하나를 랜덤으로 설정함
    private void ColorSetup()
    {
        ColorType type = (ColorType)UnityEngine.Random.Range((int)ColorType.Red, (int)ColorType.Blue+1);
        _towerColor = type;

        _towerRenderer.color = _colorDatas[(int)towerColor];
    }

    // Tower의 등급을 업그레이드 하는 메서드, Tower의 최대 등급은 3이다
    public void GradeUp()
    {
        // Tower가 최대 등급에 도달했을 경우 작업을 수행하지 않는다
        if(_towerGrade >= 3)
        {
            return;
        }
        
        _towerGrade++;
        UpdateWeaponUI();
    }

    private void UpdateWeaponUI()
    {
        // 타워등급이 1이라면 => GradeUp()이 최초로 실행됐다면
        if (_towerGrade == 1)
            _gradeLayout.gameObject.SetActive(true);  // layoutGroup UI를 활성화 한다

        // 등급을 나타내는 이미지 한개를 활성화 시킨다
        _gradeImages[_towerGrade - 1].gameObject.SetActive(true);
    }
    public virtual void OnAttack(Enemy enemy)
    {    
        // 여기서 3은 게임 전역 데이터의 업그레이드 수치를 임의로 준 값
        enemy.OnDamage(_towerData.weapons[_towerGrade].damage + _towerData.weapons[_towerGrade].upgradeDIP * 3);
    }

    // OnSkill() 메서드는 Weapon Type마다 다르게 동작해야 하기 때문에
    // 자식클래스에서 OnSkill() 메서드를 직접 구현하도록 강제하기 위해 Abstract Method로 선언
    public abstract void OnSkill();
}

/*
 * File : TowerWeapon.cs
 * First Update : 2022/04/25 MON 10:52
 * 타워의 공격을 담당하는 스크립트.
 * 타워의 종류, 공격력, 공격속도 등 공격에 관련된 데이터를 가지고 있으며,
 * 타워 종류마다 각각 다른 스킬을 구현해야 하기 때문에 추상 클래스로 선언하였다.
 * TowerWeapon 클래스를 상속받는 9개의 서브클래스가 있으며 서브클래스에서 OnSkill() 메서드를 구현한다.
 */