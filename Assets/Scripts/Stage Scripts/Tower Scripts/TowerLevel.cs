using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerLevel
{
    private HorizontalLayoutGroup _levelLayout;
    private Image[] _levelImages;

    private int _level;
    private int _maxLevel = 3;
    public int level => _level;
    public int maxLevel => _maxLevel;

    public TowerLevel(HorizontalLayoutGroup levelLayout)
    {
        _levelLayout = levelLayout;

        _levelImages = new Image[_maxLevel];
        _levelImages = _levelLayout.GetComponentsInChildren<Image>(true);

        _level = 0;
    }

    public void Reset()
    {
        // 활성화 시킨 level image를 모두 비활성화 시키고 _level 값을 0으로 바꾼다.
        while(_level > 0)
        {
            _levelImages[_level - 1].gameObject.SetActive(false);
            _level--;
        } 
    }

    // Tower의 등급을 업그레이드 하는 메서드, Tower의 최대 레벨은 3이다
    public bool LevelUp()
    {
        // Tower가 최대 등급에 도달했을 경우 작업을 수행하지 않는다
        if (_level >= _maxLevel)
            return false;

        _level++;
        UpdateLevelImage();
        return true;
    }

    public void HideLevelImage()
    {
        /*
        int levelImageIndex = 0;

        while (levelImageIndex < _level)
        {
            _levelImages[levelImageIndex].gameObject.SetActive(false);
            levelImageIndex++;
        }
        */
        _levelLayout.gameObject.SetActive(false);
    }

    public void ShowLevelImage()
    {
        /*
        int levelImageIndex = 0;

        while (levelImageIndex < _level)
        {
            _levelImages[levelImageIndex].gameObject.SetActive(true);
            levelImageIndex++;
        }
        */
        _levelLayout.gameObject.SetActive(true);
    }

    private void UpdateLevelImage()
    {
        int levelImageIndex = 0;

        while (levelImageIndex < _level)
        {
            _levelImages[levelImageIndex].gameObject.SetActive(true);
            levelImageIndex++;
        }
    }
}


/*
 * File : TowerLevel.cs
 * First Update : 2022/04/30 SAT 23:50
 * Tower의 레벨 관련 기능을 담당하는 스크립트.
 * 
 * Update : 2022/05/05 THU 23:15
 * Monovihaviour 상속 제거.
 */