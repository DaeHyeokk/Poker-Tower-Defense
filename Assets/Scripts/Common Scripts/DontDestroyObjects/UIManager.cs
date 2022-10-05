using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private static UIManager s_instance;
    public static UIManager instance
    {
        get
        {
            if (s_instance == null)
                s_instance = FindObjectOfType<UIManager>();

            return s_instance;
        }
    }

    [SerializeField]
    private ScreenCover _screenCover;
    public ActionReconfirmation _actionReconfirmation;

    public ScreenCover screenCover => _screenCover;
    public ActionReconfirmation actionReconfirmation => _actionReconfirmation;

    private void Awake()
    {
        if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            // UIManager�� ���� �����Ǵ� ���.
            // ���� ����Ǿ �ı����� �ʴ� ������Ʈ�� �����Ѵ�.
            DontDestroyOnLoad(instance);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLoadScene;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLoadScene;
    }

    private void OnLoadScene(Scene scene, LoadSceneMode mode)
    {
        // ���� �ε��� ������ ActionReconfirmation ������Ʈ�� ã�´�.
        _actionReconfirmation = FindObjectOfType<ActionReconfirmation>(true);
    }

    public void GameStartScreenCoverFadeOut()
    {
        _screenCover.gameObject.SetActive(true);
        _screenCover.FadeOut(Color.black, 0.5f);
    }

    public void FillTheScreen()
    {
        _screenCover.gameObject.SetActive(true);
        _screenCover.FillTheScreen();
    }
}
