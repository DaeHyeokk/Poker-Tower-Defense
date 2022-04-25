using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public abstract class TowerType : MonoBehaviour
{ 
    enum ColorType { RED = 0, GREEN, BLUE }

    [SerializeField]
    private HorizontalLayoutGroup gradeLayout;
    private Image[] gradeImages;

    private Color[] ColorDatas;
    private SpriteRenderer towerRenderer;

    private TowerData towerData;
    public float damage => towerData.weapons[towerGrade].damage;
    public float rate => towerData.weapons[towerGrade].rate;
    public float range => towerData.weapons[towerGrade].range;

    public int towerGrade   { get; private set; }
    private ColorType towerColor;
    // public int killCount { get; private set; }// 미구현 : 유닛당 킬수 카운팅. 게임 종료 시 플레이어 데이터에 누적하는 용도
    public event Action actionOnSkill;

    private void Awake()
    {
        gradeLayout.gameObject.SetActive(false);
        gradeImages = new Image[3];
        gradeImages = gradeLayout.GetComponentsInChildren<Image>(true);

        towerRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();

        ColorDatas = new Color[3];

        ColorDatas[0] = new Color(200, 0, 0);
        ColorDatas[1] = new Color(0, 200, 0);
        ColorDatas[2] = new Color(0, 0, 200);
    }

    // Tower의 능력치, 등급, 색상타입 세팅
    // 처음 생성되는 타워이므로 등급을 0으로 설정
    public void Setup(TowerData _towerData)
    {
        towerData = _towerData;
        towerGrade = 0;
        ColorSetup();
    }

    // Color Type은 Red, Green, Blue 셋 중 하나를 랜덤으로 설정함
    private void ColorSetup()
    {
        ColorType type = (ColorType)UnityEngine.Random.Range((int)ColorType.RED, (int)ColorType.BLUE+1);
        towerColor = type;

        towerRenderer.color = ColorDatas[(int)towerColor];
    }

    // Tower의 등급을 업그레이드 하는 메서드, Tower의 최대 등급은 3이다
    public void GradeUp()
    {
        // Tower가 최대 등급에 도달했을 경우 작업을 수행하지 않는다
        if(towerGrade >= 3)
        {
            return;
        }
        
        towerGrade++;
        UpdateWeaponUI();
    }

    private void UpdateWeaponUI()
    {
        // 타워등급이 1이라면 => GradeUp()이 최초로 실행됐다면
        if (towerGrade == 1)
            gradeLayout.gameObject.SetActive(true);  // layoutGroup UI를 활성화 한다

        // 등급을 나타내는 이미지 한개를 활성화 시킨다
        gradeImages[towerGrade - 1].gameObject.SetActive(true);
    }
    public virtual void OnAttack(Enemy enemy)
    {    
        // 여기서 3은 게임 전역 데이터의 업그레이드 수치를 임의로 준 값
        enemy.OnDamage(towerData.weapons[towerGrade].damage + towerData.weapons[towerGrade].upgradeDIP * 3);
    }

    // OnSkill() 메서드는 Weapon Type마다 다르게 동작해야 하기 때문에
    // 자식클래스에서 OnSkill() 메서드를 직접 구현하도록 강제하기 위해 Abstract Method로 선언
    public abstract void OnSkill();
}
