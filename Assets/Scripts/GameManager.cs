using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
                return _instance;
            }

            return _instance;
        }
    }

    [SerializeField]
    private int _pokerCount;

    private int _round;
    private int _finalRound;
    private int _life;
    private int _gold;
    private int _mineral;
    private int _changeChance;
    private bool _isGameover;

    private int[] _colorUpgradeIncrementCost;
    private int[] _colorUpgradeCounts;
    private int[] _colorUpgradeCosts;

    public int round => _round;
    public int life => _life;
    public int gold => _gold;
    public int mineral => _mineral;
    public int changeChance => _changeChance;
    public int[] colorUpgradeCount => _colorUpgradeCounts;
    public int pokerCount => _pokerCount;
    public bool isGameover => _isGameover;
    private void Awake()
    {
        if (instance != this)
            Destroy(gameObject);

        _round = 0;
        _finalRound = 40;
        _life = 100;
        _gold = 400;
        _mineral = 400;
        _changeChance = 3;
        _isGameover = false;

        _colorUpgradeIncrementCost = new int[3];
        _colorUpgradeCounts = new int[3];
        _colorUpgradeCosts = new int[3];
        for(int i=0; i<3; i++)
        {
            _colorUpgradeIncrementCost[i] = 1;
            _colorUpgradeCounts[i] = 0;
            _colorUpgradeCosts[i] = 5;
        }

        GameInfoUISetup();
    }

    private void GameInfoUISetup()
    {
        UIManager.instance.SetLiftAmountText(_life);
        UIManager.instance.SetGoldAmountText(_gold);
        UIManager.instance.SetMineralAmountText(_mineral);
        UIManager.instance.SetCardChangeAmountText(_changeChance);
        
        for(int i=0; i<3; i++)
        {
            UIManager.instance.SetColorUpgradeCountText(i, _colorUpgradeCounts[i]);
            UIManager.instance.SetColorUpgradeCostText(i, _colorUpgradeCosts[i]);
        }
    }

    public void IncreaseRound()
    {
        _round++;
        IncreaseGold(200);
    }

    public void DecreaseLife(int decreaseCount)
    {
        _life -= decreaseCount;
        UIManager.instance.SetLiftAmountText(_life);

        if (_life <= 0)
            EndGame();
    }
    private void EndGame()
    {
        _isGameover = true;
    }

    public void IncreaseGold(int increaseCount)
    {
        _gold += increaseCount;
        UIManager.instance.SetGoldAmountText(_gold);
    }
    public void DecreaseGold(int decreaseCount)
    {
        _gold -= decreaseCount;
        UIManager.instance.SetGoldAmountText(_gold);
    }

    public void IncreaseMineral(int increaseCount)
    {
        _mineral += increaseCount;
        UIManager.instance.SetMineralAmountText(_mineral);
    }

    public void DecreaseMineral(int decreaseCount)
    {
        _mineral -= decreaseCount;
        UIManager.instance.SetMineralAmountText(_mineral);
    }

    public void IncreaseChangeChance(int increaseCount)
    {
        _changeChance += increaseCount;
        UIManager.instance.SetCardChangeAmountText(_changeChance);
    }
    public void DecreaseChangeChance()
    {
        _changeChance--;
        UIManager.instance.SetCardChangeAmountText(_changeChance);
    }

    public void UpgradeColor(int index)
    {
        if (_colorUpgradeCosts[index] > _mineral)
            return;

        DecreaseMineral(_colorUpgradeCosts[index]);

        _colorUpgradeCounts[index]++;
        UIManager.instance.SetColorUpgradeCountText(index, _colorUpgradeCounts[index]);

        _colorUpgradeCosts[index] += _colorUpgradeIncrementCost[index];
        UIManager.instance.SetColorUpgradeCostText(index, _colorUpgradeCosts[index]);

        _colorUpgradeIncrementCost[index]++;
    }

    public bool IsFinalRound() { return _round == _finalRound; }
}

/*
 * File : GameManager.cs
 * First Update : 2022/04/22 FRI 02:30
 * PokerTower Defense 게임의 전체적인 정보를 관리할 GameManager 스크립트
 * 진행중인 게임의 라운드와 Gameover 상태를 갖고 있음
 * 
 * Update : 2022/05/10 THU 06:30
 * 진행중인 게임의 남은 라이프, 골드와 미네랄, 카드 변경권 횟수를 관리하는 로직 추가.
 * 현재 라운드가 마지막 라운드인지 확인하는 로직 추가.
 */
