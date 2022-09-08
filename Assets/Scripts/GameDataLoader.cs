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
        GameManager.instance.CheckNetworkReachable(
            // 인터넷 연결이 확인되면 Login을 시도함.
            () => Login(),
            () => _networkNotReachablePanelObject.SetActive(true)
            );
    }

    public void Login()
    {
        _loadingUIController.loadingTypeString = "로그인 중";
        GameManager.instance.Login(
            // Login이 성공하면 구글 클라우드에서 데이터 로드를 시도함.
            () => 
            {
                GameManager.instance.isLogin = true;
                Load(); 
            },
            () => _playGamesLoginFailedPanelObject.SetActive(true)
            );

    }

    public void Load()
    {
        _loadingUIController.loadingTypeString = "데이터 불러오기 중";

        GameManager.instance.Load(
            // 데이터 로딩이 성공하면 로비씬을 로드함.
            () =>SceneManager.LoadScene("LobbyScene"),
            () => _dataLoadFailedPanelObject.SetActive(true)
            );
    }

    public void OnClickOfflineStartButton()
    {
        GameManager.instance.playerGameData.SetDefaultValue();
        GameManager.instance.isLogin = false;

        SceneManager.LoadScene("LobbyScene");
    }

}
