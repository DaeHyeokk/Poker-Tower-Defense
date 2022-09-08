using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GoogleMobileAds.Api;
using TMPro;

public class StageManager : MonoBehaviour
{
    public enum StageDifficulty { Easy, Normal, Hard, Hell }
    public static StageDifficulty stageDifficulty { get; set; }

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
    private WaveSystem _waveSystem;
    [SerializeField]
    private GameObject _stageMenuPanelObject;
    [SerializeField]
    private GameObject _stageClearPanelObject;
    [SerializeField]
    private GameObject _stageDefeatPanelObject;
    [SerializeField]
    private GameObject _loadingUIPanelObject;
    [SerializeField]
    private GameObject _saveFailedPanelObject;

    private readonly float[] _enemyHpPercentages = new float[] { 0.5f, 1f, 1.5f, 3f };
    private readonly float[] _enemySpeedPercentages = new float[] { 0.8f, 1f, 1.2f, 1.5f };

    private int _gold = 400;
    private int _mineral = 100;
    private int _changeChance;
    private int _jokerCard;
    private int _clearCount;
    private int _bestRoundRecord;
    private float _gameSpeed;
    private float _backupGameSpeed;
    private float _maxGameSpeed;
    private bool _isPaused;
    private bool _isEnd;
    private bool _isInterstitialShowed;
    public event Action onStagePaused;
    public event Action onStageResumed;
    public event Action onStageEnd;

