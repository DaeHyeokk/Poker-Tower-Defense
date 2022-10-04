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
        _loadingUIController.loadingTypeString = "��Ʈ��ũ ���� Ȯ�� ��";

        // ���ͳݿ� ����Ǿ� �ִٸ� ���� ������ ��ġ�ϴ��� Ȯ��.
        if (GameManager.instance.CheckNetworkReachable())
            Invoke("CheckGameVersion", 1f);
        else
            _networkNotReachablePanelObject.SetActive(true);
    }

    private void CheckGameVersion()
    {
        _loadingUIController.loadingTypeString = "���� ���� Ȯ�� ��";
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

    private void Load()
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
