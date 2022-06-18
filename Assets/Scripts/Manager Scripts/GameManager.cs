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

    private int _gold;
    private int _mineral;
    private int _changeChance;
    private float _gameSpeed;
    private float _backupGameSpeed;
    private float _maxGameSpeed;
    private bool _isPausing;
    private bool _isGameover;


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

    public int pokerCount => _pokerCount;
    public float maxGameSpeed => _maxGameSpeed;
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

        ScreenSleepSetup();
    }
    private void ScreenSleepSetup()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
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