    public int gold
    {
        get => _gold;
        set
        {
            if (_gold < value)
                SoundManager.instance.PlaySFX(SoundFileNameDictionary.goldIncreaseSound);

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
                SoundManager.instance.PlaySFX(SoundFileNameDictionary.mineralIncreaseSound);

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

    private PlayerGameData _playerGameData => GameManager.instance.playerGameData;

    public bool isEnd => _isEnd;
    public int pokerCount => _pokerCount;
    public int clearCount => _clearCount;
    public int bestRoundRecord => _bestRoundRecord;
    public float enemyHpPercentage => _enemyHpPercentages[(int)stageDifficulty];
    public float enemySpeedPercentage => _enemySpeedPercentages[(int)stageDifficulty];

    private void Awake()
    {
        if (instance != this)
            Destroy(gameObject);

        _gold = 400;
        _mineral = 100;
        changeChance = 5;
        jokerCard = 2;

        _maxGameSpeed = GameManager.instance.playerGameData.isPurchased3xSpeed ? 3 : 2;
        _backupGameSpeed = 1f;
        gameSpeed = 1f;

        _stageDataUIController.SetGoldAmountText(_gold);
        _stageDataUIController.SetMineralAmountText(_mineral);
        
        _clearCount = _playerGameData.playerStageDataList[(int)stageDifficulty].clearCount;
        _bestRoundRecord = _playerGameData.playerStageDataList[(int)stageDifficulty].bestRecord;

        // Ÿ���� ���� ������������ ����ߴ� ų���� �ʱ�ȭ �Ѵ�.
        Tower.ResetTowerKillCount();

        UIManager.instance.GameStartScreenCoverFadeOut();
        SoundManager.instance.PlayBGM(SoundFileNameDictionary.mainBGM);
        GameManager.instance.SetScreenResolution();

        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        LoadGoogleAds();
    }

    private void OnApplicationPause(bool pause)
    {
        // ������ �̹� ���� ���°ų� ���� ���°� �ƴ϶�� ������ �����.
        if (pause)
        {
            if (!_isPaused && !_isEnd)
                ShowPauseUIPanel();
        }
        else
        {
            // ������ �簳 �� �� ���� �����ִ� ���¿����� ������ �簳�Ѵ�.
            if (_isInterstitialShowed)
            {
                this.ResumeGame();
                SoundManager.instance.PlayBGM(SoundFileNameDictionary.mainBGM);
                _isInterstitialShowed = false;
            }
        }
    }

    private void LoadGoogleAds()
    {
        // GoogleAdsManager�κ��� interstitialAd ��ü�� ������.
        InterstitialAd interstitialAd = GoogleAdsManager.instance.interstitialAd;

        // interstitialAd�� ���� null�� �ƴ϶�� ����.
        if (interstitialAd != null)
        {
            // BGM�� ������ ���߰� �÷��̾�� ���� �����ش�.
            if (interstitialAd.IsLoaded())
            {
                _isInterstitialShowed = true;
                this.PauseGame();
                SoundManager.instance.PauseBGM();
                interstitialAd.Show();
            }
            // ���� �ε忡 �����ߴٸ� �ϴ� �Ѿ�鼭 ���� �ε带 �ٽ� �õ��Ѵ�.
            else
                GoogleAdsManager.instance.ReloadInterstitialAd();
        }

        // �ϴܿ� ���� ��� ���� ���
        GoogleAdsManager.instance.LoadBanner(GoogleAdsManager.BannerAdSizeType.Standard);

    }

    public void OnClickSpeedUpButton()
    {
        gameSpeed++;
        Time.timeScale = gameSpeed;
        _stageDataUIController.SetGameSpeedText(_gameSpeed);
    }

    public void ShowPauseUIPanel()
    {
        // ������ ����� ��� (Ŭ���� �� �������� �ǵ��ư� ���) ���� Ŭ���� UI�� Ȱ��ȭ �Ѵ�.
        if (_isEnd)
            _stageClearPanelObject.SetActive(true);
        // ������ �������� ��� ���� �޴� UI�� Ȱ��ȭ �Ѵ�.
        else
            _stageMenuPanelObject.SetActive(true);

        PauseGame();
    }

    public void HidePauseUIPanel()
    {
        // ������ ����� ��� (Ŭ���� �� �������� �ǵ��ư� ���) ���� Ŭ���� UI�� ��Ȱ��ȭ �Ѵ�.
        if (_isEnd)
            _stageClearPanelObject.SetActive(false);
        // ������ �������� ��� ���� �޴� UI�� ��Ȱ��ȭ �Ѵ�.
        else
            _stageMenuPanelObject.SetActive(false);

        ResumeGame();
    }

    public void OnClickRestartButton()
    {
        // ������ ������ ���� ��Ȳ�̶�� �÷��̾ �߸� ������ ��츦 ����� ��Ȯ���Ѵ�.
        // ������ ���� ��Ȳ�̶�� ��Ȯ������ �ʰ� �ٷ� �����Ѵ�.
        if (_isEnd)
            RestartGame();
        else
        {
            UIManager.instance.actionReconfirmation.actionReconfirmationText.text = "������ \n�ٽ� �����Ͻðڽ��ϱ�?";
            UIManager.instance.actionReconfirmation.onYesButton += RestartGame;
            UIManager.instance.actionReconfirmation.gameObject.SetActive(true);
        }
    }

    public void OnClickQuitButton()
    {
        // ������ ������ ���� ��Ȳ�̶�� �÷��̾ �߸� ������ ��츦 ����� ��Ȯ���Ѵ�.
        // ������ ���� ��Ȳ�̶�� ��Ȯ������ �ʰ� �ٷ� �����Ѵ�.
        if (_isEnd)
            ReturnLobby();
        else
        {
            UIManager.instance.actionReconfirmation.actionReconfirmationText.text = "������ \n�����ðڽ��ϱ�?";
            UIManager.instance.actionReconfirmation.onYesButton += ReturnLobby;
            UIManager.instance.actionReconfirmation.gameObject.SetActive(true);
        }
    }

    private void PauseGame()
    {
        if(!_isPaused)
        { 
            onStagePaused();
            _isPaused = true;
            _backupGameSpeed = gameSpeed;
            gameSpeed = 0f;
        }
    }

    private void ResumeGame()
    {
        if (_isPaused)
        {
            onStageResumed();
            _isPaused = false;
            gameSpeed = _backupGameSpeed;
        }
    }

    private void ReturnLobby()
    {
        SceneManager.LoadScene("LobbyScene");
    }

    private void RestartGame()
    {
        SceneManager.LoadScene("SingleStageScene");
    }

    public void ClearGame()
    {
        onStageEnd();
        _isPaused = true;
        _isEnd = true;
        _backupGameSpeed = gameSpeed;
        gameSpeed = 0f;
        _clearCount++;
        _stageClearPanelObject.SetActive(true);

        // �÷��̾ �α����� ���� ��쿡�� �����͸� �����Ѵ�.
        if (Social.localUser.authenticated)
        {
            // ���� ���̵��� ó�� Ŭ���� �ߴٸ� �ִ� ���嵵 �����Ѵ�.
            if (_clearCount == 1)
                SaveBestRecordData();
            SaveClearStageData();
            AddPlayerTowerKillCountData(true);
            // �߿��� �������̹Ƿ� �ٷ� �����Ѵ�.
            SaveGoogleCloud();
        }

        SoundManager.instance.PlayBGM(SoundFileNameDictionary.stageClearBGM);
    }

    public void DefeatGame()
    {
        onStageEnd();
        _isPaused = true;
        _isEnd = true;
        gameSpeed = 0f;
        _stageDefeatPanelObject.SetActive(true);

        // �÷��̾ �α����� ���� ��쿡�� �����͸� �����Ѵ�.
        if (Social.localUser.authenticated)
        {
            if (_waveSystem.wave > _bestRoundRecord)
                SaveBestRecordData();
            AddPlayerTowerKillCountData(false);
            // �߿��� �������̹Ƿ� �ٷ� �����Ѵ�.
            SaveGoogleCloud();
        }
        SoundManager.instance.PauseBGM();
        SoundManager.instance.PlaySFX(SoundFileNameDictionary.stageDefeatSound);
    }

    private void SaveClearStageData()
    {
        PlayerStageData playerStageData = _playerGameData.playerStageDataList[(int)stageDifficulty];
        playerStageData.clearCount = this.clearCount;
        _playerGameData.playerStageDataList[(int)stageDifficulty] = playerStageData;
    }

    private void SaveBestRecordData()
    {
        PlayerStageData playerStageData = _playerGameData.playerStageDataList[(int)stageDifficulty];
        playerStageData.bestRecord = _waveSystem.wave;
        _playerGameData.playerStageDataList[(int)stageDifficulty] = playerStageData;
    }

    private void AddPlayerTowerKillCountData(bool isClear)
    {
        for(int i=0; i<Tower.towerTypeNames.Length; i++)
        {
            // ������ Ŭ������ ��� Ÿ���� ����� ų���� �ι�� �Ѵ�.
            int stageKillsCount;
            if (isClear)
                stageKillsCount = Tower.GetKillCount(i) * 2;
            else
                stageKillsCount = Tower.GetKillCount(i);

            // �߰������� Ȱ��ȭ �Ǿ��ִ� ��� ����� ų���� �ι�� �Ѵ�.
            stageKillsCount = Tower.GetKillCount(i) * (GameManager.instance.playerGameData.isBonusRewardActived ? 2 : 1);

            Tower.AddPlayerTowerKillCount(i, stageKillsCount);
        }

        // Ÿ���� ų���� ��� �������״ٸ� �����͸� �����Ѵ�.
        for (int i = 0; i < Tower.towerTypeNames.Length; i++)
        {
            PlayerTowerData playerTowerData = GameManager.instance.playerGameData.playerTowerDataList[i];
            int level = playerTowerData.level;
            int killCount = playerTowerData.killCount;

            _playerGameData.playerTowerDataList[i] = new(level, killCount);
        }

        // ���ʽ� ������ Ȱ��ȭ�� ���¿��ٸ� ��Ȱ��ȭ ���·� �ǵ�����.
        if (_playerGameData.isBonusRewardActived)
            _playerGameData.isBonusRewardActived = false;
    }

    public void SaveGoogleCloud()
    {
        _loadingUIPanelObject.SetActive(true);

        GameManager.instance.Save(
            () => _loadingUIPanelObject.SetActive(false),
            () =>
            {
                _loadingUIPanelObject.SetActive(false);
                _saveFailedPanelObject.SetActive(true);
            });
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
