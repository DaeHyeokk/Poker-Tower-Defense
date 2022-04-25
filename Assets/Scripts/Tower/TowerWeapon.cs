using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;
public class TowerWeapon : MonoBehaviour
{ 
    enum ColorType
    {
        RED = 0, GREEN, BLUE
    };

    [SerializeField]
    private HorizontalLayoutGroup layout;
    [SerializeField]
    private Image[] images;

    private Color[] ColorDatas;
    private SpriteRenderer towerRenderer;

    private TowerData towerData;
    public float damage => towerData.weapons[towerGrade].damage;
    public float rate => towerData.weapons[towerGrade].rate;
    public float range => towerData.weapons[towerGrade].range;

    public int towerGrade   { get; private set; }
    public int towerColor { get; private set; }
    // public int killCount { get; private set; }// 미구현 : 유닛당 킬수 카운팅. 게임 종료 시 플레이어 데이터에 누적하는 용도
    public event Action actionOnSkill;

    public void GradeUp()
    {
        if (towerGrade < 3)
        {
            towerGrade++;
            UpdateWeaponUI();
        }
        else
            Debug.Log("Tower Grade out of range error!");
    }

    private void Awake()
    {
        layout.gameObject.SetActive(false);
        towerRenderer = gameObject.GetComponent<SpriteRenderer>();

        ColorDatas = new Color[3];

        ColorDatas[0] = new Color(200, 0, 0);
        ColorDatas[1] = new Color(0, 200, 0);
        ColorDatas[2] = new Color(0, 0, 200);
    }

    public void Setup(TowerData _towerData)
    {
        towerData = _towerData;
        towerGrade = 0;
        ColorSetup();
    }

    private void ColorSetup()
    {
        ColorType type = (ColorType)UnityEngine.Random.Range((int)ColorType.RED, (int)ColorType.BLUE+1);
        towerColor = (int)type;

        towerRenderer.color = ColorDatas[towerColor];
    }

    private void UpdateWeaponUI()
    {
        // 타워등급이 1이라면 => GradeUp()이 최초로 실행됐다면
        if (towerGrade == 1)
            layout.gameObject.SetActive(true);  // layoutGroup UI를 활성화 한다

        // 등급을 나타내는 이미지 한개를 활성화 시킨다
        images[towerGrade - 1].gameObject.SetActive(true);
    }
    public virtual void OnAttack(Enemy enemy)
    {    
        // 여기서 3은 게임 전역 데이터의 업그레이드 수치를 임의로 준 값
        enemy.OnDamage(towerData.weapons[towerGrade].damage + towerData.weapons[towerGrade].upgradeDIP * 3);
    }
}
