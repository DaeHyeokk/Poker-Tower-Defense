using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class GameDataLoader : MonoBehaviour
{
    [SerializeField]
    private GameObject _networkNotReachablePanelObject;
    [SerializeField]
    private GameObject _playGamesLoginFailedPanelObject;
    [SerializeField]
    private GameObject _dataLoadFailedPanelObject;
    [SerializeField]
    private GameObject _gameUpdatePanelObject;
    [SerializeField]
    private LoadingUIController _loadingUIController;
    [SerializeField]
    private TMPro.TextMeshProUGUI _versionText;

    private readonly string _versionSheetURL = "https://docs.google.com/spreadsheets/d/1U5EGMo1ADAkYTdkzq3QVmrKvyVsAi6KYI9rP4v_m4pE/export?format=tsv";
    private readonly string _googlePlayURL = "https://play.google.com/store/apps/details?id=com.devdduck.pokertowerdefense";

    private void Start()
    {
        _versionText.text = 'v' + Application.version;
        SoundManager.instance.PauseBGM();
        LoadPlayerGameData();
    }

    private void LoadPlayerGameData()
    {
        CheckNetworkReachable();
    }

    private void CheckNetworkReachable()
    {
        _loadingUIController.loadingTypeString = "네트워크 상태 확인 중";

        // 인터넷에 연결되어 있다면 게임 버전이 일치하는지 확인.
        if (GameManager.instance.CheckNetworkReachable())
            Invoke("CheckGameVersion", 1f);
        else
            _networkNotReachablePanelObject.SetActive(true);
    }

    private void CheckGameVersion()
    {
        _loadingUIController.loadingTypeString = "게임 버전 확인 중";
        StartCoroutine(CheckGameVersionCoroutine());
    }

    private IEnumerator CheckGameVersionCoroutine()
    {
        UnityWebRequest www = UnityWebRequest.Get(_versionSheetURL);
        yield return www.SendWebRequest();

        string versionData = www.downloadHandler.text;

        if (Application.version == versionData)
            Login();
        else
            _gameUpdatePanelObject.SetActive(true);
    }

    private void Login()
    {
        _loadingUIController.loadingTypeString = "로그인 중";
        GameManager.instance.Login(
            // Login이 성공하면 구글 클라우드에서 데이터 로드를 시도함.
            () => 
            {
                Load(); 
            },
            () =>
            {
                _playGamesLoginFailedPanelObject.SetActive(true);
            });
    }

    private bool _isLoading;

    private void Load()
    {
        _isLoading = true;
        _loadingUIController.loadingTypeString = "데이터 불러오기 중";

        GameManager.instance.Load(
            // 데이터 로딩이 성공하면 로비씬을 로드함.
            () => 
            {
                SceneManager.LoadScene("LobbyScene");
            },
            () =>
            {
                _dataLoadFailedPanelObject.SetActive(true);
            });
    }

    private void OnApplicationPause(bool pause)
    {
        // 데이터를 로딩중에 앱을 벗어날 경우 데이터 로드 콜백 메세지를 못받았을 확률이 높으므로,
        // 로드 실패 판넬을 활성화 하여 다시 로드하도록 한다.
        if (pause && _isLoading)
            _dataLoadFailedPanelObject.SetActive(true);
    }

    public void OnClickGameUpdateButton()
    {
        Application.OpenURL(_googlePlayURL);
    }

    public void OnClickOfflineStartButton()
    {
        GameManager.instance.playerGameData.SetDefaultValue();
        SceneManager.LoadScene("LobbyScene");
    }

}
