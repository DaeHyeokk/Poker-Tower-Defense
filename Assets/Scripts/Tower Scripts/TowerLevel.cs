using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerLevel : MonoBehaviour
{
    [SerializeField]
    private HorizontalLayoutGroup _levelLayout;
    private Image[] _levelImages;

    private int _levelCount;

    public int levelCount => _levelCount;

    private void Awake()
    {
        _levelLayout.gameObject.SetActive(false);
        _levelImages = new Image[3];
        _levelImages = _levelLayout.GetComponentsInChildren<Image>(true);
    }

    public void DefaultSetup()
    {
        _levelCount = 0;
    }

    // Tower�� ����� ���׷��̵� �ϴ� �޼���, Tower�� �ִ� ������ 3�̴�
    public void LevelUp()
    {
        // Tower�� �ִ� ��޿� �������� ��� �۾��� �������� �ʴ´�
        if (_levelCount >= 3)
        {
            return;
        }

        _levelCount++;
        UpdateWeaponUI();
    }

    private void UpdateWeaponUI()
    {
        // Ÿ������� 1�̶�� => LevelUp()�� ó�� ����ƴٸ�
        if (_levelCount == 1)
            _levelLayout.gameObject.SetActive(true);  // layoutGroup UI�� Ȱ��ȭ �Ѵ�

        // ����� ��Ÿ���� �̹��� �Ѱ��� Ȱ��ȭ ��Ų��
        _levelImages[_levelCount - 1].gameObject.SetActive(true);
    }
}


/*
 * File : TowerLevel.cs
 * First Update : 2022/04/30 SAT 23:50
 * Tower�� ���� ���� ����� ����ϴ� ��ũ��Ʈ.
 */