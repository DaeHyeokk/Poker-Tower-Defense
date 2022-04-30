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

    // Tower의 등급을 업그레이드 하는 메서드, Tower의 최대 레벨은 3이다
    public void LevelUp()
    {
        // Tower가 최대 등급에 도달했을 경우 작업을 수행하지 않는다
        if (_levelCount >= 3)
        {
            return;
        }

        _levelCount++;
        UpdateWeaponUI();
    }

    private void UpdateWeaponUI()
    {
        // 타워등급이 1이라면 => LevelUp()이 처음 실행됐다면
        if (_levelCount == 1)
            _levelLayout.gameObject.SetActive(true);  // layoutGroup UI를 활성화 한다

        // 등급을 나타내는 이미지 한개를 활성화 시킨다
        _levelImages[_levelCount - 1].gameObject.SetActive(true);
    }
}


/*
 * File : TowerLevel.cs
 * First Update : 2022/04/30 SAT 23:50
 * Tower의 레벨 관련 기능을 담당하는 스크립트.
 */