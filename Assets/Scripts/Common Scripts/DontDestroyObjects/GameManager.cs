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
            // GameManager�� ���� �����Ǵ� ���.
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
        SetScreenResolution();
        UIManager.instance.GameStartScreenCoverFadeOut();
    }

    // �ػ� �����ϴ� �Լ� //
    private void SetScreenResolution()
    {
        int setWidth = 1440; // ����� ���� �ʺ�
        int setHeight = 3200; // ����� ���� ����

        int deviceWidth = Screen.width; // ��� �ʺ� ����
        int deviceHeight = Screen.height; // ��� ���� ����

        Screen.SetResolution(setWidth, (int)(((float)deviceHeight / deviceWidth) * setWidth), true);

        //ȭ���� ���߾����� ����.
        if ((float)setWidth / setHeight < (float)deviceWidth / deviceHeight) // ����� �ػ� �� �� ū ���
        {
            float newWidth = ((float)setWidth / setHeight) / ((float)deviceWidth / deviceHeight); // ���ο� �ʺ�
            Camera.main.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f); // ���ο� Rect ����
        }
        else // ������ �ػ� �� �� ū ���
        {
            float newHeight = ((float)deviceWidth / deviceHeight) / ((float)setWidth / setHeight); // ���ο� ����
            Camera.main.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight); // ���ο� Rect ����
        }
    }

    public void SetStageDifficulty(StageDifficulty stageDifficulty) => this.stageDifficulty = stageDifficulty;
    
}
