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
            // GameManager가 최초 생성되는 경우.
            // 씬이 종료되어도 파괴되지 않는 오브젝트로 설정한다.
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

        // 인터넷에 연결되지 않았고 waitDelay가 3초 미만일 경우 수행 
        while (Application.internetReachability == NetworkReachability.NotReachable && waitDelay < maximumDelay)
        {
            yield return null;
            waitDelay += Time.deltaTime;
        }

        // 3초동안 기다렸으나 인터넷 연결이 안됐을 경우 수행
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

        // 구글플레이 로그인 시도
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

    #region 스냅샷 불러오기
    private void OpenSavedGame(string filename)
    {
        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
        savedGameClient.OpenWithAutomaticConflictResolution(filename, DataSource.ReadCacheOrNetwork,
            ConflictResolutionStrategy.UseLongestPlaytime, OnSavedGameOpened);
    }

    private void OnSavedGameOpened(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        Debug.LogFormat("OnSavedGameOpened?:?{0},?{1}", status, _isSaving);
        // 스냅샷을 여는데 성공했을 경우 수행
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
            Debug.Log("시냅샷 불러오기 실패");
            Debug.Log(status.ToString());
            // 스냅샷 불러오기 실패.
        }
    }
    #endregion

    #region 데이터 로드
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
            // 받아온 데이터를 string 형태로 인코딩 하고, MyGameData 클래스로 변환한다.
            string myGameDataString = Encoding.UTF8.GetString(loadedData);

            // gameDataString 문자열이 빈 문자열이라면 (저장된 데이터가 없다면)
            // 기본생성자를 호출하여 기본값으로 설정한다.
            if (myGameDataString == "")
                playerGameData.SetDefaultValue();
            // 저장된 데이터가 있다면 데이터 값을 대입한다.
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

    #region 데이터 세이브
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

    // 해상도 설정하는 함수 //
    public void SetScreenResolution()
    {
        int setWidth = 1440; // 사용자 설정 너비
        int setHeight = 3200; // 사용자 설정 높이

        int deviceWidth = Screen.width; // 기기 너비 저장
        int deviceHeight = Screen.height; // 기기 높이 저장

        Screen.SetResolution(setWidth, (int)(((float)deviceHeight / deviceWidth) * setWidth), true);

        //화면을 정중앙으로 맞춤.
        if ((float)setWidth / setHeight < (float)deviceWidth / deviceHeight) // 기기의 해상도 비가 더 큰 경우
        {
            float newWidth = ((float)setWidth / setHeight) / ((float)deviceWidth / deviceHeight); // 새로운 너비
            Camera.main.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f); // 새로운 Rect 적용
        }
        else // 게임의 해상도 비가 더 큰 경우
        {
            float newHeight = ((float)deviceWidth / deviceHeight) / ((float)setWidth / setHeight); // 새로운 높이
            Camera.main.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight); // 새로운 Rect 적용
        }
    }
}
