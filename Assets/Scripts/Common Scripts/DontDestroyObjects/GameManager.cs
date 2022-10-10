using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;

[Serializable]
public class PlayerGameData
{
    public bool isDataRestored;
    public bool isBonusRewardActived;
    public List<PlayerStageData> playerStageDataList;
    public List<PlayerTowerData> playerTowerDataList;

    public PlayerGameData()
    {
        isDataRestored = false;
        isBonusRewardActived = false;
        playerStageDataList = new(4); // Easy, Normal, Hard, Hell
        playerTowerDataList = new(Tower.towerTypeNames.Length);

        for(int i=0; i<playerStageDataList.Capacity; i++)
        {
            int clearCount = 0;
            int bestRoundRecord = 0;
            float bestBossKilledTakenTimeRecord = PlayerStageData.DEFAULT_TAKEN_TIME;

            playerStageDataList.Add(new(clearCount, bestRoundRecord, bestBossKilledTakenTimeRecord));
        }
        
        for(int i=0; i<playerTowerDataList.Capacity; i++)
        {
            int level = 1;
            int killCount = 0;

            playerTowerDataList.Add(new(level, killCount));
        }
    }

    public void SetDefaultValue()
    {
        isBonusRewardActived = false;

        for (int i = 0; i < playerStageDataList.Capacity; i++)
        {
            int clearCount = 0;
            int bestRoundRecord = 0;
            float bestBossKilledTakenTimeRecord = PlayerStageData.DEFAULT_TAKEN_TIME;

            playerStageDataList[i] = new(clearCount, bestRoundRecord, bestBossKilledTakenTimeRecord);
        }

        for (int i = 0; i < playerTowerDataList.Capacity; i++)
        {
            int level = 1;
            int killCount = 0;

            playerTowerDataList[i] = new(level, killCount);
        }
    }
}

[Serializable]
public struct PlayerStageData
{
    public const float DEFAULT_TAKEN_TIME = 99999f;

    public int clearCount;
    public int bestRoundRecord;
    public float bestBossKilledTakenTimeRecord;

    public PlayerStageData(int clearCount, int bestRoundRecord, float bestBossKilledTakenTimeRecord)
    {
        this.clearCount = clearCount;
        this.bestRoundRecord = bestRoundRecord;
        this.bestBossKilledTakenTimeRecord = bestBossKilledTakenTimeRecord;
    }
}

[Serializable]
public struct PlayerTowerData
{
    public int level;
    public int killCount;

    public PlayerTowerData(int level, int killCount)
    {
        this.level = level;
        this.killCount = killCount;
    }
}

public class GameManager : MonoBehaviour
{
    private static GameManager s_instance;
    public static GameManager instance
    {
        get
        {
            if (s_instance == null)
                s_instance = FindObjectOfType<GameManager>();

            return s_instance;
        }
    }

    private event Action onSuccessed;
    private event Action onFailed;

    public PlayerGameData playerGameData { get; set; } = new();

    private void Awake()
    {
        if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            // GameManager�� ���� �����Ǵ� ���.
            // ���� ����Ǿ �ı����� �ʴ� ������Ʈ�� �����Ѵ�.
            DontDestroyOnLoad(instance);
        }

