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

    public int round { get; set; }

    public int life
    {
        get => _life;
        set
        {
            _life = value;

            if (_life < 0) 
                _life = 0;

            UIManager.instance.SetLiftAmountText(_life);

            if (_life == 0) { }
                EndGame();
        }
    }

    public int gold
    {
        get => _gold;
        set
        {
            _gold = value;
            UIManager.instance.SetGoldAmountText(_gold);
        }
    }

    public int mineral
    {
        get => _mineral;
        set
        {
            _mineral = value;
            UIManager.instance.SetMineralAmountText(_mineral);
        }
    }

    public int changeChance
    {
        get => _changeChance;
        set
        {
            _changeChance = value;
            UIManager.instance.SetCardChangeAmountText(_changeChance);
        }
    }

    public int[] colorUpgradeCounts => _colorUpgradeCounts;
    public int pokerCount => _pokerCount;
    public bool isGameover => _isGameover;
    private void Awake()
    {
        if (instance != this)
            Destroy(gameObject);

        round = 0;
        life = 100;
        gold = 3800;
        mineral = 400;
        changeChance = 3;
        _isGameover = false;
        _finalRound = 40;

        _colorUpgradeIncrementCost = new int[3];
        _colorUpgradeCounts = new int[3];
        _colorUpgradeCosts = new int[3];
        for(int i=0; i<3; i++)
        {
            _colorUpgradeIncrementCost[i] = 1;
            SetColorUpgradeCount(i, 0);
            SetColorUpgradeCost(i, 5);
        }

        ScreenSleepSetup();
    }
    private void ScreenSleepSetup()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    public void IncreaseRound()
    {
        round++;
        gold += 200;
    }

    public void UpgradeColor(int index)
    {
        if (_colorUpgradeCosts[index] > mineral)
            return;

        mineral -= _colorUpgradeCosts[index];

        SetColorUpgradeCount(index, _colorUpgradeCounts[index]+1);
        SetColorUpgradeCost(index, _colorUpgradeCosts[index]+1);
        
        _colorUpgradeIncrementCost[index]++;
    }

    private void SetColorUpgradeCount(int index, int value)
    {
        _colorUpgradeCounts[index] = value;
        UIManager.instance.SetColorUpgradeCountText(index, _colorUpgradeCounts[index]);
    }

    private void SetColorUpgradeCost(int index, int value)
    {
        _colorUpgradeCosts[index] = value;
        UIManager.instance.SetColorUpgradeCostText(index, _colorUpgradeCosts[index]);
    }

    private void EndGame()
    {
        _isGameover = true;
    }

    public bool IsFinalRound() { return _round == _finalRound; }
}

/*
 * File : GameManager.cs
 * First Update : 2022/04/22 FRI 02:30
 * PokerTower Defense ������ ��ü���� ������ ������ GameManager ��ũ��Ʈ
 * �������� ������ ����� Gameover ���¸� ���� ����
 * 
 * Update : 2022/05/10 TUE 06:30
 * �������� ������ ���� ������, ���� �̳׶�, ī�� ����� Ƚ���� �����ϴ� ���� �߰�.
 * ���� ���尡 ������ �������� Ȯ���ϴ� ���� �߰�.
 * 
 * Update : 2022/05/12 THU 23:30
 * Color Upgrade ���� �߰�.
 */
