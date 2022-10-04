using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GoogleMobileAds.Api;

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
    private GameObject _tutorialObject;
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

    private readonly float[] _roundEnemyHpPercentages = new float[] { 1f, 2f, 4f, 8f };
    private readonly float[] _bossEnemyHpPercentages = new float[] { 0.5f, 1f, 2f, 4f };
    private readonly float[] _specialBossHpPercentages = new float[] { 0.85f, 1f, 1.15f, 1.3f };
    private readonly float[] _enemySpeedPercentages = new float[] { 0.85f, 1f, 1.15f, 1.3f };

    private int _gold = 400;
    private int _mineral = 100;
    private int _changeChance;
    private int _jokerCard;
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
            {
                if (_maxGameSpeed == 2)
                    _gameSpeed = 1f;
                else
                    _gameSpeed = 0.5f;
            }
            else if (_gameSpeed == 1.5f)
                _gameSpeed = 1f;

            Time.timeScale = _gameSpeed;
            Time.fixedDeltaTime = 0.02f * _gameSpeed;
        }
    }

    private PlayerGameData _playerGameData => GameManager.instance.playerGameData;

    public bool isEnd => _isEnd;
    public int pokerCount => _pokerCount;
    public int bestRoundRecord => _playerGameData.playerStageDataList[(int)stageDifficulty].bestRoundRecord;
    public float bestBossKilledTakenTimeRecord => _playerGameData.playerStageDataList[(int)stageDifficulty].bestBossKilledTakenTimeRecord;
    public float bossKilledTakenTime { get; set; }
    public float roundEnemyHpPercentage => _roundEnemyHpPercentages[(int)stageDifficulty];
    public float bossEnemyHpPercentage => _bossEnemyHpPercentages[(int)stageDifficulty];
    public float specialBossHpPercentage => _specialBossHpPercentages[(int)stageDifficulty];
    public float enemySpeedPercentage => _enemySpeedPercentages[(int)stageDifficulty];

    public GameObject loadingUIPanelObject => _loadingUIPanelObject;

    private void Awake()
    {
        if (instance != this)
            Destroy(gameObject);
        else
        {
            _gold = 400;
            _mineral = 100;

            _stageDataUIController.SetGoldAmountText(_gold);
            _stageDataUIController.SetMineralAmountText(_mineral);

            changeChance = 10;
            // �÷��̾ �����̾��н��� ������ ��� ī�屳ȯ�� 10���� �߰� ����.
            if (IAPManager.instance.HadPurchashed(IAPManager.instance.productPremiumPass))
                changeChance += 10;

            // �÷��̾ �߰� ��� ��ǰ�� ������ ��� �ִ� ���� ���ǵ带 3���� �Ѵ�.
            _maxGameSpeed = IAPManager.instance.HadPurchashed(IAPManager.instance.productExtraGameSpeed) ? 3 : 2;
            _backupGameSpeed = 1f;
            gameSpeed = 1f;

            // Ÿ���� ���� ������������ ����ߴ� ų���� �ʱ�ȭ �Ѵ�.
            Tower.ResetTowerKillCount();

            UIManager.instance.GameStartScreenCoverFadeOut();
            SoundManager.instance.PlayBGM(SoundFileNameDictionary.mainBGM);
            GameManager.instance.SetScreenResolution();

            Screen.sleepTimeout = SleepTimeout.NeverSleep;

            LoadGoogleAds();
        }
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

        // interstitialAd�� ���� null�� �ƴ϶�� ����
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
    }

    public void OnClickSpeedUpButton()
    {
        gameSpeed++;
        _stageDataUIController.SetGameSpeedText(gameSpeed);
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
        _stageClearPanelObject.SetActive(true);

        // �÷��̾ �α����� ���� ��쿡�� �����͸� �����Ѵ�.
        if (Social.localUser.authenticated)
        {
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
            if (_waveSystem.wave > bestRoundRecord)
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
        // �ְ� ��Ϻ��� �� ������ ������ ��Ҵٸ� �ְ� ����� �����ϰ�, �������忡 ����Ѵ�.
        if (playerStageData.bestBossKilledTakenTimeRecord > bossKilledTakenTime)
        {
            if(playerStageData.clearCount == 0)
                SaveBestRecordData();

            playerStageData.clearCount++;
            playerStageData.bestBossKilledTakenTimeRecord = bossKilledTakenTime;
            _playerGameData.playerStageDataList[(int)stageDifficulty] = playerStageData;
            // �������� ��� �޼ҵ� ȣ��.
            GameManager.instance.ReportBossKilledTakenTime(stageDifficulty, bossKilledTakenTime);
        }
    }

    private void SaveBestRecordData()
    {
        PlayerStageData playerStageData = _playerGameData.playerStageDataList[(int)stageDifficulty];
        playerStageData.bestRoundRecord = _waveSystem.wave;
        _playerGameData.playerStageDataList[(int)stageDifficulty] = playerStageData;
    }

    private void AddPlayerTowerKillCountData(bool isClear)
    {
        for(int i=0; i<Tower.towerTypeNames.Length; i++)
        {
            int stageKillsCount = GetStageTowerKillCount(i, isClear);
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

    public int GetStageTowerKillCount(int towerIndex, bool isClear)
    {
        // ������ Ŭ������ ��� Ÿ���� ����� ų���� �ι�� �Ѵ�.
        int stageKillsCount;
        if (isClear)
            stageKillsCount = Tower.GetKillCount(towerIndex) * 2;
        else
            stageKillsCount = Tower.GetKillCount(towerIndex);

        // �߰������� Ȱ��ȭ �Ǿ��ִ� ��� ����� ų���� �ι�� �Ѵ�.
        stageKillsCount *= (GameManager.instance.playerGameData.isBonusRewardActived ? 2 : 1);

        return stageKillsCount;
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
