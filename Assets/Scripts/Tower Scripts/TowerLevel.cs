using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerLevel
{
    private HorizontalLayoutGroup _levelLayout;
    private Image[] _levelImages;

    private int _level;

    public int level => _level;

    public TowerLevel(HorizontalLayoutGroup levelLayout)
    {
        _levelLayout = levelLayout;
        _levelLayout.gameObject.SetActive(false);

        _levelImages = new Image[3];
        _levelImages = _levelLayout.GetComponentsInChildren<Image>(true);

        _level = 0;
    }

    public void Reset()
    {
        // Ȱ��ȭ ��Ų level image�� ��� ��Ȱ��ȭ ��Ű�� _levelCount ���� 0���� �ٲ۴�.
        while(_level > 0)
        {
            _levelImages[_level - 1].gameObject.SetActive(false);
            _level--;
        } 
    }

    // Tower�� ����� ���׷��̵� �ϴ� �޼���, Tower�� �ִ� ������ 3�̴�
    public void LevelUp()
    {
        // Tower�� �ִ� ��޿� �������� ��� �۾��� �������� �ʴ´�
        if (_level >= 3)
        {
            return;
        }

        _level++;
        UpdateWeaponUI();
    }

    private void UpdateWeaponUI()
    {
        // Ÿ������� 1�̶�� => LevelUp()�� ó�� ����ƴٸ�
        if (_level == 1)
            _levelLayout.gameObject.SetActive(true);  // layoutGroup UI�� Ȱ��ȭ �Ѵ�

        // ����� ��Ÿ���� �̹��� �Ѱ��� Ȱ��ȭ ��Ų��
        _levelImages[_level - 1].gameObject.SetActive(true);
    }
}


/*
 * File : TowerLevel.cs
 * First Update : 2022/04/30 SAT 23:50
 * Tower�� ���� ���� ����� ����ϴ� ��ũ��Ʈ.
 * 
 * Update : 2022/05/05 THU 23:15
 * ������Ʈ���� Ŭ������ ����. (?)
 */