using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    private static StageManager _instance;
    public static StageManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<StageManager>();
                return _instance;
            }

            return _instance;
        }
    }

    [SerializeField]
    private int _pokerCount;
    [SerializeField]
    private StageDataUIController _stageDataUIController;
    [SerializeField]
    private GameObject _stageMenuPanelObject;
    [SerializeField]
    private GameObject _stageClearPanelObject;
    [SerializeField]
    private GameObject _stageDefeatPanelObject;

    private readonly float[] _enemyHpPercentages = new float[] { 0.5f, 1f, 1.5f, 2f };
    private readonly float[] _enemySpeedPercentages = new float[] { 0.8f, 1f, 1.2f, 1.5f };

    private int _gold;
    private int _mineral;
    private int _changeChance;
    private int _jokerCard;
    private float _gameSpeed;
    private float _backupGameSpeed;
    private float _maxGameSpeed;
    private bool _isPaused;
    private bool _isEnd;

    public event Action onStagePaused;
    public event Action onStageResumed;
    public event Action onStageEnd;

    public int gold
    {
        get => _gold;
        set
        {
            if (_gold < value)
                SoundManager.instance.PlaySFX("Gold Increase Sound");

            _gold = value;
            _stageDataUIController.SetGoldAmountText(_gold);
        }
    }

    public int mineral
    {
        get => _mineral;
        set
        {
            if (_mineral < value)
                SoundManager.instance.PlaySFX("Mineral Increase Sound");

            _mineral = value;
            _stageDataUIController.SetMineralAmountText(_mineral);
        }
    }

    public int changeChance
    {
        get => _changeChance;
        set
        {
            _changeChance = value;
            _stageDataUIController.SetCardChangeAmountText(_changeChance);
        }
    }

    public int jokerCard
    {
        get => _jokerCard;
        set
        {
            _jokerCard = value;
            _stageDataUIController.SetJokerCardAmountText(_jokerCard);
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

    public bool isEnd => _isEnd;
    public int pokerCount => _pokerCount;
    public float enemyHpPercentage => _enemyHpPercentages[(int)GameManager.instance.stageDifficulty];
    public float enemySpeedPercentage => _enemySpeedPercentages[(int)GameManager.instance.stageDifficulty];
    
    private void Awake()
    {
        if (instance != this)
            Destroy(gameObject);
        
        _maxGameSpeed = 3f;
        gameSpeed = 1f;
        _gold = 400;
        _mineral = 100;
        _changeChance = 5;
        _jokerCard = 5;

        _stageDataUIController.SetGoldAmountText(_gold);
        _stageDataUIController.SetMineralAmountText(_mineral);

        ScreenSleepSetup();
    }

    private void OnApplicationPause(bool pause)
    {
        // ������ �̹� ���� ���°ų� ���� ���°� �ƴ϶�� ������ �����.
        if (pause)
            if (!_isPaused && !_isEnd)
                PauseGame();
    }

    private void ScreenSleepSetup()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    public void SpeedUpGame()
    {
        gameSpeed++;
        Time.timeScale = gameSpeed;
        _stageDataUIController.SetGameSpeedText(_gameSpeed);
    }

    public void PauseGame()
    {
        onStagePaused();
        _isPaused = true;
        _backupGameSpeed = gameSpeed;
        gameSpeed = 0f;
        _stageMenuPanelObject.SetActive(true);

        SoundManager.instance.PauseBGM();
        SoundManager.instance.PlaySFX("Stage Pause Sound");
    }

    public void ResumeGame()
    {
        onStageResumed();
        _isPaused = false;
        gameSpeed = _backupGameSpeed;
        _stageMenuPanelObject.SetActive(false);

        SoundManager.instance.ResumeBGM();
    }

    public void ReturnLobby()
    {
        SceneManager.LoadScene("LobbyScene");
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("SingleStageScene");
    }

    public void ClearGame()
    {
        onStageEnd();
        _isEnd = true;
        gameSpeed = 0f;
        _stageClearPanelObject.SetActive(true);

        SaveClearStageData();
        SaveTowerKillCountData();
        PlayerPrefs.Save();

        SoundManager.instance.PlayBGM("Stage Clear Sound");
    }

    public void DefeatGame()
    {
        onStageEnd();
        _isEnd = true;
        gameSpeed = 0f;
        _stageDefeatPanelObject.SetActive(true);

        SaveTowerKillCountData();
        PlayerPrefs.Save();

        SoundManager.instance.PauseBGM();
        SoundManager.instance.PlaySFX("Stage Defeat Sound");
    }

    private void SaveClearStageData()
    {
        int lastClearStage = PlayerPrefs.HasKey("Clear Stage") ? PlayerPrefs.GetInt("Clear Stage") : -1;
        int nowClearStage = (int)GameManager.instance.stageDifficulty;

        // �÷��̾ Ŭ������ �ִ� ���̵����� ���� ���̵��� Ŭ�����ߴٸ� PlayerPrefs�� �����Ѵ�.
        if (lastClearStage < nowClearStage)
            PlayerPrefs.SetInt("Clear Stage", nowClearStage);
    }

    private void SaveTowerKillCountData()
    {
        Tower.ResetTowerKillCount();
        return;
    }
}

/*
 * File : StageManager.cs
 * First Update : 2022/04/22 FRI 02:30
 * PokerTower Defense ������ ��ü���� ������ ������ StageManager ��ũ��Ʈ
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
