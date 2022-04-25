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
    // public int killCount { get; private set; }// �̱��� : ���ִ� ų�� ī����. ���� ���� �� �÷��̾� �����Ϳ� �����ϴ� �뵵
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
        // Ÿ������� 1�̶�� => GradeUp()�� ���ʷ� ����ƴٸ�
        if (towerGrade == 1)
            layout.gameObject.SetActive(true);  // layoutGroup UI�� Ȱ��ȭ �Ѵ�

        // ����� ��Ÿ���� �̹��� �Ѱ��� Ȱ��ȭ ��Ų��
        images[towerGrade - 1].gameObject.SetActive(true);
    }
    public virtual void OnAttack(Enemy enemy)
    {    
        // ���⼭ 3�� ���� ���� �������� ���׷��̵� ��ġ�� ���Ƿ� �� ��
        enemy.OnDamage(towerData.weapons[towerGrade].damage + towerData.weapons[towerGrade].upgradeDIP * 3);
    }
}
