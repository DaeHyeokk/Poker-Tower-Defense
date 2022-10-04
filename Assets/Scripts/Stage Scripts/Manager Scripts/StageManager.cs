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
            // 플레이어가 프리미엄패스를 구입한 경우 카드교환권 10장을 추가 지급.
            if (IAPManager.instance.HadPurchashed(IAPManager.instance.productPremiumPass))
                changeChance += 10;

            // 플레이어가 추가 배속 상품을 구입한 경우 최대 게임 스피드를 3으로 한다.
            _maxGameSpeed = IAPManager.instance.HadPurchashed(IAPManager.instance.productExtraGameSpeed) ? 3 : 2;
            _backupGameSpeed = 1f;
            gameSpeed = 1f;

            // 타워가 이전 스테이지에서 기록했던 킬수를 초기화 한다.
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
        // 게임이 이미 멈춘 상태거나 끝난 상태가 아니라면 게임을 멈춘다.
        if (pause)
        {
            if (!_isPaused && !_isEnd)
                ShowPauseUIPanel();
        }
        else
        {
            // 게임이 재개 될 때 광고를 보고있던 상태였으면 게임을 재개한다.
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
        // GoogleAdsManager로부터 interstitialAd 객체를 가져옴.
        InterstitialAd interstitialAd = GoogleAdsManager.instance.interstitialAd;

        // interstitialAd의 값이 null이 아니라면 수행
        if (interstitialAd != null)
        {
            // BGM과 게임을 멈추고 플레이어에게 광고를 보여준다.
            if (interstitialAd.IsLoaded())
            {
                _isInterstitialShowed = true;
                this.PauseGame();
                SoundManager.instance.PauseBGM();
                interstitialAd.Show();
            }
            // 광고 로드에 실패했다면 일단 넘어가면서 광고 로드를 다시 시도한다.
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
        // 게임이 종료된 경우 (클리어 후 게임으로 되돌아간 경우) 게임 클리어 UI를 활성화 한다.
        if (_isEnd)
            _stageClearPanelObject.SetActive(true);
        // 게임이 진행중인 경우 게임 메뉴 UI를 활성화 한다.
        else
            _stageMenuPanelObject.SetActive(true);

        PauseGame();
    }

    public void HidePauseUIPanel()
    {
        // 게임이 종료된 경우 (클리어 후 게임으로 되돌아간 경우) 게임 클리어 UI를 비활성화 한다.
        if (_isEnd)
            _stageClearPanelObject.SetActive(false);
        // 게임이 진행중인 경우 게임 메뉴 UI를 비활성화 한다.
        else
            _stageMenuPanelObject.SetActive(false);

        ResumeGame();
    }

    public void OnClickRestartButton()
    {
        // 게임이 끝나지 않은 상황이라면 플레이어가 잘못 눌렀을 경우를 대비해 재확인한다.
        // 게임이 끝난 상황이라면 재확인하지 않고 바로 수행한다.
        if (_isEnd)
            RestartGame();
        else
        {
            UIManager.instance.actionReconfirmation.actionReconfirmationText.text = "게임을 \n다시 시작하시겠습니까?";
            UIManager.instance.actionReconfirmation.onYesButton += RestartGame;
            UIManager.instance.actionReconfirmation.gameObject.SetActive(true);
        }
    }

    public void OnClickQuitButton()
    {
        // 게임이 끝나지 않은 상황이라면 플레이어가 잘못 눌렀을 경우를 대비해 재확인한다.
        // 게임이 끝난 상황이라면 재확인하지 않고 바로 수행한다.
        if (_isEnd)
            ReturnLobby();
        else
        {
            UIManager.instance.actionReconfirmation.actionReconfirmationText.text = "게임을 \n나가시겠습니까?";
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

        // 플레이어가 로그인을 했을 경우에만 데이터를 저장한다.
        if (Social.localUser.authenticated)
        {
            SaveClearStageData();
            AddPlayerTowerKillCountData(true);
            // 중요한 데이터이므로 바로 저장한다.
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

        // 플레이어가 로그인을 했을 경우에만 데이터를 저장한다.
        if (Social.localUser.authenticated)
        {
            if (_waveSystem.wave > bestRoundRecord)
                SaveBestRecordData();
            AddPlayerTowerKillCountData(false);
            // 중요한 데이터이므로 바로 저장한다.
            SaveGoogleCloud();
        }

        SoundManager.instance.PauseBGM();
        SoundManager.instance.PlaySFX(SoundFileNameDictionary.stageDefeatSound);
    }

    private void SaveClearStageData()
    {
        PlayerStageData playerStageData = _playerGameData.playerStageDataList[(int)stageDifficulty];
        // 최고 기록보다 더 빠르게 보스를 잡았다면 최고 기록을 갱신하고, 리더보드에 등록한다.
        if (playerStageData.bestBossKilledTakenTimeRecord > bossKilledTakenTime)
        {
            if(playerStageData.clearCount == 0)
                SaveBestRecordData();

            playerStageData.clearCount++;
            playerStageData.bestBossKilledTakenTimeRecord = bossKilledTakenTime;
            _playerGameData.playerStageDataList[(int)stageDifficulty] = playerStageData;
            // 리더보드 등록 메소드 호출.
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

        // 타워의 킬수를 모두 누적시켰다면 데이터를 저장한다.
        for (int i = 0; i < Tower.towerTypeNames.Length; i++)
        {
            PlayerTowerData playerTowerData = GameManager.instance.playerGameData.playerTowerDataList[i];
            int level = playerTowerData.level;
            int killCount = playerTowerData.killCount;

            _playerGameData.playerTowerDataList[i] = new(level, killCount);
        }

        // 보너스 보상이 활성화된 상태였다면 비활성화 상태로 되돌린다.
        if (_playerGameData.isBonusRewardActived)
            _playerGameData.isBonusRewardActived = false;
    }

    public int GetStageTowerKillCount(int towerIndex, bool isClear)
    {
        // 게임을 클리어한 경우 타워가 기록한 킬수를 두배로 한다.
        int stageKillsCount;
        if (isClear)
            stageKillsCount = Tower.GetKillCount(towerIndex) * 2;
        else
            stageKillsCount = Tower.GetKillCount(towerIndex);

        // 추가보상이 활성화 되어있는 경우 기록한 킬수를 두배로 한다.
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
 * PokerTower Defense 게임의 전체적인 정보를 관리할 StageManager 스크립트
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
