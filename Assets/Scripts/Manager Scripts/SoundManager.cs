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
        // 원래 씬과 새로 로드하는 씬이 서로 다른 씬일 경우에만 리소스 파일을 로딩한다.
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
            // 동일한 오디오가 10개 이상 중첩 재생중이라면 재생하지 않는다.
            if (_sfxAudioCountDict[audioFileName] >= 10)
                return;

            _sfxAudioCountDict[audioFileName]++;

                _sfxAudioSource.PlayOneShot(audioClip, _sfxAudioSource.volume);

            // 오디오 출력이 종료되면 카운트를 감소시키기 위한 코루틴 메소드.
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
