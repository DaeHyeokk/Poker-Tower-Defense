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
    private GameObject _sfxAudioSourcePrefab;
    private ObjectPool<SFXAudioSource> _sfxAudioSourcePool;

    private Scene _currentScene;

    private Dictionary<string, AudioClip> _bgmAudioClipDict = new();
    private Dictionary<string, AudioClip> _sfxAudioClipDict = new();
    private Dictionary<AudioClip, Queue<SFXAudioSource>> _sfxPlayingAudioClipDict = new();

    public AudioSource bgmAudioSource => _bgmAudioSource;
    public float sfxVolume { get; set; } = 1f;
    public bool isSfxMuted { get; set; }

    private void Awake()
    {
        if (instance != this)
            Destroy(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += LoadSceneAction;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= LoadSceneAction;
    }

    private void LoadSceneAction(Scene scene, LoadSceneMode mode)
    {
        // 원래 씬과 새로 로드하는 씬이 서로 다른 씬일 경우에만 리소스 파일을 로딩한다.
        if (instance._currentScene != scene)
        {
            instance.LoadSceneSoundResource();
            instance._currentScene = scene;
        }

        _sfxAudioSourcePool = new(_sfxAudioSourcePrefab, 10);
        instance.PlayBGM("Main BGM");
    }

    private void LoadSceneSoundResource()
    {
        AudioClip[] bgmAudioClips;
        AudioClip[] sfxAudioClips;

        _bgmAudioClipDict.Clear();
        _sfxAudioClipDict.Clear();
        _sfxPlayingAudioClipDict.Clear();

        switch(SceneManager.GetActiveScene().name)
        {
            case "SingleModeScene":
                bgmAudioClips = Resources.LoadAll<AudioClip>("Sounds/Stage/BGM");
                sfxAudioClips = Resources.LoadAll<AudioClip>("Sounds/Stage/SFX");
                break;

            case "LobbyScene":
                bgmAudioClips = Resources.LoadAll<AudioClip>("Sounds/Lobby/BGM");
                sfxAudioClips = Resources.LoadAll<AudioClip>("Sounds/Lobby/SFX");
                break;

            default:
                bgmAudioClips = null;
                sfxAudioClips = null;
                break;
        }

        foreach (AudioClip audioClip in bgmAudioClips)
            _bgmAudioClipDict.Add(audioClip.name, audioClip);

        foreach (AudioClip audioClip in sfxAudioClips)
        {
            _sfxAudioClipDict.Add(audioClip.name, audioClip);
            _sfxPlayingAudioClipDict.Add(audioClip, new Queue<SFXAudioSource>());
        }
    }

    public void PlayBGM(string audioFileName)
    {
        AudioClip audioClip;

        if (!_bgmAudioClipDict.TryGetValue(audioFileName, out audioClip))
        {
            Debug.Log("할당되지 않은 오디오 파일을 참조하려 합니다.");
        }
        else
        {
            // 전에 플레이 중이던 BGM을 정지한다.
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
            Debug.Log("할당되지 않은 오디오 파일을 참조하려 합니다.");
        }
        else
        {
            // 동일한 오디오가 5개 이상 중첩 재생중이라면 가장 먼저 플레이한 사운드를 중지한다.
            if (_sfxPlayingAudioClipDict[audioClip].Count >= 5)
                ReturnAudioSource(audioClip);

            SFXAudioSource sfxAudioSource = _sfxAudioSourcePool.GetObject();
            _sfxPlayingAudioClipDict[audioClip].Enqueue(sfxAudioSource);
            sfxAudioSource.audioSource.clip = audioClip;
            sfxAudioSource.audioSource.Play();
        }
    }

    public void ReturnAudioSource(AudioClip audioClip)
    {
        SFXAudioSource sfxAudioSource = _sfxPlayingAudioClipDict[audioClip].Dequeue();
        sfxAudioSource.audioSource.Stop();
        _sfxAudioSourcePool.ReturnObject(sfxAudioSource);
    }
}
