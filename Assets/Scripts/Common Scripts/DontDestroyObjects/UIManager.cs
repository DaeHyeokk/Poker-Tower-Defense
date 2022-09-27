using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<UIManager>();

            return _instance;
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
            // UIManager가 최초 생성되는 경우.
            // 씬이 종료되어도 파괴되지 않는 오브젝트로 설정한다.
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
        // 새로 로드한 씬에서 ActionReconfirmation 오브젝트를 찾는다.
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
