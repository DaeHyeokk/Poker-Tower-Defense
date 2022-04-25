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
    // public int killCount { get; private set; }// �̱��� : ���ִ� ų�� ī����. ���� ���� �� �÷��̾� �����Ϳ� �����ϴ� �뵵
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

    // Tower�� �ɷ�ġ, ���, ����Ÿ�� ����
    // ó�� �����Ǵ� Ÿ���̹Ƿ� ����� 0���� ����
    public void Setup(TowerData _towerData)
    {
        towerData = _towerData;
        towerGrade = 0;
        ColorSetup();
    }

    // Color Type�� Red, Green, Blue �� �� �ϳ��� �������� ������
    private void ColorSetup()
    {
        ColorType type = (ColorType)UnityEngine.Random.Range((int)ColorType.RED, (int)ColorType.BLUE+1);
        towerColor = type;

        towerRenderer.color = ColorDatas[(int)towerColor];
    }

    // Tower�� ����� ���׷��̵� �ϴ� �޼���, Tower�� �ִ� ����� 3�̴�
    public void GradeUp()
    {
        // Tower�� �ִ� ��޿� �������� ��� �۾��� �������� �ʴ´�
        if(towerGrade >= 3)
        {
            return;
        }
        
        towerGrade++;
        UpdateWeaponUI();
    }

    private void UpdateWeaponUI()
    {
        // Ÿ������� 1�̶�� => GradeUp()�� ���ʷ� ����ƴٸ�
        if (towerGrade == 1)
            gradeLayout.gameObject.SetActive(true);  // layoutGroup UI�� Ȱ��ȭ �Ѵ�

        // ����� ��Ÿ���� �̹��� �Ѱ��� Ȱ��ȭ ��Ų��
        gradeImages[towerGrade - 1].gameObject.SetActive(true);
    }
    public virtual void OnAttack(Enemy enemy)
    {    
        // ���⼭ 3�� ���� ���� �������� ���׷��̵� ��ġ�� ���Ƿ� �� ��
        enemy.OnDamage(towerData.weapons[towerGrade].damage + towerData.weapons[towerGrade].upgradeDIP * 3);
    }

    // OnSkill() �޼���� Weapon Type���� �ٸ��� �����ؾ� �ϱ� ������
    // �ڽ�Ŭ�������� OnSkill() �޼��带 ���� �����ϵ��� �����ϱ� ���� Abstract Method�� ����
    public abstract void OnSkill();
}
