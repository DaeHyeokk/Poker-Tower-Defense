using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;

[Serializable]
public class PlayerGameData
{
    public bool isPurchasedRemoveAds;
    public bool isPurchasedPremiumTicket;
    public bool isPurchased3xSpeed;
    public bool isBonusRewardActived;
    public List<PlayerStageData> playerStageDataList;
    public List<PlayerTowerData> playerTowerDataList;

    public PlayerGameData()
    {
        isPurchasedRemoveAds = false;
        isPurchasedPremiumTicket = false;
        isPurchased3xSpeed = false;
        isBonusRewardActived = false;
        playerStageDataList = new(4); // Easy, Normal, Hard, Hell
        playerTowerDataList = new(Tower.towerTypeNames.Length);

        for(int i=0; i<playerStageDataList.Capacity; i++)
        {
            int bestRecord;
            int clearCount;

            bestRecord = 0;
            clearCount = 0;

            playerStageDataList.Add(new(bestRecord, clearCount));
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
        isPurchasedRemoveAds = false;
        isPurchasedPremiumTicket = false;
        isPurchased3xSpeed = false;
        isBonusRewardActived = false;

        for (int i = 0; i < playerStageDataList.Capacity; i++)
        {
            int bestRecord;
            int clearCount;

            bestRecord = 0;
            clearCount = 0;

            playerStageDataList[i] = new(bestRecord, clearCount);
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
    public int bestRecord;
    public int clearCount;

    public PlayerStageData(int bestRecord, int clearCount)
    {
        this.bestRecord = bestRecord;
        this.clearCount = clearCount;
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
    private static GameManager _instance;
    public static GameManager instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<GameManager>();

            return _instance;
        }
    }

    public PlayerGameData playerGameData { get; set; } = new();
    public bool isLogin { get; set; }

    private event Action onSuccessed;
    private event Action onFailed;

    private void Awake()
    {
        if (instance != this)
            Destroy(gameObject);
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

    private void OnApplicationPause()
    {
        if(isLogin)
            Save();
    }

    public void CheckNetworkReachable(Action onSuccessed = null, Action onFailed = null)
    {
        if (this.onSuccessed != null)
            this.onSuccessed = null;
        this.onSuccessed += onSuccessed;

        if (this.onFailed != null)
            this.onFailed = null;
        this.onFailed += onFailed;

        StartCoroutine(CheckNetworkReachableCoroutine());
    }

    private IEnumerator CheckNetworkReachableCoroutine()
    {
        float waitDelay = 0f;
        float maximumDelay = 3f;

        yield return new WaitForSeconds(1f);

        // ���ͳݿ� ������� �ʾҰ� waitDelay�� 3�� �̸��� ��� ���� 
        while (Application.internetReachability == NetworkReachability.NotReachable && waitDelay < maximumDelay)
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
        Debug.LogFormat("OnSavedGameOpened?:?{0},?{1}", status, _isSaving);
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
            Debug.Log("�ó��� �ҷ����� ����");
            Debug.Log(status.ToString());
            // ������ �ҷ����� ����.
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
            // �⺻�����ڸ� ȣ���Ͽ� �⺻������ �����Ѵ�.
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
