using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class LobbyManager : MonoBehaviour
{
    private static LobbyManager s_instance;
    public static LobbyManager instance
    {
        get
        {
            if (s_instance == null)
            {
                s_instance = FindObjectOfType<LobbyManager>();
                return s_instance;
            }
            return s_instance;
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
        else
        {
            Screen.sleepTimeout = SleepTimeout.SystemSetting;

            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02f;

            if (Social.localUser.authenticated)
                _loginText.text = "�α׾ƿ�";
            else
                _loginText.text = "�α���";
        }
    }

    private void Start()
    {
        UIManager.instance.GameStartScreenCoverFadeOut();
        SoundManager.instance.PlayBGM(SoundFileNameDictionary.mainBGM);
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