        var config = new PlayGamesClientConfiguration.Builder().EnableSavedGames().Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
    }

    private void OnApplicationPause(bool pause)
    {

        if (pause && Social.localUser.authenticated && SceneManager.GetActiveScene().name != "GameLoadingScene")
            Save();

        PlayerPrefs.Save();
    }

    // ���ͳݿ� ����Ǿ� ���� ��� true �ȵǾ� ���� ��� false ��ȯ.
    public bool CheckNetworkReachable() => !(Application.internetReachability == NetworkReachability.NotReachable);

    private IEnumerator CheckNetworkReachableCoroutine()
    {
        float waitDelay = 0f;
        float maximumDelay = 3f;

        yield return new WaitForSeconds(1f);

        // ���ͳݿ� ������� �ʾҰ� waitDelay�� 3�� �̸��� ��� ���� 
        while (!CheckNetworkReachable() && waitDelay < maximumDelay)
        {
            yield return null;
            waitDelay += Time.deltaTime;
        }

        // 3�ʵ��� ��ٷ����� ���ͳ� ������ �ȵ��� ��� ����
        if (waitDelay >= maximumDelay)
        {
            if (onFailed != null) onFailed();
        }
        else
        {
            if (onSuccessed != null) onSuccessed();
        }
    }

    public void Login(Action onSuccessed = null, Action onFailed = null)
    {
        // �̹� �α��� �� ����ڶ�� �ǳʶڴ�.
        if (Social.localUser.authenticated) return;

        if (this.onSuccessed != null)
            this.onSuccessed = null;
        this.onSuccessed += onSuccessed;

        if (this.onFailed != null)
            this.onFailed = null;
        this.onFailed += onFailed;

        // �����÷��� �α��� �õ�
        Social.localUser.Authenticate((bool success) =>
        {
            if (success)
            {
                if (onSuccessed != null) onSuccessed();
            }
            else
            {
                if (onFailed != null) onFailed();
            }
        });
    }

    public void Logout()
    {
        PlayGamesPlatform.Instance.SignOut();
    }

    internal void ProcessAuthentication(SignInStatus status)
    {
        if (status == SignInStatus.Success)
        {
            if (onSuccessed != null) onSuccessed();
        }
        else
        {
            if (onFailed != null) onFailed();
        }
    }

    private bool _isSaving;
    private readonly string SNAPSHOT_NAME = "PlayerGameData";

    #region ������ �ҷ�����
    private void OpenSavedGame(string filename)
    {
        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
        savedGameClient.OpenWithAutomaticConflictResolution(filename, DataSource.ReadCacheOrNetwork,
            ConflictResolutionStrategy.UseLongestPlaytime, OnSavedGameOpened);
    }

    private void OnSavedGameOpened(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        // �������� ���µ� �������� ��� ����
        if (status == SavedGameRequestStatus.Success)
        {
            if (_isSaving)
            {
                SaveData(game);
            }
            else
            {
                LoadData(game);
            }
        }
        else
        {
            // ������ �ҷ����� ����.
            if (onFailed != null) onFailed();
        }
    }
    #endregion

    #region ������ �ε�
    public void Load(Action onSuccessed = null, Action onFailed = null)
    {
        if (Social.localUser.authenticated)
        {
            if (this.onSuccessed != null)
                this.onSuccessed = null;
            this.onSuccessed += onSuccessed;

            if (this.onFailed != null)
                this.onFailed = null;
            this.onFailed += onFailed;

            _isSaving = false;
            OpenSavedGame(SNAPSHOT_NAME);
        }
    }

    private void LoadData(ISavedGameMetadata game)
    {
        PlayGamesPlatform.Instance.SavedGame.ReadBinaryData(game, LoadDataCallBack);
    }

    private void LoadDataCallBack(SavedGameRequestStatus status, byte[] loadedData)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            // �޾ƿ� �����͸� string ���·� ���ڵ� �ϰ�, MyGameData Ŭ������ ��ȯ�Ѵ�.
            string myGameDataString = Encoding.UTF8.GetString(loadedData);

            // gameDataString ���ڿ��� �� ���ڿ��̶�� (����� �����Ͱ� ���ٸ�)
            // �⺻������ �����Ѵ�.
            if (myGameDataString == "")
                playerGameData.SetDefaultValue();
            // ����� �����Ͱ� �ִٸ� ������ ���� �����Ѵ�.
            else
                playerGameData = JsonUtility.FromJson<PlayerGameData>(myGameDataString);

            if (onSuccessed != null) onSuccessed();
        }
        else
        {
            if (onFailed != null) onFailed();
        }
    }
    #endregion

    #region ������ ���̺�
    public void Save(Action onSuccessed = null, Action onFailed = null)
    {
        if (Social.localUser.authenticated)
        {
            if (this.onSuccessed != null)
                this.onSuccessed = null;
            this.onSuccessed += onSuccessed;

            if (this.onFailed != null)
                this.onFailed = null;
            this.onFailed += onFailed;

            _isSaving = true;
            OpenSavedGame(SNAPSHOT_NAME);
        }
    }

    public void SaveData(ISavedGameMetadata game)
    {
        string playerGameDataString = JsonUtility.ToJson(playerGameData);
        byte[] playerGameDataBinary = Encoding.UTF8.GetBytes(playerGameDataString);

        SavedGameMetadataUpdate update = new SavedGameMetadataUpdate.Builder().Build();
        PlayGamesPlatform.Instance.SavedGame.CommitUpdate(game, update, playerGameDataBinary, SaveCallBack);
    }

    private void SaveCallBack(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            if (onSuccessed != null) onSuccessed();
        }
        else
        {
            if (onFailed != null) onFailed();
        }
    }
    #endregion

    #region ��������
    public void ReportBossKilledTakenTime(StageManager.StageDifficulty stageDifficulty, float bossKilledtakenTime)
    {
        if (stageDifficulty == StageManager.StageDifficulty.Easy)
            PlayGamesPlatform.Instance.ReportScore((long)(bossKilledtakenTime * 1000), GPGSIds.leaderboard_easy_mode_boss_killed_taken_time, (bool success) => { });

        else if(stageDifficulty == StageManager.StageDifficulty.Normal)
            PlayGamesPlatform.Instance.ReportScore((long)(bossKilledtakenTime * 1000), GPGSIds.leaderboard_normal_mode_boss_killed_taken_time, (bool success) => { });

        else if (stageDifficulty == StageManager.StageDifficulty.Hard)
            PlayGamesPlatform.Instance.ReportScore((long)(bossKilledtakenTime * 1000), GPGSIds.leaderboard_hard_mode_boss_killed_taken_time, (bool success) => { });

        else if (stageDifficulty == StageManager.StageDifficulty.Hell)
            PlayGamesPlatform.Instance.ReportScore((long)(bossKilledtakenTime * 1000), GPGSIds.leaderboard_hell_mode_boss_killed_taken_time, (bool success) => { });
    }

    public void ShowBossKilledTakenTimeLeaderboard(StageManager.StageDifficulty stageDifficulty)
    {
        if (stageDifficulty == StageManager.StageDifficulty.Easy)
            PlayGamesPlatform.Instance.ShowLeaderboardUI(GPGSIds.leaderboard_easy_mode_boss_killed_taken_time);

        else if (stageDifficulty == StageManager.StageDifficulty.Normal)
            PlayGamesPlatform.Instance.ShowLeaderboardUI(GPGSIds.leaderboard_normal_mode_boss_killed_taken_time);

        else if (stageDifficulty == StageManager.StageDifficulty.Hard)
            PlayGamesPlatform.Instance.ShowLeaderboardUI(GPGSIds.leaderboard_hard_mode_boss_killed_taken_time);

        else if (stageDifficulty == StageManager.StageDifficulty.Hell)
            PlayGamesPlatform.Instance.ShowLeaderboardUI(GPGSIds.leaderboard_hell_mode_boss_killed_taken_time);
    }
    #endregion

    // �ػ� �����ϴ� �Լ� //
    public void SetScreenResolution()
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
}
