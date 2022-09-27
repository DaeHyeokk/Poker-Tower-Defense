using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    private LoadingUIController _loadingUIController;

    private void Start()
    {
        SoundManager.instance.PauseBGM();
        LoadPlayerGameData();
    }

    private void LoadPlayerGameData()
    {
        CheckNetworkReachable();
    }

    public void CheckNetworkReachable()
    {
        _loadingUIController.loadingTypeString = "네트워크 상태 확인 중";

        // 인터넷에 연결되어 있다면 로그인 시도.
        if (GameManager.instance.CheckNetworkReachable())
            Invoke("Login", 2.5f);
        else
            _networkNotReachablePanelObject.SetActive(true);
    }

    public void Login()
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

    public void Load()
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
        {
            _dataLoadFailedPanelObject.SetActive(true);
        }
    }

    public void OnClickOfflineStartButton()
    {
        GameManager.instance.playerGameData.SetDefaultValue();
        SceneManager.LoadScene("LobbyScene");
    }

}
