using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    private static SoundManager _instance;
    public static SoundManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<SoundManager>();
                DontDestroyOnLoad(_instance.gameObject);
            }

            return _instance;
        }
    }

    [SerializeField]
    private AudioSource _bgmAudioSource;
    [SerializeField]
    private AudioSource _sfxAudioSource;

    private Scene _currentScene;

    private Dictionary<string, AudioClip> _bgmAudioClipDict = new();
    private Dictionary<string, AudioClip> _sfxAudioClipDict = new();
    private Dictionary<string, int> _sfxAudioCountDict = new();

    public AudioSource bgmAudioSource => _bgmAudioSource;
    public AudioSource sfxAudioSource => _sfxAudioSource;

    private void Awake()
    {
        if (instance != this)
            Destroy(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoad;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoad;
    }

    private void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        // ���� ���� ���� �ε��ϴ� ���� ���� �ٸ� ���� ��쿡�� ���ҽ� ������ �ε��Ѵ�.
        if (instance._currentScene != scene)
        {
            instance.LoadSceneSoundResource();
            instance._currentScene = scene;
        }

        instance.PlayBGM("Main BGM");
    }

    private void LoadSceneSoundResource()
    {
        AudioClip[] audioClips;

        _bgmAudioClipDict.Clear();
        _sfxAudioClipDict.Clear();
        _sfxAudioCountDict.Clear();

        switch(SceneManager.GetActiveScene().name)
        {
            case "SingleModeScene":
                audioClips = Resources.LoadAll<AudioClip>("Sounds/Stage/BGM");
                foreach(AudioClip audioClip in audioClips)
                    _bgmAudioClipDict.Add(audioClip.name, audioClip);

                audioClips = Resources.LoadAll<AudioClip>("Sounds/Stage/SFX");
                foreach(AudioClip audioClip in audioClips)
                {
                    _sfxAudioClipDict.Add(audioClip.name, audioClip);
                    _sfxAudioCountDict.Add(audioClip.name, 0);
                }
                break;

            case "LobbyScene":
                audioClips = Resources.LoadAll<AudioClip>("Sounds/Lobby/BGM");
                foreach(AudioClip audioClip in audioClips)
                    _bgmAudioClipDict.Add(audioClip.name, audioClip);

                audioClips = Resources.LoadAll<AudioClip>("Sounds/Lobby/SFX");
                foreach(AudioClip audioClip in audioClips)
                {
                    _sfxAudioClipDict.Add(audioClip.name, audioClip);
                    _sfxAudioCountDict.Add(audioClip.name, 0);
                }
                break;
        }
    }

    public void PlayBGM(string audioFileName)
    {
        AudioClip audioClip;

        if (!_bgmAudioClipDict.TryGetValue(audioFileName, out audioClip))
        {
            Debug.Log("�Ҵ���� ���� ����� ������ �����Ϸ� �մϴ�.");
        }
        else
        {
            // ���� �÷��� ���̴� BGM�� �����Ѵ�.
            _bgmAudioSource.Stop();

            _bgmAudioSource.clip = audioClip;

            _bgmAudioSource.Play();
        }
    }
    
    public void PauseBGM()
    {
        if(_bgmAudioSource.isPlaying)
            _bgmAudioSource.Pause();
    }

    public void ResumeBGM()
    {
        if (!_bgmAudioSource.isPlaying)
            _bgmAudioSource.Play();
    }

    public void PlaySFX(string audioFileName)
    {
        AudioClip audioClip;

        if (!_sfxAudioClipDict.TryGetValue(audioFileName, out audioClip))
        {
            Debug.Log("�Ҵ���� ���� ����� ������ �����Ϸ� �մϴ�.");
        }
        else
        {
            // ������ ������� 10�� �̻� ��ø ������̶�� ������� �ʴ´�.
            if (_sfxAudioCountDict[audioFileName] >= 10)
                return;

            _sfxAudioCountDict[audioFileName]++;

                _sfxAudioSource.PlayOneShot(audioClip, _sfxAudioSource.volume);

            // ����� ����� ����Ǹ� ī��Ʈ�� ���ҽ�Ű�� ���� �ڷ�ƾ �޼ҵ�.
            StartCoroutine(DecreaseAudioCountCoroutine(audioFileName, audioClip.length));
        }
    }

    private IEnumerator DecreaseAudioCountCoroutine(string audioFileName, float delay)
    {
        while(delay > 0f)
        {
            yield return null;
            delay -= Time.deltaTime;
        }

        _sfxAudioCountDict[audioFileName]--;
    }
}
