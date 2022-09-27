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
        _loadingUIController.loadingTypeString = "��Ʈ��ũ ���� Ȯ�� ��";

        // ���ͳݿ� ����Ǿ� �ִٸ� �α��� �õ�.
        if (GameManager.instance.CheckNetworkReachable())
            Invoke("Login", 2.5f);
        else
            _networkNotReachablePanelObject.SetActive(true);
    }

    public void Login()
    {
        _loadingUIController.loadingTypeString = "�α��� ��";
        GameManager.instance.Login(
            // Login�� �����ϸ� ���� Ŭ���忡�� ������ �ε带 �õ���.
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
        _loadingUIController.loadingTypeString = "������ �ҷ����� ��";

        GameManager.instance.Load(
            // ������ �ε��� �����ϸ� �κ���� �ε���.
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
        // �����͸� �ε��߿� ���� ��� ��� ������ �ε� �ݹ� �޼����� ���޾��� Ȯ���� �����Ƿ�,
        // �ε� ���� �ǳ��� Ȱ��ȭ �Ͽ� �ٽ� �ε��ϵ��� �Ѵ�.
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
