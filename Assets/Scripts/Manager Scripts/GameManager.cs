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
    private int _jokerCard;
    private float _gameSpeed;
    private float _backupGameSpeed;
    private float _maxGameSpeed;
    private bool _isPaused;
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

    public int jokerCard
    {
        get => _jokerCard;
        set
        {
            _jokerCard = value;
            _gameDataUIController.SetJokerCardAmountText(_jokerCard);
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
            Time.fixedDeltaTime = 0.02f * _gameSpeed;
        }
    }

    public int pokerCount => _pokerCount;
    public bool isPaused => _isPaused;
    public bool isGameover => _isGameover;

    private void Awake()
    {
        if (instance != this)
            Destroy(gameObject);
        
        _maxGameSpeed = 3f;
        gameSpeed = 1f;
        gold = 600;
        mineral = 100;
        changeChance = 40;
        jokerCard = 5;
        _isPaused = false;
        _isGameover = false;

        UIManager.instance.GameStartScreenCoverFadeOut();
        ScreenSleepSetup();
        SetScreenResolution();
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
            if (!_isPaused)
                PauseGame();
    }

    // �ػ� �����ϴ� �Լ� //
    private void SetScreenResolution()
    {
        int setWidth = 1440; // ����� ���� �ʺ�
        int setHeight = 3200; // ����� ���� ����

        int deviceWidth = Screen.width; // ��� �ʺ� ����
        int deviceHeight = Screen.height; // ��� ���� ����

        Screen.SetResolution(setWidth, (int)(((float)deviceHeight / deviceWidth) * setWidth), true);

        //ȭ���� ���߾����� ����.
        if ((float)setWidth / setHeight < (float)deviceWidth / deviceHeight) // ����� �ػ� �� �� ū ���
        {
            float newWidth = ((float)setWidth / setHeight) / ((float)deviceWidth / deviceHeight); // ���ο� �ʺ�
            Camera.main.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f); // ���ο� Rect ����
        }
        else // ������ �ػ� �� �� ū ���
        {
            float newHeight = ((float)deviceWidth / deviceHeight) / ((float)setWidth / setHeight); // ���ο� ����
            Camera.main.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight); // ���ο� Rect ����
        }
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
        _isPaused = true;
        _backupGameSpeed = gameSpeed;
        gameSpeed = 0f;
        UIManager.instance.ShowGameMenu();
    }

    public void ResumeGame()
    {
        _isPaused = false;
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
        gameSpeed = 0f;
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
