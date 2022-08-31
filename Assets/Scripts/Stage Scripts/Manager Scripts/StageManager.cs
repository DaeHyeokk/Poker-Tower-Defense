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

    private readonly float[] _enemyHpPercentages = new float[] { 0.5f, 1f, 1.5f, 3f };
    private readonly float[] _enemySpeedPercentages = new float[] { 0.8f, 1f, 1.2f, 1.5f };

    private int _gold;
    private int _mineral;
    private int _changeChance;
    private int _jokerCard;
    private int _clearCount;
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

    public GameManager.StageDifficulty stageDifficulty => GameManager.instance.stageDifficulty;
    public bool isEnd => _isEnd;
    public int pokerCount => _pokerCount;
    public int clearCount => _clearCount;
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

        _clearCount = PlayerPrefs.GetInt(stageDifficulty.ToString());

        _stageDataUIController.SetGoldAmountText(_gold);
        _stageDataUIController.SetMineralAmountText(_mineral);

        ScreenSleepSetup();
    }

    private void OnApplicationPause(bool pause)
    {
        // 게임이 이미 멈춘 상태거나 끝난 상태가 아니라면 게임을 멈춘다.
        if (pause)
            if (!_isPaused && !_isEnd)
                PauseGame();
    }

    private void ScreenSleepSetup()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    public void OnClickSpeedUpButton()
    {
        gameSpeed++;
        Time.timeScale = gameSpeed;
        _stageDataUIController.SetGameSpeedText(_gameSpeed);
    }

    public void OnClickPauseButton() => PauseGame();

    public void OnClickResumeButton() => ResumeGame();

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
        onStagePaused();
        _isPaused = true;
        _backupGameSpeed = gameSpeed;
        gameSpeed = 0f;

        // 게임이 종료된 경우 (클리어 후 게임으로 되돌아간 경우) 게임 클리어 UI를 활성화 한다.
        if (_isEnd)
            _stageClearPanelObject.SetActive(true);
        // 게임이 진행중인 경우 게임 메뉴 UI를 활성화 한다.
        else
        {
            _stageMenuPanelObject.SetActive(true);
            //SoundManager.instance.PauseBGM();
            SoundManager.instance.PlaySFX(SoundFileNameDictionary.stagePauseSound);
        }
    }

    private void ResumeGame()
    {
        onStageResumed();
        _isPaused = false;
        gameSpeed = _backupGameSpeed;

        // 게임이 종료된 경우 (클리어 후 게임으로 되돌아간 경우) 게임 클리어 UI를 비활성화 한다.
        if (_isEnd)
            _stageClearPanelObject.SetActive(false);
        // 게임이 진행중인 경우 게임 메뉴 UI를 비활성화 한다.
        else
        {
            _stageMenuPanelObject.SetActive(false);
            //SoundManager.instance.ResumeBGM();
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
        _isEnd = true;
        _backupGameSpeed = gameSpeed;
        gameSpeed = 0f;
        _clearCount++;
        _stageClearPanelObject.SetActive(true);

        SaveClearStageData();
        IncreaseTowerKillCountData(true);
        PlayerPrefs.Save();

        SoundManager.instance.PlayBGM(SoundFileNameDictionary.stageClearBGM);
    }

    public void DefeatGame()
    {
        onStageEnd();
        _isEnd = true;
        gameSpeed = 0f;
        _stageDefeatPanelObject.SetActive(true);

        IncreaseTowerKillCountData(false);
        PlayerPrefs.Save();

        SoundManager.instance.PauseBGM();
        SoundManager.instance.PlaySFX(SoundFileNameDictionary.stageDefeatSound);
    }

    private void SaveClearStageData()
    {
        PlayerPrefs.SetInt(stageDifficulty.ToString(), clearCount);

        int lastClearStage = PlayerPrefs.HasKey("Clear Stage") ? PlayerPrefs.GetInt("Clear Stage") : -1;
        int nowClearStage = (int)GameManager.instance.stageDifficulty;

        // 플레이어가 클리어한 최대 난이도보다 높은 난이도를 클리어했다면 PlayerPrefs를 갱신한다.
        if (lastClearStage < nowClearStage)
            PlayerPrefs.SetInt("Clear Stage", nowClearStage);
    }

    private void IncreaseTowerKillCountData(bool isClear)
    {
        Dictionary<string, KeyValuePair<int, int>> towerDataDict = PlayerDataManager.instance.towerDataDict;

        for(int i=0; i<Tower.towerTypeNames.Length; i++)
        {
            // 게임을 클리어한 경우 타워가 기록한 킬수를 두배로 하고 원래 값에 누적시킨다.
            int stageKillsCount;
            if (isClear)
                stageKillsCount = Tower.GetKillCount((i) * 2);
            else
                stageKillsCount = Tower.GetKillCount(i);

            // PlayerDataManager에서 참조한 딕셔너리에 저장된 값을 대입한다.
            string towerName = Tower.towerTypeNames[i];
            int level = towerDataDict[towerName].Key;
            int killCount = towerDataDict[towerName].Value;
            int nextLevelKillCount = level * Tower.defaultLevelupKillCount;

            // 타워의 킬 카운트에 이번 스테이지에서 획득한 킬 카운트를 누적시킨다.
            killCount += stageKillsCount;

            // 만약 킬 카운트가 다음 레벨에 필요한 킬 카운트를 넘었다면 타워를 레벨업하고 킬수를 차감한다.
            if(killCount >= nextLevelKillCount)
            {
                level++;
                killCount -= nextLevelKillCount;
            }

            // 딕셔너리에서 해당 타워의 Value값을 갱신한다.
            towerDataDict[towerName] = new KeyValuePair<int, int>(level, killCount);
        }

        // 타워의 킬수를 모두 누적시켰다면 데이터를 저장한다.
        PlayerDataManager.instance.SavePlayerPrefsTowerData();

        // 타워가 스테이지에서 기록한 킬수를 초기화 한다.
        Tower.ResetTowerKillCount();
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
