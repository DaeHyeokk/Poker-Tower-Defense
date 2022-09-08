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
            _loginText.text = "로그아웃";
        else
            _loginText.text = "로그인";
    }

    private void Start()
    {

        UIManager.instance.GameStartScreenCoverFadeOut();
        SoundManager.instance.PlayBGM(SoundFileNameDictionary.mainBGM);
        LoadGoogleAds();
    }

    public void LoadGoogleAds()
    {

        // 하단에 구글 배너 광고를 띄움.
        GoogleAdsManager.instance.LoadBanner(GoogleAdsManager.BannerAdSizeType.Standard);
    }

    public void OnClickLoginButton()
    {
        if (Social.localUser.authenticated)
        {
            UIManager.instance.actionReconfirmation.actionReconfirmationText.text = "초기화면으로 되돌아갑니다.\n로그아웃 하시겠습니까?";
            UIManager.instance.actionReconfirmation.onYesButton += () =>
            {
                GameManager.instance.Logout();
                SceneManager.LoadScene("GameLoadingScene");
            };
        }
        else
        {
            UIManager.instance.actionReconfirmation.actionReconfirmationText.text = "초기화면으로 되돌아갑니다.\n로그인 하시겠습니까?";
            UIManager.instance.actionReconfirmation.onYesButton += () => SceneManager.LoadScene("GameLoadingScene");
        }

        UIManager.instance.actionReconfirmation.gameObject.SetActive(true);
    }

    public void OnClickGameQuitButton()
    {
        UIManager.instance.actionReconfirmation.actionReconfirmationText.text = "게임을 \n종료하시겠습니까?";
        UIManager.instance.actionReconfirmation.onYesButton += QuitApplication;
        UIManager.instance.actionReconfirmation.gameObject.SetActive(true);
    }

    private void QuitApplication()
    {
        Application.Quit();
    }
}
