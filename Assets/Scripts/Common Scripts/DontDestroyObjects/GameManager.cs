using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<GameManager>();

            return _instance;
        }
    }
    public enum StageDifficulty { Easy, Normal, Hard, Hell }

    public StageDifficulty stageDifficulty { get; private set; }

    private void Awake()
    {
        if (instance != this)
            Destroy(gameObject);
        else
        {
            // GameManager가 최초 생성되는 경우.
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
        SetScreenResolution();
        UIManager.instance.GameStartScreenCoverFadeOut();
    }

    // 해상도 설정하는 함수 //
    private void SetScreenResolution()
    {
        int setWidth = 1440; // 사용자 설정 너비
        int setHeight = 3200; // 사용자 설정 높이

        int deviceWidth = Screen.width; // 기기 너비 저장
        int deviceHeight = Screen.height; // 기기 높이 저장

        Screen.SetResolution(setWidth, (int)(((float)deviceHeight / deviceWidth) * setWidth), true);

        //화면을 정중앙으로 맞춤.
        if ((float)setWidth / setHeight < (float)deviceWidth / deviceHeight) // 기기의 해상도 비가 더 큰 경우
        {
            float newWidth = ((float)setWidth / setHeight) / ((float)deviceWidth / deviceHeight); // 새로운 너비
            Camera.main.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f); // 새로운 Rect 적용
        }
        else // 게임의 해상도 비가 더 큰 경우
        {
            float newHeight = ((float)deviceWidth / deviceHeight) / ((float)setWidth / setHeight); // 새로운 높이
            Camera.main.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight); // 새로운 Rect 적용
        }
    }

    public void SetStageDifficulty(StageDifficulty stageDifficulty) => this.stageDifficulty = stageDifficulty;
    
}
