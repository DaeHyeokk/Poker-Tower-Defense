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
    GameDataUIController _gameDataUIController;
    [SerializeField]
    ColorUpgradeUIController _colorUpgradeUIController;

    private int _wave;
    private int _finalWave;
    private int _life;
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

    public int wave
    {
        get => _wave;
        set
        {
            if(value > _finalWave)
            {
                ClearGame();
                return;
            }

            _wave = value;
            _gameDataUIController.SetWaveText(_wave);
        }
    }

    public int life
    {
        get => _life;
        set
        {
            _life = value;

            if (_life < 0) 
                _life = 0;

            _gameDataUIController.SetLiftAmountText(_life);

            if (_life == 0)
                EndGame();
        }
    }

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
        wave = 0;
        life = 100;
        gold = 400;
        mineral = 400;
        changeChance = 4000;
        _isPausing = false;
        _isGameover = false;
        _finalWave = 40;

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

    public void IncreaseWave()
    {
        wave++;
        gold += 200;
    }

    public void UpgradeColor(int index)
    {
        if (_colorUpgradeCosts[index] > mineral)
        {
            UIManager.instance.ShowSystemMessage("미네랄이 부족합니다.");
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

    private void EndGame()
    {
        _isGameover = true;
    }

    private void ClearGame()
    {

    }

    public bool IsFinalWave() { return wave == _finalWave; }


}

/*
 * File : GameManager.cs
 * First Update : 2022/04/22 FRI 02:30
 * PokerTower Defense 게임의 전체적인 정보를 관리할 GameManager 스크립트
 * 진행중인 게임의 라운드와 Gameover 상태를 갖고 있음
 * 
 * Update : 2022/05/10 TUE 06:30
 * 진행중인 게임의 남은 라이프, 골드와 미네랄, 카드 변경권 횟수를 관리하는 로직 추가.
 * 현재 라운드가 마지막 라운드인지 확인하는 로직 추가.
 * 
 * Update : 2022/05/12 THU 23:30
 * Color Upgrade 로직 추가.
 * 
 * Update : 2022/06/12 SUN 20:25
 * Game Pause, Game Speedup 로직 추가.
 */
