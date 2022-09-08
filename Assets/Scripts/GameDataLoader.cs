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
        _loadingUIController.loadingTypeString = "��Ʈ��ũ ���� Ȯ�� ��";
        GameManager.instance.CheckNetworkReachable(
            // ���ͳ� ������ Ȯ�εǸ� Login�� �õ���.
            () => Login(),
            () => _networkNotReachablePanelObject.SetActive(true)
            );
    }

    public void Login()
    {
        _loadingUIController.loadingTypeString = "�α��� ��";
        GameManager.instance.Login(
            // Login�� �����ϸ� ���� Ŭ���忡�� ������ �ε带 �õ���.
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
        _loadingUIController.loadingTypeString = "������ �ҷ����� ��";

        GameManager.instance.Load(
            // ������ �ε��� �����ϸ� �κ���� �ε���.
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
