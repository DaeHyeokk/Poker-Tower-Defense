using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GoogleMobileAds.Api;
using TMPro;
public class LobbyManager : MonoBehaviour
{
    private static LobbyManager _instance;
    public static LobbyManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<LobbyManager>();
                return _instance;
            }
            return _instance;
        }
    }

    [SerializeField]
    private Button _loginButton;
    [SerializeField]
    private TextMeshProUGUI _loginText;
    [SerializeField]
    private LoadingUIController _loadingUIController;

    public LoadingUIController loadingUIController => _loadingUIController;

    private void Awake()
    {
        if (instance != this)
            Destroy(gameObject);

        Time.timeScale = 1f;

        if (Social.localUser.authenticated)
            _loginText.text = "�α׾ƿ�";
        else
            _loginText.text = "�α���";
    }

    private void Start()
    {

        UIManager.instance.GameStartScreenCoverFadeOut();
        SoundManager.instance.PlayBGM(SoundFileNameDictionary.mainBGM);
        LoadGoogleAds();
    }

    public void LoadGoogleAds()
    {

        // �ϴܿ� ���� ��� ���� ���.
        GoogleAdsManager.instance.LoadBanner(GoogleAdsManager.BannerAdSizeType.Standard);
    }

    public void OnClickLoginButton()
    {
        if (Social.localUser.authenticated)
        {
            UIManager.instance.actionReconfirmation.actionReconfirmationText.text = "�ʱ�ȭ������ �ǵ��ư��ϴ�.\n�α׾ƿ� �Ͻðڽ��ϱ�?";
            UIManager.instance.actionReconfirmation.onYesButton += () =>
            {
                GameManager.instance.Logout();
                SceneManager.LoadScene("GameLoadingScene");
            };
        }
        else
        {
            UIManager.instance.actionReconfirmation.actionReconfirmationText.text = "�ʱ�ȭ������ �ǵ��ư��ϴ�.\n�α��� �Ͻðڽ��ϱ�?";
            UIManager.instance.actionReconfirmation.onYesButton += () => SceneManager.LoadScene("GameLoadingScene");
        }

        UIManager.instance.actionReconfirmation.gameObject.SetActive(true);
    }

    public void OnClickGameQuitButton()
    {
        UIManager.instance.actionReconfirmation.actionReconfirmationText.text = "������ \n�����Ͻðڽ��ϱ�?";
        UIManager.instance.actionReconfirmation.onYesButton += QuitApplication;
        UIManager.instance.actionReconfirmation.gameObject.SetActive(true);
    }

    private void QuitApplication()
    {
        Application.Quit();
    }
}
