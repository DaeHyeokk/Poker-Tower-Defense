using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    [SerializeField]
    private GameDataUIController _gameDataUIController;
    [SerializeField]
    private ColorUpgradeUIController _colorUpgradeUIController;

    private int _gold;
    private int _mineral;
    private int _changeChance;
    private float _gameSpeed;
    private float _backupGameSpeed;
    private float _maxGameSpeed;
    private bool _isPausing;
    private bool _isGameover;

    private int[] _colorUpgradeIncrementCost;
    private int[] _colorUpgradeCounts;
    private int[] _colorUpgradeCosts;

    public int gold
    {
        get => _gold;
        set
        {
            _gold = value;
            _gameDataUIController.SetGoldAmountText(_gold);
        }
    }

    public int mineral
    {
        get => _mineral;
        set
        {
            _mineral = value;
            _gameDataUIController.SetMineralAmountText(_mineral);
        }
    }

    public int changeChance
    {
        get => _changeChance;
        set
        {
            _changeChance = value;
            _gameDataUIController.SetCardChangeAmountText(_changeChance);
        }
    }

    public float gameSpeed
    {
        get => _gameSpeed;
        set
        {
            _gameSpeed = value;

            if (_gameSpeed > _maxGameSpeed)
                _gameSpeed = 1f;

            Time.timeScale = _gameSpeed;
        }
    }

    public int[] colorUpgradeCounts => _colorUpgradeCounts;
    public int pokerCount => _pokerCount;
    public bool isPausing => _isPausing;
    public bool isGameover => _isGameover;

    private void Awake()
    {
        if (instance != this)
            Destroy(gameObject);

        _maxGameSpeed = 3f;
        gameSpeed = 1f;
        gold = 600;
        mineral = 100;
        changeChance = 4000;
        _isPausing = false;
        _isGameover = false;

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

    public void UpgradeColor(int index)
    {
        if (_colorUpgradeCosts[index] > mineral)
        {
            UIManager.instance.ShowSystemMessage("�̳׶��� �����մϴ�.");
            return;
        }

        mineral -= _colorUpgradeCosts[index];

        SetColorUpgradeCount(index, _colorUpgradeCounts[index]+1);
        SetColorUpgradeCost(index, _colorUpgradeCosts[index]+1);
        
        _colorUpgradeIncrementCost[index]++;
    }

    private void SetColorUpgradeCount(int index, int value)
    {
        _colorUpgradeCounts[index] = value;
        _colorUpgradeUIController.SetColorUpgradeCountText(index, _colorUpgradeCounts[index]);
    }

    private void SetColorUpgradeCost(int index, int value)
    {
        _colorUpgradeCosts[index] = value;
        _colorUpgradeUIController.SetColorUpgradeCostText(index, _colorUpgradeCosts[index]);
    }

    public void SpeedUpGame()
    {
        gameSpeed++;
        Time.timeScale = gameSpeed;
        _gameDataUIController.SetGameSpeedText(_gameSpeed);
    }

    public void PauseGame()
    {
        _isPausing = true;
        _backupGameSpeed = gameSpeed;
        gameSpeed = 0f;
        UIManager.instance.ShowGameMenu();
    }

    public void ResumeGame()
    {
        _isPausing = false;
        gameSpeed = _backupGameSpeed;
        UIManager.instance.HideGameMenu();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("SingleModeScene");
    }

    public void DefeatGame()
    {
        _isGameover = true;
    }

    public void ClearGame()
    {

    }


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
 * 
 * Update : 2022/06/12 SUN 20:25
 * Game Pause, Game Speedup ���� �߰�.
 */
